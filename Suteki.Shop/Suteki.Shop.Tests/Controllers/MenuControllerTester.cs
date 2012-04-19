using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Common.Services;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Tests.Controllers
{
	[TestFixture]
	public class MenuControllerTester
	{
		private MenuController controller;
		private IRepository<Menu> menuRepository;
	    private IRepository<Content> contentRepository; 
		private IOrderableService<Content> orderableService;

		[SetUp]
		public void Setup()
		{
			menuRepository = MockRepository.GenerateStub<IRepository<Menu>>();
			orderableService = MockRepository.GenerateStub<IOrderableService<Content>>();
		    contentRepository = MockRepository.GenerateStub<IRepository<Content>>();
			controller = new MenuController(menuRepository, orderableService, contentRepository);
		}

		[Test]
		public void Edit_should_render_view()
		{
			var menu = new Menu();
			menuRepository.Expect(x => x.GetAll()).Return(new List<Menu>().AsQueryable());
			menuRepository.Expect(x => x.GetById(3)).Return(menu);
			controller.Edit(3)
				.WithModel<CmsViewData>()
				.AssertAreSame(menu, x => x.Content);
				
		}

		[Test]
		public void New_should_render_view()
		{
			const int parentContentId = 1;

			var mainMenu = new Menu { Id = parentContentId };
			menuRepository.Expect(mr => mr.GetById(parentContentId)).Return(mainMenu);

			controller.New(parentContentId)
				.ForView("Edit")
				.WithModel<CmsViewData>()
				.AssertNotNull<CmsViewData, Content>(vd => vd.Menu)
				.AssertAreEqual(parentContentId, vd => vd.Menu.ParentContent.Id);

		}

		[Test]
		public void NewWithPost_should_save()
		{
            contentRepository.Stub(x => x.GetAll()).Return(new List<Content>().AsQueryable());

		    var menu = new Menu {ParentContent = new Menu {Id = 5}};

			controller.New(menu)
				.ReturnsRedirectToRouteResult()
				.ToController("Menu")
				.ToAction("List").WithRouteValue("id", menu.ParentContent.Id.ToString());


			menuRepository.AssertWasCalled(x => x.SaveOrUpdate(menu));

		}

		[Test]
		public void NewWithPost_should_render_edit_view_on_error()
		{
            contentRepository.Stub(x => x.GetAll()).Return(new List<Content>().AsQueryable());

			controller.ModelState.AddModelError("foo", "bar");
            var menu = new Menu { ParentContent = new Menu { Id = 5 } };
			controller.New(menu)
				.ReturnsViewResult()
				.ForView("Edit")
				.WithModel<CmsViewData>()
				.AssertAreSame(menu, x => x.Content);
		}

	    [Test]
	    public void NewWithPost_should_not_allow_duplicate_names()
	    {
	        var contents = new List<Content>
	        {
	            new TextContent { Id = 3, Name = "Widget" },
                new TopContent { Id = 7, Name = "Gadget" }
	        };

	        contentRepository.Stub(x => x.GetAll()).Return(contents.AsQueryable());

            var menu = new Menu { ParentContent = new Menu { Id = 5 }, Name = "Gadget" };

            controller.New(menu)
                .ReturnsViewResult()
                .ForView("Edit")
                .WithModel<CmsViewData>()
                .AssertAreSame(menu, x => x.Content);
        }

		[Test]
		public void List_ShouldShowListOfExistingContent() 
		{
			var mainMenu = new Menu();
			menuRepository.Expect(mr => mr.GetById(1)).Return(mainMenu);

			controller.List(1)
				.ReturnsViewResult()
				.WithModel<CmsViewData>()
				.AssertAreSame(mainMenu, vd => vd.Menu);
		}
	}
}