using System.ComponentModel.DataAnnotations;
using Octokit;

namespace GitHubPlagiarist.Configuration
{
    public class SearchConfiguration
    {
        public int ItemsPerPage => 100;

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [MinLength(1), Required(ErrorMessage = "Specify at least one keyword to search for.")]
        public string[] Keywords { get; set; }

        [MinLength(1), Required(ErrorMessage = "Specify at least one language to search in.")]
        public string[] Languages { get; set; }

        [Required]
        public string Reviewer { get; set; }

        public string ExceptionsListFileName { get; set; }

        [RequiresNonDefault(nameof(ExceptionsListFileName), AllowEmptyString = false)]
        public bool AppendResultsToExceptionsFile { get; set; }

        public IEnumerable<CodeInQualifier> CodeInQualifiers { get; set; } = new[]
        {
            CodeInQualifier.File
        };
    }
}
