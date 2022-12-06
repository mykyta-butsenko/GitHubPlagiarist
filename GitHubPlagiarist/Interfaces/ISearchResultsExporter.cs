namespace GitHubPlagiarist.Interfaces
{
    internal interface ISearchResultsExporter
    {
        Task ExportAsync<TSource>(
            string fileName,
            IEnumerable<TSource> source,
            CancellationToken cancellationToken = default);
    }
}
