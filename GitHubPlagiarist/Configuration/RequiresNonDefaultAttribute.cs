using System.ComponentModel.DataAnnotations;
using System.Reflection;
using GitHubPlagiarist.Resources;

namespace GitHubPlagiarist.Configuration
{
    /// <summary>
    /// Specifies that the data field requires non default value of the other data field 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiresNonDefaultAttribute : ValidationAttribute
    {
        private readonly string _propertyName;

        public RequiresNonDefaultAttribute(string propertyName)
        {
            _propertyName = propertyName;
            ErrorMessage = string.Format(
                NonLocalizableStrings.RequiresNonDefautAttributeErrorMessageFormat,
                propertyName);
        }

        public bool AllowEmptyString { get; set; }

        /// <inheritdoc />
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Skip validation when decorated property has default value
            if (value == null || value.Equals(GetDefaultValue(value.GetType())))
            {
                return ValidationResult.Success;
            }

            PropertyInfo propertyInfo = validationContext.ObjectType.GetProperty(_propertyName);
            if (propertyInfo == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            object propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance);

            // ReSharper disable once PossibleNullReferenceException
            if (propertyValue == null ||
                propertyValue.Equals(GetDefaultValue(propertyInfo.PropertyType)) ||
                IsEmptyString(propertyValue))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        private static object GetDefaultValue(Type type)
        {
            // Create instance with default value for ValueType
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        private bool IsEmptyString(object @object)
        {
            return !AllowEmptyString && @object is string stringValue && stringValue.Trim().Length == 0;
        }
    }
}