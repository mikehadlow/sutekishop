using System.Web.Mvc;
using NUnit.Framework;
using Suteki.Common.Validation;

namespace Suteki.Common.Tests.Validation
{
	[TestFixture]
	public class ValidationExtensionsTests
	{
		public void CollectingExceptionsSpike()
		{
			var validator = new Validator
			                {
			                	() => { throw new ValidationException("the first one"); },
			                	() => { throw new ValidationException("the second one"); },
			                	() => { throw new ValidationException("the third one"); },
			                };

		    var modelState = new ModelStateDictionary();
			validator.Validate(modelState);

            modelState["validation_error_0"].Errors[0].ErrorMessage.ShouldEqual("the first one");
            modelState["validation_error_1"].Errors[0].ErrorMessage.ShouldEqual("the second one");
            modelState["validation_error_2"].Errors[0].ErrorMessage.ShouldEqual("the third one");
		}
	}
}