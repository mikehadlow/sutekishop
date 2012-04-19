using System;
using System.Collections.Generic;
using Suteki.Common.Services;
using Suteki.Common.Validation;

namespace Suteki.Shop.Services.ProductBuilderContributors
{
    public class Images : IProductBuilderContributor
    {
        readonly IHttpFileService httpFileService;
        readonly IOrderableService<ProductImage> productOrderableService;

        public Images(IHttpFileService httpFileService, IOrderableService<ProductImage> productOrderableService)
        {
            this.httpFileService = httpFileService;
            this.productOrderableService = productOrderableService;
        }

        public void ContributeTo(ProductBuildingContext context)
        {
            IEnumerable<Image> images = null;
            if (Validator.ValidateFails(context.ModelStateDictionary, () =>
                images = httpFileService.GetUploadedImages(context.HttpRequestBase, ImageDefinition.ProductImage, ImageDefinition.ProductThumbnail)
            )) return;

            var position = productOrderableService.NextPosition;
            foreach (var image in images)
            {
                context.Product.AddProductImage(image, position);
                position++;
            }            
        }

        public int Order
        {
            get { return 4; }
        }
    }
}