using GitHubPlagiarist.Models;
using Serilog;

namespace GitHubPlagiarist.ResultsHandlers
{
    internal class ExceptionsFilter : CodeResultsHandler
    {
        private readonly HashSet<string> _exceptions;

        public ExceptionsFilter(string exceptionsFileFullName)
        {
            if (string.IsNullOrEmpty(exceptionsFileFullName))
            {
                throw new ArgumentNullException(nameof(exceptionsFileFullName));
            }

            _exceptions = File.Exists(exceptionsFileFullName)
                ? File.ReadAllLines(exceptionsFileFullName).Distinct().ToHashSet()
                : new HashSet<string>();
        }

        public override void Handle(Code result)
        {
#if DEBUG
            Log.Information(nameof(ExceptionsFilter));
#endif

            if (_exceptions.Contains(result.FileUrl) || _exceptions.Contains(result.Repository.Url))
            {
                return;
            }

            base.Handle(result);
        }
    }
}