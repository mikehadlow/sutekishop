using System.Linq;
using System.Web.Mvc;
using MvcContrib;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Common.Services;
using Suteki.Shop.Filters;
using Suteki.Shop.Repositories;
using Suteki.Shop.Services;

namespace Suteki.Shop.Controllers
{
    public class OutfitController : ControllerBase
    {
        private readonly IRepository<Outfit> outfitRepository;
        private readonly IRepository<Product> productRepository;
		private readonly IUnitOfWorkManager uow;
        private readonly IHttpFileService httpFileService;
        private readonly IOrderableService<OutfitImage> imageOrderableService; 

        public OutfitController(IRepository<Outfit> outfitRepository, IUnitOfWorkManager uow, IRepository<Product> productRepository, IHttpFileService httpFileService, IOrderableService<OutfitImage> imageOrderableService)
        {
            this.outfitRepository = outfitRepository;
            this.uow = uow;
            this.productRepository = productRepository;
            this.httpFileService = httpFileService;
            this.imageOrderableService = imageOrderableService;
        }

        [HttpGet]
        [UnitOfWork]
        public ActionResult Index()
        {
            var outfits = outfitRepository.GetAll().Active();
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
                UpdateImages(outfit);
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
                UpdateImages(outfit);

                return this.RedirectToAction(x => x.Item(outfit.Id));
            }
            return View("Edit", outfit);
        }

        private void UpdateImages(Outfit outfit)
        {
            var images = httpFileService.GetUploadedImages(
                Request, 
                ImageDefinition.ProductImage,
                ImageDefinition.ProductThumbnail);

            var position = imageOrderableService.NextPosition;
            foreach (var image in images)
            {
                outfit.OutfitImages.Add(new OutfitImage
                {
                    Image = image,
                    Outfit = outfit,
                    Position = position
                });
                position++;
            }
        }

		[AdministratorsOnly, UnitOfWork]
        public ActionResult MoveImageUp(int id, int position)
        {
            imageOrderableService
                .MoveItemAtPosition(position)
                .ConstrainedBy(x => x.Outfit.Id == id)
                .UpOne();
            return this.RedirectToAction(x => x.Edit(id));
        }

		[AdministratorsOnly, UnitOfWork]
        public ActionResult MoveImageDown(int id, int position)
        {
            imageOrderableService
                .MoveItemAtPosition(position)
                .ConstrainedBy(x => x.Outfit.Id == id)
                .DownOne();
            return this.RedirectToAction(x => x.Edit(id));
        }

		[AdministratorsOnly, UnitOfWork]
        public ActionResult DeleteImage(int id, int outfitImageId)
        {
            var outfit = outfitRepository.GetById(id);
            var outfitImage = outfit.OutfitImages.SingleOrDefault(x => x.Id == outfitImageId);
            if (outfitImage != null)
            {
                outfit.OutfitImages.Remove(outfitImage);
            }
            return this.RedirectToAction(x => x.Edit(id));
        }

		[AdministratorsOnly]
        [HttpGet]
        public ActionResult AddToOutfit(int id)
		{
		    return View("AddToOutfit", new AddToOutfitViewModel
		    {
		        Outfit = new Outfit(),
                Product = productRepository.GetById(id)
		    });
		}

        [AdministratorsOnly, UnitOfWork]
        [HttpPost]
        public ActionResult AddToOutfit(AddToOutfitViewModel addToOutfitViewModel)
        {
            addToOutfitViewModel.Outfit.OutfitProducts.Add(new OutfitProduct
            {
                Outfit = addToOutfitViewModel.Outfit,
                Product = addToOutfitViewModel.Product,
                Position = imageOrderableService.NextPosition
            });
            return this.RedirectToAction(x => x.Item(addToOutfitViewModel.Outfit.Id));
        }

        [AdministratorsOnly, UnitOfWork]
        public ActionResult DeleteProduct(int id, int outfitProductId)
        {
            var outfit = outfitRepository.GetById(id);
            var outfitProduct = outfit.OutfitProducts.SingleOrDefault(x => x.Id == outfitProductId);
            if (outfitProduct != null)
            {
                outfit.OutfitProducts.Remove(outfitProduct);
            }
            return this.RedirectToAction(x => x.Edit(id));
        }
    }

    public class AddToOutfitViewModel
    {
        public Outfit Outfit { get; set; }
        public Product Product { get; set; }
    }
}