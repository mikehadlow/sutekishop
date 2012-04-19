using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Suteki.Common.Validation
{
    public class Validator : List<Action>
    {
        public void Validate(ModelStateDictionary modelState)
        {
            var errorNumber = 0;
            foreach (var validation in this)
            {
                try
                {
                    validation();
                }
                catch (ValidationException validationException)
                {
                    modelState.AddModelError(string.Format("validation_error_{0}", errorNumber), validationException.Message);
                    ErrorsOccured = true;
                }
                errorNumber++;
            }
        }

        public bool ErrorsOccured { get; private set; }

        public static void Validate(ModelStateDictionary modelState, Action actionToValidate)
        {
            ValidateFails(modelState, actionToValidate);
        }

        public static bool ValidateFails(ModelStateDictionary modelState, Action actionToValidate)
        {
            var validator = new Validator {actionToValidate};
            validator.Validate(modelState);
            return validator.ErrorsOccured;
        }
    }
}