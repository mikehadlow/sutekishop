using System;
using System.ComponentModel.DataAnnotations;

namespace Suteki.Shop.Models.CustomDataAnnotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class StringPropertyAttributeBase : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value == null) return true;

            var stringValue = value as string;

            if (stringValue == null)
            {
                throw new ValidationException(string.Format("{0} can only be used on string properties", GetType().Name));
            }

            return IsValid(stringValue);
        }

        protected abstract bool IsValid(string value);
    }
}