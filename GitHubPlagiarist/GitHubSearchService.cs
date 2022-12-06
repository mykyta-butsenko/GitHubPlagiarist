using System.Diagnostics;
using GitHubPlagiarist.Configuration;
using GitHubPlagiarist.Interfaces;
using GitHubPlagiarist.Models;
using GitHubPlagiarist.ResultsHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace GitHubPlagiarist
{
    internal class GitHubSearchService : IHostedService
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IConfiguration _configuration;
        private readonly IGitHubSeeker _gitHubSeeker;

        public GitHubSearchService(
            IHostApplicationLifetime applicationLifetime,
            IGitHubSeeker gitHubSeeker,
            IConfiguration configuration)
        {
            _applicationLifetime = applicationLifetime;
            _gitHubSeeker = gitHubSeeker;
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _applicationLifetime.ApplicationStarted.Register(async () => await ExecuteAsync(cancellationToken));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
#if DEBUG
                var stopwatch = new Stopwatch();
                stopwatch.Start();
#endif

                var searchConfiguration = _configuration.Get<SearchConfiguration>();
                if (!ValidateConfiguration(searchConfiguration))
                {
                    _applicationLifetime.StopApplication();
                    return;
                }

                IEnumerable<Code> codeResults = (await _gitHubSeeker.SearchCodeAsync(searchConfiguration, cancellationToken));
                IResultsHandler<Code> resultHandlerChain = BuildCodeResultHandlerChain(searchConfiguration);

                foreach (Code codeResult in codeResults)
                {
                    resultHandlerChain.Handle(codeResult);
                }
#if DEBUG
                stopwatch.Stop();
                Log.Information(stopwatch.Elapsed.ToString());
#endif
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }

            _applicationLifetime.StopApplication();
        }

        /// <summary>
        /// Builds a chain of handlers to handle each of the results.
        /// </summary>
        /// <param name="searchConfiguration">Search configuration.</param>
        /// <returns>An instance of the <see cref="IResultsHandler{TResult}"/></returns>
        private static IResultsHandler<Code> BuildCodeResultHandlerChain(SearchConfiguration searchConfiguration)
        {
            var exactMatchFilter = new ExactMatchFilter(searchConfiguration.Keywords);
            var appendResultsToJsonHandler = new AppendResultsToJsonHandler(ResolveResultsFileFullName(searchConfiguration));

            if (string.IsNullOrEmpty(searchConfiguration.ExceptionsListFileName))
            {
                exactMatchFilter.SetNext(appendResultsToJsonHandler);

                return exactMatchFilter;
            }

            string exceptionsFileFullName = ResolveExceptionsListFileFullName(searchConfiguration.ExceptionsListFileName);

            IResultsHandler<Code> exceptionsFilter = exactMatchFilter.SetNext(new ExceptionsFilter(exceptionsFileFullName));

            if (searchConfiguration.AppendResultsToExceptionsFile)
            {
                exceptionsFilter.SetNext(new AppendExceptionHandler(exceptionsFileFullName))
                    .SetNext(appendResultsToJsonHandler);
            }
            else
            {
                exceptionsFilter.SetNext(appendResultsToJsonHandler);
            }

            return exactMatchFilter;
        }

        private static bool ValidateConfiguration(SearchConfiguration searchOptions)
        {
            var optionsValidator = new DataAnnotationValidateOptions<SearchConfiguration>(nameof(SearchConfiguration));

            ValidateOptionsResult validationResult = optionsValidator.Validate(
                nameof(SearchConfiguration),
                searchOptions);

            if (!validationResult.Failed)
            {
                return true;
            }

            Log.Error(string.Join("; ", validationResult.Failures));

            return false;
        }

        private static string ResolveExceptionsListFileFullName(string fileName)
        {
            return Path.IsPathRooted(fileName)
                ? fileName
                : Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Configuration",
                    fileName);
        }

        private static string ResolveResultsFileFullName(SearchConfiguration searchOptions)
        {
            string filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Results",
                searchOptions.CustomerName);

            Directory.CreateDirectory(filePath);

            var fileName = $"{searchOptions.ProjectName} - {searchOptions.Reviewer}";

            return Path.Combine(filePath, fileName);
        }
    }
}