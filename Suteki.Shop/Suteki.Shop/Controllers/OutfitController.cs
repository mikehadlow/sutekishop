using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcContrib;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Shop.Filters;
using Suteki.Shop.Services;

namespace Suteki.Shop.Controllers
{
    public class OutfitController : ControllerBase
    {
        private readonly IRepository<Outfit> outfitRepository;
        private readonly IRepository<Product> productRepository;
		private readonly IUnitOfWorkManager uow;
        private readonly IHttpFileService httpFileService;

        public OutfitController(IRepository<Outfit> outfitRepository, IUnitOfWorkManager uow, IRepository<Product> productRepository, IHttpFileService httpFileService)
        {
            this.outfitRepository = outfitRepository;
            this.uow = uow;
            this.productRepository = productRepository;
            this.httpFileService = httpFileService;
        }

        [HttpGet]
        [UnitOfWork]
        public ActionResult Index()
        {
            var outfits = new List<Outfit>();
            return View("Index", outfits);
        }

        [HttpGet]
        [UnitOfWork]
        public ActionResult Item(int id)
        {
            var outfit = outfitRepository.GetById(id);
            return View("Item", outfit);
        }

        [HttpGet]
        [UnitOfWork]
        [AdministratorsOnly]
        public ActionResult New()
        {
            var outfit = new Outfit();
            return View("Edit", outfit);
        }

        [HttpPost]
        [UnitOfWork]
        [AdministratorsOnly]
        [ValidateInput(false)]
        public ActionResult New(Outfit outfit)
        {
            if (ModelState.IsValid)
            {
                outfitRepository.SaveOrUpdate(outfit);
                uow.Commit();
                return this.RedirectToAction(x => x.Item(outfit.Id));
            }
            return View("Edit", outfit);
        }

        [HttpGet]
        [UnitOfWork]
        [AdministratorsOnly]
        public ActionResult Edit(int id)
        {
            var outfit = outfitRepository.GetById(id);
            SetProductIds(outfit);
            return View("Edit", outfit);
        }

        private void SetProductIds(Outfit outfit)
        {
            foreach (var outfitProduct in outfit.OutfitProducts)
            {
                outfit.ProductIds.Add(outfitProduct.Product.Id);
            }
        }

        [HttpPost]
        [UnitOfWork]
        [AdministratorsOnly]
        [ValidateInput(false)]
        public ActionResult Edit(Outfit outfit)
        {
            if (ModelState.IsValid)
            {
                UpdateProducts(outfit);
                UpdateImages(outfit);

                return this.RedirectToAction(x => x.Item(outfit.Id));
            }
            return View("Edit", outfit);
        }

        private void UpdateProducts(Outfit outfit)
        {
            // remove removed products:
            var outfitProductsToRemove = outfit.OutfitProducts
                .Where(outfitProduct => outfit.ProductIds.All(x => x != outfitProduct.Product.Id))
                .ToList();

            foreach (OutfitProduct outfitProduct in outfitProductsToRemove)
            {
                outfit.OutfitProducts.Remove(outfitProduct);
            }


            // Add new products:
            foreach (var productId in outfit.ProductIds)
            {
                if (outfit.OutfitProducts.All(x => x.Product.Id != productId))
                {
                    var product = productRepository.GetById(productId);
                    outfit.OutfitProducts.Add(new OutfitProduct
                    {
                        Position = 0,
                        Product = product,
                        Outfit = outfit
                    });
                }
            }
        }

        private void UpdateImages(Outfit outfit)
        {
            var images = httpFileService.GetUploadedImages(
                Request, 
                ImageDefinition.ProductImage,
                ImageDefinition.ProductThumbnail);

            foreach (var image in images)
            {
                outfit.OutfitImages.Add(new OutfitImage
                {
                    Image = image,
                    Outfit = outfit,
                    Position = 0
                });
            }
        }

        public ActionResult MoveImageUp(int id, int position)
        {
            throw new NotImplementedException();
        }

        public ActionResult MoveImageDown(int id, int position)
        {
            throw new NotImplementedException();
        }

        public void DeleteImage(int id, int outfitImageId)
        {
            throw new NotImplementedException();
        }
    }
}