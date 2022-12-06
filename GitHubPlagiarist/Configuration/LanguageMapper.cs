using GitHubPlagiarist.Exceptions;
using GitHubSearch.Resources;
using Octokit;

namespace GitHubPlagiarist.Configuration
{
    public static class LanguageMapper
    {
        private static readonly Dictionary<string, Language> Languages = new Dictionary<string, Language>
        {
            ["csharp"] = Language.CSharp,
            ["javascript"] = Language.JavaScript,
            ["cplusplus"] = Language.CPlusPlus,
            ["visualbasic"] = Language.VisualBasic,
            ["java"] = Language.Java,

            ["c#"] = Language.CSharp,
            ["js"] = Language.JavaScript,
            ["vb"] = Language.VisualBasic,
            ["cpp"] = Language.CPlusPlus
        };

        public static Language Map(string language)
        {
            try
            {
                return Languages[language.ToLower()];
            }
            catch (Exception)
            {
                throw new LanguageNotRegisteredException(NonLocalizableStrings.LanguageIsNotRegistered);
            }
        }
    }
}
