using OctokitSearchCode = Octokit.SearchCode;

namespace GitHubPlagiarist.Models.Octokit
{
    public class SearchCode : OctokitSearchCode
    {
        public IEnumerable<TextMatch> TextMatches { get; set; }
    }
}
