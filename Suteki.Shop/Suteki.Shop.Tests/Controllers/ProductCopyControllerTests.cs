// ReSharper disable InconsistentNaming
using System;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;
using Suteki.Shop.Services;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class ProductCopyControllerTests
    {
        ProductCopyController productCopyController;
        IProductCopyService productCopyService;
        FakeRepository<Product> productRepository;
        IUnitOfWorkManager unitOfWork;

        [SetUp]
        public void SetUp()
        {
            productCopyService = MockRepository.GenerateStub<IProductCopyService>();
            productRepository = new FakeRepository<Product>();
            unitOfWork = MockRepository.GenerateStub<IUnitOfWorkManager>();

            productCopyController = new ProductCopyController(
                productCopyService,
                productRepository,
                unitOfWork);
        }

        [Test]
        public void Copy_should_create_and_save_a_copy_of_the_product()
        {
            const int productId = 5;
            const int copiedId = 7;
            var originalProduct = new Product { Id = productId };
            var copiedProduct = new Product();

            productCopyService.Stub(s => s.Copy(originalProduct)).Return(copiedProduct);
            productRepository.SaveOrUpdateDelegate = product =>
            {
                product.Id = copiedId;
            };

            productCopyController.Copy(originalProduct)
                .ReturnsRedirectToRouteResult()
                .ToController("Product")
                .ToAction("Edit")
                .WithRouteValue("id", "7");

            unitOfWork.AssertWasCalled(u => u.Commit());
        }
    }
}
// ReSharper restore InconsistentNaming