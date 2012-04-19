// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;
using Suteki.Shop.Services;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class HeaderControllerTests
    {
        private HeaderController headerController;
        private MockBaseControllerService baseControllerService;

        [SetUp]
        public void SetUp()
        {
            baseControllerService = new MockBaseControllerService();
            headerController = new HeaderController(baseControllerService);
            SetupBaseControllerService();
        }

        [Test]
        public void Head_should_return_view_model()
        {
            var headerViewData = headerController.Head()
                .ReturnsViewResult()
                .ForView("Head")
                .WithModel<HeaderViewData>();

            AssertHeaderViewData(headerViewData);
        }

        [Test]
        public void Header_should_return_view_model()
        {
            var headerViewData = headerController.Header()
                .ReturnsViewResult()
                .ForView("Header")
                .WithModel<HeaderViewData>();

            AssertHeaderViewData(headerViewData);
        }

        [Test]
        public void Footer_should_return_view_model()
        {
            var headerViewData = headerController.Footer()
                .ReturnsViewResult()
                .ForView("Footer")
                .WithModel<HeaderViewData>();

            AssertHeaderViewData(headerViewData);
        }

        [Test]
        public void PageEnd_should_return_view_model()
        {
            var headerViewData = headerController.PageEnd()
                .ReturnsViewResult()
                .ForView("PageEnd")
                .WithModel<HeaderViewData>();

            AssertHeaderViewData(headerViewData);
        }

        private void AssertHeaderViewData(HeaderViewData headerViewData)
        {
            headerViewData.SiteUrl.ShouldEqual(baseControllerService.SiteUrl);
            headerViewData.Title.ShouldEqual(baseControllerService.ShopName);
            headerViewData.Email.ShouldEqual(baseControllerService.EmailAddress);
            headerViewData.Copyright.ShouldEqual(baseControllerService.Copyright);
            headerViewData.PhoneNumber.ShouldEqual(baseControllerService.PhoneNumber);
            headerViewData.GoogleTrackingCode.ShouldEqual("\"ABC123\"");
            headerViewData.SiteCss.ShouldEqual("gadgetsareus.css");
        }

        private void SetupBaseControllerService()
        {
            baseControllerService.GoogleTrackingCode = "ABC123";
            baseControllerService.ShopName = "Widgets Are Us";
            baseControllerService.EmailAddress = "info@widgetsareus.com";
            baseControllerService.SiteUrl = "http://www.widgetsareus.com/";
            baseControllerService.MetaDescription = "Awesome range of pure goodness";
            baseControllerService.Copyright = "Copyright C Gadgets are us 2010";
            baseControllerService.PhoneNumber = "1234567890";
            baseControllerService.SiteCss = "gadgetsareus.css";
            baseControllerService.FacebookUserId = "gadgetsareus";
        }
    }

    public class MockBaseControllerService : IBaseControllerService
    {
        public IRepository<Category> CategoryRepository { get; set; }
        public string GoogleTrackingCode { get; set; }
        public string ShopName { get; set; }
        public string EmailAddress { get; set; }
        public string SiteUrl { get; set; }
        public string MetaDescription { get; set; }
        public string Copyright { get; set; }
        public string PhoneNumber { get; set; }
        public string SiteCss { get; set; }
        public string FacebookUserId { get; set; }
    }
}

// ReSharper restore InconsistentNaming
