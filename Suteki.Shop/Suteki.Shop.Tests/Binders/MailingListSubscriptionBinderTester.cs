using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Shop.Binders;

namespace Suteki.Shop.Tests.Binders
{
	[TestFixture]
	public class MailingListSubscriptionBinderTester
	{
		MailingListSubscriptionBinder binder;
		ModelBindingContext context;
		FakeValueProvider valueProvider;
		ControllerContext controllerContext;
	    MailingListSubscription subscription;

		[SetUp]
		public void Setup()
		{

            subscription = new MailingListSubscription
            {
                Contact = new Contact()
            };

            var repository = new FakeRepository(id => subscription);
		    var repositoryResolver = MockRepository.GenerateStub<IRepositoryResolver>();
		    repositoryResolver.Stub(r => r.GetRepository(typeof (MailingListSubscription))).Return(repository);

			binder = new MailingListSubscriptionBinder(repositoryResolver);


			valueProvider = new FakeValueProvider();
			context = new ModelBindingContext()
			{
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(MailingListSubscription)),
				ModelState =  new ModelStateDictionary(),
				ValueProvider = valueProvider
			};

			controllerContext = new ControllerContext
			{
				HttpContext = MockRepository.GenerateStub<HttpContextBase>() 				
			};
			controllerContext.HttpContext.Expect(x => x.Request).Return(MockRepository.GenerateStub<HttpRequestBase>());
			controllerContext.HttpContext.Request.Expect(x => x.Form).Return(new NameValueCollection());
        }

		[Test]
		public void AddsErrorToModelState_WhenEmailsDoNotMatch()
		{
            binder.Accept(new BindMailingListAttribute { ValidateConfirmEmail = true });
            
            valueProvider.AddValue("subscription.Email", "foo", "foo");
            valueProvider.AddValue("emailconfirm", "bar", "bar");
			//controllerContext.HttpContext.Request.Form.Add("emailconfirm", "bar");

			binder.BindModel(controllerContext, context);
			context.ModelState["emailconfirm"].Errors.Single().ErrorMessage.ShouldEqual("Email and Confirm Email do not match");
		}
	}
}