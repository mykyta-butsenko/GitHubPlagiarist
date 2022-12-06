using GitHubPlagiarist.Models;
using Serilog;

namespace GitHubPlagiarist.ResultsHandlers
{
    internal class AppendExceptionHandler : CodeResultsHandler
    {
        private readonly string _exceptionsListFileFullName;

        public AppendExceptionHandler(string exceptionsListFileFullName)
        {
            _exceptionsListFileFullName = exceptionsListFileFullName;
        }

        public override void Handle(Code result)
        {
#if DEBUG
            Log.Information(nameof(AppendExceptionHandler));
#endif

            var fileStream = new FileStream(_exceptionsListFileFullName, FileMode.Append);
            using var streamWriter = new StreamWriter(fileStream);
            streamWriter.WriteLine(result.Repository.Url);

            base.Handle(result);
        }
    }
}
