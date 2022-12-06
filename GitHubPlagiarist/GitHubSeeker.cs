using GitHubPlagiarist.Configuration;
using GitHubPlagiarist.Interfaces;
using GitHubPlagiarist.Models;
using GitHubSearch.Resources;
using Octokit;
using Serilog;

namespace GitHubPlagiarist
{
    using Repository = Models.Repository;
    using SearchCode = Models.Octokit.SearchCode;
    using SearchCodeResult = Models.Octokit.SearchCodeResult;

    internal class GitHubSeeker : IGitHubSeeker
    {
        private const int FirstPage = 1;
        private const int RetryAttempts = 5;
        private const int RetrySeconds = 60;

        // See Git Hub documentation https://developer.github.com/v3/search/#search
        private const int MaxSearchResults = 1000;

        private readonly IGitHubClient _gitHubClient;

        public GitHubSeeker(string authToken)
        {
            _gitHubClient = new GitHubClient(new ProductHeaderValue(NonLocalizableStrings.ApplicationName))
            {
                Credentials = new Credentials(authToken)
            };
        }

        public async Task<IEnumerable<Code>> SearchCodeAsync(
            SearchConfiguration options,
            CancellationToken cancellationToken = default)
        {
            Language[] languages = options.Languages.Select(LanguageMapper.Map).ToArray();
            var code = new List<SearchCode>();

            foreach (string keyword in options.Keywords)
            {

                foreach (Language language in languages)
                {
                    Log.Information($"Searching for {keyword}. Targeted language: {language}");

                    SearchCodeResult searchCodeFirstPageResult = await RetrySendSearchCodeRequestAsync(
                        FirstPage,
                        keyword,
                        language,
                        options.CodeInQualifiers,
                        options.ItemsPerPage,
                        cancellationToken);

                    if (searchCodeFirstPageResult.TotalCount == 0)
                    {
                        continue;
                    }

                    code.AddRange(searchCodeFirstPageResult.Items);

                    int pagesCount = GetPageCount(searchCodeFirstPageResult.TotalCount, options.ItemsPerPage);

                    for (var page = 2; page <= pagesCount; page++)
                    {
                        SearchCodeResult searchCodePageResult = await RetrySendSearchCodeRequestAsync(
                            page,
                            keyword,
                            language,
                            options.CodeInQualifiers,
                            options.ItemsPerPage,
                            cancellationToken);
                        code.AddRange(searchCodePageResult.Items);
                    }
                }
            }

            return code.Select(
                c => new Code
                {
                    Repository = new Repository
                    {
                        Url = c.Repository.HtmlUrl
                    },
                    FileUrl = c.HtmlUrl,
                    TextMatches = c.TextMatches
                });
        }

        private async Task<SearchCodeResult> RetrySendSearchCodeRequestAsync(
            int page,
            string keyword,
            Language language,
            IEnumerable<CodeInQualifier> qualifiers,
            int itemsPerPage,
            CancellationToken cancellationToken)
        {
            var request = new SearchCodeRequest(keyword)
            {
                Page = page,
                PerPage = itemsPerPage,
                Language = language,
                In = qualifiers
            };

            SearchCodeResult searchCodeResult = null;
            for (var attempt = 1; attempt <= RetryAttempts; attempt++)
            {
                try
                {
                    searchCodeResult = (await _gitHubClient.Connection.Get<SearchCodeResult>(
                        ApiUrls.SearchCode(),
                        request.Parameters,
                        NonLocalizableStrings.SearchCodeAcceptHeaderValue,
                        cancellationToken)).Body;

                    break;
                }
                catch (AbuseException
                    ex) // See info about Abuse rate limits https://developer.github.com/v3/#abuse-rate-limits
                {
                    Log.Information(ex.Message);
                    Log.Information(
                        string.Format(
                            NonLocalizableStrings.RetryAfterSecondsFormat,
                            ex.RetryAfterSeconds ?? RetrySeconds));

                    await Task.Delay(TimeSpan.FromSeconds(ex.RetryAfterSeconds ?? RetrySeconds), cancellationToken);
                }
                catch (RateLimitExceededException ex)
                {
                    Log.Information(ex.Message);

                    TimeSpan timeSpan = await GetRateLimitResetTimeSpan();
                    Log.Information(
                        string.Format(
                            NonLocalizableStrings.RetryAfterSecondsFormat,
                            timeSpan.Seconds));

                    if (timeSpan.Seconds > 0)
                    {
                        await Task.Delay(timeSpan, cancellationToken);
                    }
                }
                //TODO! There may be need to refactor this exception.
                catch (ForbiddenException ex)
                {
                    Log.Information(ex.Message);

                    TimeSpan timeSpan = await GetRateLimitResetTimeSpan();
                    Log.Information(
                        string.Format(
                            NonLocalizableStrings.RetryAfterSecondsFormat,
                            timeSpan.Seconds));

                    if (timeSpan.Seconds > 0)
                    {
                        await Task.Delay(timeSpan, cancellationToken);
                    }
                }
                //TODO! There may be need to refactor this exception.
                catch (OverflowException ex)
                {
                    Log.Information(ex.Message);
                    Log.Information(
                        "This exception has been probably thrown because of Octokit.Internal.SearchResult.TotalCount type limitations - " +
                        "it has an Int type with the max value = 2,147,483,647, while the result number has a bigger value. " +
                        $"Probably there are too many coincidences with the search keyword = {keyword}.");

                    searchCodeResult = new SearchCodeResult();
                }
                //TODO! There may be need to refactor this exception.
                //catch (Exception ex)
                //{
                //    Log.Information(ex.Message);
                //    Log.Information("The general exception has been thrown. Be careful with it!");

                //    TimeSpan timeSpan = await GetRateLimitResetTimeSpan();
                //    Log.Information(
                //        string.Format(
                //            NonLocalizableStrings.RetryAfterSecondsFormat,
                //            timeSpan.Seconds));

                //    if (timeSpan.Seconds > 0)
                //    {
                //        await Task.Delay(timeSpan, cancellationToken);
                //    }
                //}
            }

            return searchCodeResult;
        }

        private static int GetPageCount(int totalCount, int itemsPerPage)
        {
            int itemsCount = totalCount > MaxSearchResults ? MaxSearchResults : totalCount;

            return (int) Math.Ceiling((double) itemsCount / itemsPerPage);
        }

        private async Task<TimeSpan> GetRateLimitResetTimeSpan()
        {
            RateLimit rateLimit = (await _gitHubClient.Miscellaneous.GetRateLimits()).Resources.Search;

            TimeSpan dateTimeDiff = rateLimit.Reset.UtcDateTime - DateTime.UtcNow;

            return new TimeSpan(dateTimeDiff.Ticks);
        }
    }
}