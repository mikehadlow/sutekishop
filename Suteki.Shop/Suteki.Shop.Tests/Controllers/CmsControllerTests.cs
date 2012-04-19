using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Suteki.Common.Repositories;
using Suteki.Common.Services;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;
using Suteki.Shop.ViewData;
using System.Threading;
using System.Security.Principal;
using System.Web.Mvc;
using Rhino.Mocks;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class CmsControllerTests
    {
        private CmsController cmsController;

        private IRepository<Content> contentRepository;
        private IRepository<Menu> menuRepository;
        private IOrderableService<Content> contentOrderableService;

        [SetUp]
        public void SetUp()
        {
            // you have to be an administrator to access the CMS controller
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin"), new[] { "Administrator" });

            contentRepository = MockRepository.GenerateStub<IRepository<Content>>();
            menuRepository = MockRepository.GenerateStub<IRepository<Menu>>();
            contentOrderableService = MockRepository.GenerateStub<IOrderableService<Content>>();

            cmsController = new CmsController(
                contentRepository, 
                menuRepository,
                contentOrderableService);
        }

        [Test]
        public void Index_ShouldRenderIndexViewWithContent()
        {
            const string urlName = "home";

            var contents = new List<Content>
            {
                new TextContent { UrlName = "home" },
                new ActionContent { Name = "Help Pages" }
            }.AsQueryable();

            contentRepository.Expect(cr => cr.GetAll()).Return(contents);

            cmsController.Index(urlName)
                .ReturnsViewResult()
                .ForView("SubPage")
                .WithModel<CmsViewData>()
                .AssertAreSame(
                    contents.OfType<ITextContent>().First(), 
                    vd => vd.TextContent);
        }

        [Test]
        public void Index_ShouldRenderTopContentWithTopPageView()
        {
            const string urlName = "home_page";

            var contents = new List<Content>
            {
                new TopContent { UrlName = "home_page" }
            }.AsQueryable();

            contentRepository.Expect(cr => cr.GetAll()).Return(contents);

            cmsController.Index(urlName)
                .ReturnsViewResult()
                .ForView("TopPage")
                .WithModel<CmsViewData>()
                .AssertAreSame(
                    contents.OfType<ITextContent>().First(), vd => vd.TextContent);

        }

        [Test]
        public void Add_ShouldShowContentEditViewWithDefaultContent()
        {
            const int menuId = 1;

            var menu = new Menu {Id = menuId};
            menuRepository.Expect(cr => cr.GetById(menuId)).Return(menu);

            var menus = new List<Content>().AsQueryable();
            contentRepository.Expect(cr => cr.GetAll()).Return(menus);

            cmsController.Add(menuId)
                .ReturnsViewResult()
                .ForView("Edit")
                .WithModel<CmsViewData>()
                .AssertNotNull(vd => vd.TextContent)
                .AssertAreEqual(menuId, vd => vd.Content.ParentContent.Id);
        }

		[Test]
		public void AddWithPost_ShouldAddNewContent()
		{
            contentRepository.Stub(cr => cr.GetAll()).Return(new List<Content>().AsQueryable());
			var content = new TextContent
			{
                ParentContent = new Menu { Id = 4 }
			};

			cmsController.Add(content)
				.ReturnsRedirectToRouteResult()
				.ToController("Menu")
				.ToAction("List")
				.WithRouteValue("id", "4");

			contentRepository.AssertWasCalled(x => x.SaveOrUpdate(content));
		}

        [Test]
        public void AddWithPost_should_RejectContentWithDuplicateName()
        {
            var existingContent = new List<Content>
            {
                new Menu { Id = 3, Name = "Gadget"},
                new Menu { Id = 10, Name = "Widget"},
            };

            contentRepository.Stub(cr => cr.GetAll()).Return(existingContent.AsQueryable());

            var content = new TextContent
            {
                ParentContent = new Menu { Id = 4 },
                Name = "Widget"
            };

            cmsController.Add(content)
                .ReturnsViewResult()
                .ForView("Edit")
                .WithModel<CmsViewData>()
                .AssertAreSame(content, x => x.Content);

            contentRepository.AssertWasNotCalled(x => x.SaveOrUpdate(content));
        }

    	[Test]
    	public void AddWithPost_ShouldRenderViewWithError()
    	{
			contentRepository.Stub(cr => cr.GetAll()).Return(new List<Content>().AsQueryable());
    		cmsController.ModelState.AddModelError("foo", "bar");

    		var content = new TextContent();
			cmsController.Add(content)
				.ReturnsViewResult()
				.ForView("Edit")
				.WithModel<CmsViewData>()
				.AssertAreSame(content, x => x.Content);
    	}

    	[Test]
        public void Edit_ShouldDisplayEditViewWithExistingContent()
        {
            const int contentId = 22;

            var content = new TextContent { Id = contentId };
            contentRepository.Stub(cr => cr.GetById(contentId)).Return(content);

    	    cmsController.EditText(contentId)
    	        .ReturnsViewResult()
    	        .ForView("Edit")
    	        .WithModel<CmsViewData>()
    	        .AssertAreEqual(contentId, vd => vd.Content.Id);
        }

    	[Test]
    	public void EditWithPost_ShouldRenderViewWithError()
    	{
			contentRepository.Stub(cr => cr.GetAll()).Return(new List<Content>().AsQueryable());
			cmsController.ModelState.AddModelError("foo", "bar");

			var content = new TextContent();
			cmsController.EditText(content)
				.ReturnsViewResult()
				.WithModel<CmsViewData>()
				.AssertAreSame(content, x => x.Content);
    	}

    	[Test]
    	public void EditWithPost_ShouldRedirectOnSuccessfulBinding()
    	{
            contentRepository.Stub(cr => cr.GetAll()).Return(new List<Content>().AsQueryable());
    		var content = new TextContent
    		{
                ParentContent = new Menu { Id = 4 }
    		};

			cmsController.Add(content)
				.ReturnsRedirectToRouteResult()
				.ToController("Menu")
				.ToAction("List")
				.WithRouteValue("id", "4");
    	}

    	[Test]
    	public void EditWithPost_should_work_for_TopContent()
    	{
            contentRepository.Stub(cr => cr.GetAll()).Return(new List<Content>().AsQueryable());
			cmsController.EditTop(new TopContent
			{
                ParentContent = new Menu { Id = 4 }
			});
    	}

        [Test]
        public void EditWithPost_should_Fail_on_duplicate_name()
        {
            var existingContent = new List<Content>
            {
                new Menu { Id = 3, Name = "Gadget"},
                new Menu { Id = 7, Name = "OldWidget"},
                new Menu { Id = 10, Name = "Widget"},
            };

            contentRepository.Stub(cr => cr.GetAll()).Return(existingContent.AsQueryable());

            var content = new TextContent
            {
                ParentContent = new Menu { Id = 4 },
                Id = 7,
                Name = "Widget"
            };

            cmsController.EditText(content)
                .ReturnsViewResult()
                .WithModel<CmsViewData>()
                .AssertAreSame(content, x => x.Content);
        }

        [Test]
        public void Should_show_NotFound_view_when_content_name_is_not_found()
        {
            contentRepository.Stub(cr => cr.GetAll()).Return(new List<Content>().AsQueryable());

            cmsController.Index("xxx")
                .ReturnsViewResult()
                .ForView("NotFound");
        }
    }

    public static class CreateFormExtensions
    {
        public static FormCollection ForTextContent(this FormCollection form)
        {
            form.Add("text", "some content text");
            return form;
        }

        public static FormCollection ForMenuContent(this FormCollection form)
        {
            return form;
        }
    }
}
