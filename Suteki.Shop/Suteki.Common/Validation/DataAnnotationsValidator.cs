using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace Suteki.Common.Validation
{
    public class DataAnnotationsValidator
    {
        /// <summary>
        /// Thanks to Daniel Cazzulino
        /// http://www.clariusconsulting.net/blogs/kzu/archive/2010/04/15/234739.aspx
        /// </summary>
        public static IEnumerable<ValidationResult> Validate(object item)
        {
            return from descriptor in TypeDescriptor.GetProperties(item).Cast<PropertyDescriptor>()
                   from validation in descriptor.Attributes.OfType<System.ComponentModel.DataAnnotations.ValidationAttribute>()
                   where !validation.IsValid(descriptor.GetValue(item))
                   select new ValidationResult(
                       validation.ErrorMessage ?? string.Format("{0} validation failed.", validation.GetType().Name),
                       descriptor.Name);
        }
    }

    public static class DataAnnotationsValidatorExtensions
    {
        public static IEnumerable<ValidationResult> WithPropertyPrefix(
            this IEnumerable<ValidationResult> validationResults, 
            string prefix)
        {
            return validationResults.Select(r => new ValidationResult(r.ErrorMessage, prefix + r.PropertyName));
        }

        public static void AndUpdate(this IEnumerable<ValidationResult> results, ModelStateDictionary modelState)
        {
            foreach (var validationResult in results)
            {
                modelState.AddModelError(validationResult.PropertyName, validationResult.ErrorMessage);
            }
        }
    }

    public class ValidationResult
    {
        public string ErrorMessage { get; private set; }
        public string PropertyName { get; private set; }

        public ValidationResult(string errorMessage, string propertyName)
        {
            ErrorMessage = errorMessage;
            PropertyName = propertyName;
        }
    }
}