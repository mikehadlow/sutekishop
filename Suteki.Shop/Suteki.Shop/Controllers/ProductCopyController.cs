using System;
using System.Web.Mvc;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Shop.Filters;
using Suteki.Shop.Services;

namespace Suteki.Shop.Controllers
{
    [AdministratorsOnly]
    public class ProductCopyController : ControllerBase
    {
        readonly IRepository<Product> productRepository;
        readonly IUnitOfWorkManager unitOfWork;
        readonly IProductCopyService productCopyService;

        public ProductCopyController(
            IProductCopyService productCopyService, 
            IRepository<Product> productRepository, 
            IUnitOfWorkManager unitOfWork)
        {
            this.productCopyService = productCopyService;
            this.productRepository = productRepository;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost, UnitOfWork]
        public ActionResult Copy(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("product");
            }

            var copiedProduct = productCopyService.Copy(product);
            productRepository.SaveOrUpdate(copiedProduct);

            // we need an explicit commit here to get the copiedProduct id from the db
            unitOfWork.Commit();

            return RedirectToAction("Edit", "Product", new {Id = copiedProduct.Id});
        }
    }
}