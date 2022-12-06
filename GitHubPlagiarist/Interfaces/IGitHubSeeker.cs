using GitHubPlagiarist.Configuration;
using GitHubPlagiarist.Models;

namespace GitHubPlagiarist.Interfaces
{
    internal interface IGitHubSeeker
    {
        Task<IEnumerable<Code>> SearchCodeAsync(SearchConfiguration options, CancellationToken cancellationToken = default);
    }
}
