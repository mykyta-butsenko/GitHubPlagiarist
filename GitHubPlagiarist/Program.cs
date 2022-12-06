using GitHubPlagiarist.Configuration;
using GitHubPlagiarist.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace GitHubPlagiarist
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var configFileName = string.Empty;
            if (args.Length != 0)
            {
                configFileName = args[0];
            }

            IConfiguration configuration = BuildConfiguration(configFileName);
            ConfigureSerilog(configuration);

            try
            {
                await BuildHost(configuration).RunAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                Log.CloseAndFlush();
            }
        }

        public static IHost BuildHost(IConfiguration configuration)
        {
            IHostBuilder host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(builder => builder.AddConfiguration(configuration))
                .ConfigureServices(
                    services =>
                    {
                        string accessToken =
                            configuration["AccessToken"] ??
                            configuration["ACCESS_TOKEN"] ??
                            configuration["ApiToken"] ??
                            throw new InvalidOperationException("GitHub access token is not specified.");

                        services.AddTransient<IGitHubSeeker>(gitHubSearch => new GitHubSeeker(accessToken));
                        services.AddHostedService<GitHubSearchService>();
                    })
                .UseSerilog();

            return host.Build();
        }

        // ReSharper disable once IdentifierTypo
        private static void ConfigureSerilog(IConfiguration configuration)
        {
            var logFileName = "log.txt";
            if (configuration[nameof(SearchConfiguration.CustomerName)] != null
                && configuration[nameof(SearchConfiguration.ProjectName)] != null)
            {
                logFileName =
                    $"{configuration[nameof(SearchConfiguration.CustomerName)]}." +
                    $"{configuration[nameof(SearchConfiguration.ProjectName)]}.log.txt";
            }

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .WriteTo.File(logFileName)
                .CreateLogger();
        }

        private static IConfiguration BuildConfiguration(string configFileName)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables("GIT_HUB_SEARCH_");

            if (!string.IsNullOrEmpty(configFileName))
            {
                configurationBuilder.AddJsonFile(configFileName, true, true);
            }

            return configurationBuilder.Build();
        }
    }
}