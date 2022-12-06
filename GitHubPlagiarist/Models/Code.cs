using GitHubPlagiarist.Models.Octokit;

namespace GitHubPlagiarist.Models
{
    public class Code
    {
        public string FileUrl { get; set; }

        public IEnumerable<TextMatch> TextMatches { get; set; }

        public Repository Repository { get; set; }
    }
}