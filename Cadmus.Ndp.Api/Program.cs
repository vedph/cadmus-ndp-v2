using Cadmus.Api.Config;
using Cadmus.Api.Config.Services;
using Cadmus.Api.Controllers;
using Cadmus.Api.Services;
using Cadmus.Api.Services.Seeding;
using Cadmus.Core;
using Cadmus.Core.Config;
using Cadmus.Ndp.Api.Services;
using Cadmus.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mol.Api.Controllers;
using Mufi.Core;
using Mufi.LiteDB;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using TaxoStore.Api.Controllers;
using TaxoStore.Api.Controllers.Services;

namespace Cadmus.Ndp.Api;

/// <summary>
/// Program.
/// </summary>
public static class Program
{
    // startup log file name, Serilog is configured later via appsettings.json
    private const string STARTUP_LOG_NAME = "startup.log";

    private static void ConfigureAppServices(IServiceCollection services,
        IConfiguration config)
    {
        // Cadmus repository
        string dataCS = string.Format(
        config.GetConnectionString("Default")!,
            config.GetValue<string>("DatabaseNames:Data"));
        services.AddSingleton<IRepositoryProvider>(
            _ => new AppRepositoryProvider { ConnectionString = dataCS });

        // part seeder factory provider
        services.AddSingleton<IPartSeederFactoryProvider,
            AppPartSeederFactoryProvider>();

        // item browser factory provider
        services.AddSingleton<IItemBrowserFactoryProvider>(_ =>
        new StandardItemBrowserFactoryProvider(
                config.GetConnectionString("Default")!));

        // metadata builder factory provider
        services.AddSingleton<IItemMetadataBuilderFactoryProvider>(_ =>
            new StandardItemMetadataBuilderFactoryProvider(
                config.GetConnectionString("Default")!));

        // index and graph
        ServiceConfigurator.ConfigureIndexServices(services, config);
        ServiceConfigurator.ConfigureGraphServices(services, config);

        // previewer
        services.AddSingleton(p => ServiceConfigurator.GetPreviewer(p, config));

        // MOL (embedded default data)
        services.AddMolServices(options =>
        {
            options.Provider = MolDatabaseProvider.PostgreSQL;
            options.ConnectionString = config.GetConnectionString("Mol");
            Console.WriteLine("MOL connection string: " +
                options.ConnectionString);
        });

        // MUFI (from wwwroot/mufi.db)
        services.AddSingleton<IMufiRepository>(_ =>
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            return new LiteDBMufiRepository(
                new MufiRepositoryOptions
                {
                    Source = Path.Combine(path, "mufi.db")
                });
        });

        // TaxoStore (from wwwroot/taxo/trees.csv and nodes.csv)
        services.AddTaxoStoreServices(options =>
        {
            string? connectionString = config.GetConnectionString("TaxoStore");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'TaxoStore' not found.");
            }

            options.ConnectionString = connectionString;

            options.EnableAutoInitialization =
                config.GetValue("TaxoStore:EnableAutoInitialization", true);

            options.InitializationDelaySeconds =
                config.GetValue("TaxoStore:InitializationDelaySeconds", 0);
        });
    }

    /// <summary>
    /// Entry point.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public static async Task<int> Main(string[] args)
    {
        // early startup logging to ensure we catch any exceptions
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
#if DEBUG
            .WriteTo.File(STARTUP_LOG_NAME, rollingInterval: RollingInterval.Day)
#endif
            .CreateLogger();

        try
        {
            Log.Information("Starting Cadmus API host");
            ServiceConfigurator.DumpEnvironmentVars();

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            ServiceConfigurator.ConfigureLogger(builder);

            IConfiguration config = new ConfigurationService(builder.Environment)
                .Configuration;

            ServiceConfigurator.ConfigureServices(builder.Services, config,
                builder.Environment);

            ConfigureAppServices(builder.Services, config);

            builder.Services.AddOpenApi();

            // controllers from libraries
            builder.Services.AddControllers()
                // Cadmus.Api.Controllers
                .AddApplicationPart(typeof(ItemController).Assembly)
                // Mol.Api.Controllers
                .AddApplicationPart(typeof(MolController).Assembly)
                // Taxo
                .AddApplicationPart(typeof(TaxoTreeController).Assembly)
                .AddControllersAsServices();

            WebApplication app = builder.Build();

            // https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer
            // Forwarded headers (X-Forwarded-For, X-Forwarded-Proto). This must
            // come first, so that everything after it sees the request as the
            // client sent it to the reverse proxy:
            // - X-Forwarded-For restores the client IP address, which the rate
            //   limiter uses to partition anonymous requests (otherwise all of
            //   them would share the proxy's IP and its quota);
            // - X-Forwarded-Proto restores the original scheme (https), which
            //   HTTPS redirection and the OpenAPI document servers URL use
            //   (otherwise Scalar calls http:// from an https:// page, and the
            //   browser blocks it as mixed content).
            // The headers are honored only when sent by a trusted proxy.
            // Localhost is always trusted; appsettings.json also trusts the
            // private networks (10/8, 172.16/12, 192.168/16, fc00::/7), which
            // covers a dockerized proxy on a Docker network. To override it
            // without rebuilding the image, use these environment variables:
            // - ForwardedHeaders__KnownNetworks__0=172.18.0.0/16 (and __1, ...)
            //   to trust only specific networks (CIDR);
            // - ForwardedHeaders__KnownProxies__0=172.18.0.5 to trust specific
            //   proxy IP addresses;
            // - ForwardedHeaders__TrustAllProxies=true to trust any sender (only
            //   when the API port can be reached only via the proxy);
            // - ForwardedHeaders__ForwardLimit=2 when there are 2 proxies in
            //   a chain (e.g. a CDN in front of nginx).
            // Do not also set ASPNETCORE_FORWARDEDHEADERS_ENABLED=true: that
            // adds another forwarded headers middleware, which trusts any
            // sender and bypasses this configuration.
            ServiceConfigurator.UseForwardedHeaders(app, config);

            // development or production
            if (builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-5.0&tabs=visual-studio
                app.UseExceptionHandler("/Error");
                if (config.GetValue<bool>("Server:UseHSTS"))
                {
                    Console.WriteLine("HSTS: yes");
                    app.UseHsts();
                }
                else
                {
                    Console.WriteLine("HSTS: no");
                }
            }

            // HTTPS redirection
            if (config.GetValue<bool>("Server:UseHttpsRedirection"))
            {
                Console.WriteLine("HttpsRedirection: yes");
                app.UseHttpsRedirection();
            }
            else
            {
                Console.WriteLine("HttpsRedirection: no");
            }

            // CORS
            app.UseCors("CorsPolicy");
            // authentication
            app.UseAuthentication();
            // rate limiter (unless RateLimit:IsDisabled): after authentication,
            // so that its partition key can be the authenticated user name
            ServiceConfigurator.UseRateLimiter(app, config);
            app.UseAuthorization();
            // proxy
            app.UseResponseCaching();

            // seed auth database (via Services/HostAuthSeedExtensions)
            await app.SeedAuthAsync();

            // seed Cadmus database (via Services/HostSeedExtension)
            await app.SeedAsync();

            // map controllers and Scalar API
            app.MapControllers();
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.WithTitle("Cadmus NDP API")
                       .AddPreferredSecuritySchemes("Bearer");
            });

            Log.Information("Running API");
            await app.RunAsync();

            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Cadmus API host terminated unexpectedly");
            Debug.WriteLine(ex.ToString());
            Console.WriteLine(ex.ToString());
            return 1;
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}
