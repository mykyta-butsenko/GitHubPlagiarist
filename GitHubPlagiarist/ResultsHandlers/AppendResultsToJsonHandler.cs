using System.Text.Json;
using GitHubPlagiarist.Models;
using Serilog;

namespace GitHubPlagiarist.ResultsHandlers
{
    internal class AppendResultsToJsonHandler : CodeResultsHandler
    {
        private readonly string _resultsFileFullName;

        public AppendResultsToJsonHandler(string resultsFileFullName)
        {
            _resultsFileFullName = resultsFileFullName + ".json";
        }

        public override void Handle(Code result)
        {
#if DEBUG
            Log.Information(nameof(AppendResultsToJsonHandler));
#endif

            string json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            File.AppendAllText(_resultsFileFullName, json + ",");

            base.Handle(result);
        }
    }
}
