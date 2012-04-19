using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web.Mvc;
using NUnit.Framework;
using Suteki.Common.Models;
using Suteki.Common.Repositories;
using Suteki.Common.Services;
using Suteki.Common.TestHelpers;
using Suteki.Common.Validation;
using Suteki.Common.ViewData;
using Suteki.Shop.Controllers;
using System.Collections.Generic;
using Rhino.Mocks;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class PostageControllerTests
    {
        private PostageController postageController;

        private IRepository<Postage> postageRepository;
        private IOrderableService<Postage> orderableService;
        private IHttpContextService httpContextService;

        [SetUp]
        public void SetUp()
        {
            // you have to be an administrator to access the CMS controller
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin"), new[] { "Administrator" });

            postageRepository = MockRepository.GenerateStub<IRepository<Postage>>();
            orderableService = MockRepository.GenerateStub<IOrderableService<Postage>>();
            httpContextService = MockRepository.GenerateStub<IHttpContextService>();

            postageController = new PostageController
            {
                Repository = postageRepository,
                OrderableService = orderableService,
                HttpContextService = httpContextService
            };
        }

        [Test]
        public void Index_ShouldShowListOfPostages()
        {
            var postages = new List<Postage>();
            postageRepository.Expect(pr => pr.GetAll()).Return(postages.AsQueryable());

            postageController.Index(1)
                .ReturnsViewResult()
                .ForView("Index")
                .WithModel<ScaffoldViewData<Postage>>()
                .AssertNotNull(vd => vd.Items);
        }

        [Test]
        public void New_ShouldShowEditViewWithNewPostage()
        {
            postageController.New()
                .ReturnsViewResult()
                .ForView("Edit")
                .WithModel<ScaffoldViewData<Postage>>()
                .AssertNotNull(vd => vd.Item);
        }

        [Test]
        public void Edit_ShouldShowEditViewWithExistingPostage()
        {
            var postageId = 3;
            var postage = new Postage { Id = postageId };

            postageRepository.Expect(pr => pr.GetById(postageId)).Return(postage);

            postageController.Edit(postageId)
                .ReturnsViewResult()
                .ForView("Edit")
                .WithModel<ScaffoldViewData<Postage>>()
                .AssertAreSame(postage, vd => vd.Item);
        }

    	[Test]
    	public void NewWithPost_ShouldAddNewPostage()
    	{
    		var postage = new Postage() { MaxWeight = 250, Price = new Money(5.25M), Name = "foo"};
    		postageController.New(postage)
    			.ReturnsRedirectToRouteResult()
    			.ToAction("Index");

			postageRepository.AssertWasCalled(x=>x.SaveOrUpdate(postage));
    	}

    	[Test]
    	public void NewWithPost_ShouldRenderViewOnError()
    	{
    		postageController.ModelState.AddModelError("foo", "bar");
    		postageController.New(new Postage())
    			.ReturnsViewResult()
    			.ForView("Edit");

			postageRepository.AssertWasNotCalled(x=>x.SaveOrUpdate(Arg<Postage>.Is.Anything));
    	}


    	[Test]
    	public void EditWithPost_ShouldRedirectOnSuccessfulSave()
    	{
    		var postage = new Postage();
    		postageController.Edit(postage)
    			.ReturnsRedirectToRouteResult()
				.ToAction("Index");
    	}

    	[Test]
    	public void EditWithPost_ShouldRenderViewOnError()
    	{
    		postageController.ModelState.AddModelError("foo", "Bar");
    		var postage = new Postage();
    		postageController.Edit(postage)
    			.ReturnsViewResult()
    			.WithModel<ScaffoldViewData<Postage>>()
    			.AssertNull(x => x.Message)
    			.AssertAreSame(postage, x => x.Item);
    	}

        [Test]
        public void MoveUp_ShouldMoveItemUp()
        {
            const int position = 4;

            var orderResult = MockRepository.GenerateMock<IOrderServiceWithPosition<Postage>>();

            orderableService.Stub(os => os.MoveItemAtPosition(position)).Return(orderResult);

            var postages = new List<Postage>();
            postageRepository.Expect(pr => pr.GetAll()).Return(postages.AsQueryable());

            postageController.MoveUp(position, 1);

            orderResult.AssertWasCalled(or => or.UpOne());
        }
    }
}
