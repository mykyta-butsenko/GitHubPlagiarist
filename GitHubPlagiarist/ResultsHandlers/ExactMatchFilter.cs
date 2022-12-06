using System.Text.RegularExpressions;
using GitHubPlagiarist.Models;
using GitHubPlagiarist.Models.Octokit;
using Serilog;

namespace GitHubPlagiarist.ResultsHandlers
{
    /// <summary>
    /// <remarks>
    /// It is necessary to filter results for exact matches due to specifics of the GitHub search engine.
    /// See https://github.community/t/github-enterprise-how-to-find-files-containing-only-an-exact-term/1171
    /// </remarks>
    /// </summary>
    internal class ExactMatchFilter : CodeResultsHandler
    {
        private readonly IEnumerable<string> _keywords;

        public ExactMatchFilter(IEnumerable<string> keywords)
        {
            _keywords = keywords;
        }

        public override void Handle(Code result)
        {
#if DEBUG
            Log.Information(nameof(ExactMatchFilter));
#endif

            var matchingFragments = new List<TextMatch>();
            foreach (string keyword in _keywords)
            {
                matchingFragments.AddRange(
                    result.TextMatches
                        .Where(m => Regex.IsMatch(m.Fragment, $"[^\\w, \\d]{keyword}[^\\w, \\d]"))
                        .ToList());
            }

            if (matchingFragments.Count == 0)
            {
                return;
            }

            result.TextMatches = matchingFragments;
            base.Handle(result);
        }
    }
}
