// ReSharper disable InconsistentNaming
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Services;
using Suteki.Shop.Services;
using Suteki.Shop.Services.ProductBuilderContributors;

namespace Suteki.Shop.Tests.Services.ProductBuilderContributors
{
    [TestFixture]
    public class ImagesTests : ProductBuilderContributorTestBase
    {
        IHttpFileService httpFileService;
        IOrderableService<ProductImage> productOrderableService;

        protected override IProductBuilderContributor InitContributor()
        {
            httpFileService = MockRepository.GenerateStub<IHttpFileService>();
            productOrderableService = MockRepository.GenerateStub<IOrderableService<ProductImage>>();

            return new Images(httpFileService, productOrderableService);
        }

        [Test]
        public void Should_add_uploaded_images_to_product()
        {
            var images = new[]
            {
                new Image(),
                new Image()
            };

            httpFileService.Stub(h => h.GetUploadedImages(
                context.HttpRequestBase, 
                ImageDefinition.ProductImage,
                ImageDefinition.ProductThumbnail)).Return(images.AsEnumerable());

            productOrderableService.Stub(p => p.NextPosition).Return(11);

            var product = new Product();
            context.SetProduct(product);

            contributor.ContributeTo(context);

            product.ProductImages.Count.ShouldEqual(2);
            product.ProductImages[0].Image.ShouldBeTheSameAs(images[0]);
            product.ProductImages[1].Image.ShouldBeTheSameAs(images[1]);
        }
    }
}
// ReSharper restore InconsistentNaming