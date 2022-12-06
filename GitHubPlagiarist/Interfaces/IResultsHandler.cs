namespace GitHubPlagiarist.Interfaces
{
    internal interface IResultsHandler<TResult>
    {
        IResultsHandler<TResult> SetNext(IResultsHandler<TResult> resultsHandler);

        void Handle(TResult result);
    }
}
