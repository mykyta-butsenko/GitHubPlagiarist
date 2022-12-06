using GitHubPlagiarist.Interfaces;
using GitHubPlagiarist.Models;

namespace GitHubPlagiarist.ResultsHandlers
{
    internal class CodeResultsHandler : IResultsHandler<Code>
    {
        private IResultsHandler<Code> _nextHandler;

        public IResultsHandler<Code> SetNext(IResultsHandler<Code> resultsHandler)
        {
            _nextHandler = resultsHandler;

            return _nextHandler;
        }

        public virtual void Handle(Code result)
        {
            _nextHandler?.Handle(result);
        }
    }
}
