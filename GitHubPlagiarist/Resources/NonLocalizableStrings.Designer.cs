﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GitHubPlagiarist.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class NonLocalizableStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal NonLocalizableStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GitHubPlagiarist.Resources.NonLocalizableStrings", typeof(NonLocalizableStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GitHubPlagiarist.
        /// </summary>
        internal static string ApplicationName {
            get {
                return ResourceManager.GetString("ApplicationName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ExceptionsPath.
        /// </summary>
        internal static string ExceptionsPathKey {
            get {
                return ResourceManager.GetString("ExceptionsPathKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The language you provided is not registered..
        /// </summary>
        internal static string LanguageIsNotRegistered {
            get {
                return ResourceManager.GetString("LanguageIsNotRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Specify at least one searching target..
        /// </summary>
        internal static string NoSearchingTargetsErrorMessage {
            get {
                return ResourceManager.GetString("NoSearchingTargetsErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} must be set.
        /// </summary>
        internal static string RequiresNonDefautAttributeErrorMessageFormat {
            get {
                return ResourceManager.GetString("RequiresNonDefautAttributeErrorMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Retry after {0} seconds..
        /// </summary>
        internal static string RetryAfterSecondsFormat {
            get {
                return ResourceManager.GetString("RetryAfterSecondsFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to application/vnd.github.v3.text-match+json.
        /// </summary>
        internal static string SearchCodeAcceptHeaderValue {
            get {
                return ResourceManager.GetString("SearchCodeAcceptHeaderValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SearchResults.
        /// </summary>
        internal static string SearchResultsFileName {
            get {
                return ResourceManager.GetString("SearchResultsFileName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are too many coincidences with the search keyword = {0}.
        /// </summary>
        internal static string TooManyCoincidences {
            get {
                return ResourceManager.GetString("TooManyCoincidences", resourceCulture);
            }
        }
    }
}
