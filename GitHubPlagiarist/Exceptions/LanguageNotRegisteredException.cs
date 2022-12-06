namespace GitHubPlagiarist.Exceptions
{
    public class LanguageNotRegisteredException : Exception
    {
        public LanguageNotRegisteredException(string message)
            : base(message)
        {
        }
    }
}
