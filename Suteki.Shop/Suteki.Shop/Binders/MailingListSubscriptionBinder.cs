using System;
using System.Globalization;
using System.Web.Mvc;
using Suteki.Common.Binders;
using Suteki.Common.Repositories;

namespace Suteki.Shop.Binders
{
	public class BindMailingListAttribute : EntityBindAttribute
	{
		public BindMailingListAttribute() : base(typeof(MailingListSubscriptionBinder))
		{
			Fetch = false;
			ValidateConfirmEmail = true;
		}

		public bool ValidateConfirmEmail { get; set; }
	}

    public class MailingListSubscriptionBinder : EntityModelBinder
	{
		private BindMailingListAttribute declaringAttribute;

        public MailingListSubscriptionBinder(IRepositoryResolver resolver)
            : base(resolver)
		{
		}

		protected override void ValidateEntity(ModelBindingContext bindingContext, ControllerContext controllerContext, object entity) 
        {
            if(declaringAttribute == null || !declaringAttribute.ValidateConfirmEmail) return;

		    var confirmEmailResult = bindingContext.ValueProvider.GetValue("emailconfirm");
		    if (confirmEmailResult == null)
		    {
		        throw new ApplicationException("Expected 'emailconfirm' form or querystring value not found.");
		    }

            var confirmEmail = confirmEmailResult.AttemptedValue;

            var subscription = (MailingListSubscription)entity;
			if(subscription.Email != confirmEmail)
			{
				bindingContext.ModelState.AddModelError("emailconfirm", "Email and Confirm Email do not match");
				bindingContext.ModelState.SetModelValue("emailconfirm", new ValueProviderResult(confirmEmail??"", confirmEmail??"", CultureInfo.CurrentCulture));
			}
		}

		public override void Accept(Attribute attribute)
		{
			this.declaringAttribute = (BindMailingListAttribute)attribute;
			base.Accept(attribute);
		}
	}
}