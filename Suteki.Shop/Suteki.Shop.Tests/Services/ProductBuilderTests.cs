// ReSharper disable InconsistentNaming
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using NUnit.Framework;
using Suteki.Shop.Services;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Tests.Services
{
    [TestFixture]
    public class ProductBuilderTests
    {
        ProductViewData productViewData;
        ModelStateDictionary modelStateDictionary;

        [SetUp]
        public void SetUp()
        {
            productViewData = new ProductViewData();
            modelStateDictionary = new ModelStateDictionary();
        }

        [Test]
        public void Should_build_a_new_product_from_ProductViewData()
        {
            var orders = new List<int>();
            var expectedProduct = new Product();

            var contributor1 = new MockProductBuilderContributor(2, ctx =>
            {
                ctx.ProductViewData.ShouldBeTheSameAs(productViewData);
                orders.Add(2);
            });
            var contributor2 = new MockProductBuilderContributor(1, ctx =>
            {
                ctx.ProductViewData.ShouldBeTheSameAs(productViewData);
                orders.Add(1);
                ctx.SetProduct(expectedProduct);
            });

            IProductBuilder productBuilder = new ProductBuilder(new[]{ contributor1, contributor2 });

            var product = productBuilder.ProductFromProductViewData(productViewData, modelStateDictionary, new MockHttpRequest());

            product.ShouldBeTheSameAs(expectedProduct);

            orders[0].ShouldEqual(1);
            orders[1].ShouldEqual(2);
        }

        [Test]
        public void Should_not_process_contributor_if_ModelStateDictionary_is_invalid()
        {
            var contributor1 = new MockProductBuilderContributor(1, ctx => ctx.ModelStateDictionary.AddModelError("x", "message"));
            var contributor2 = new MockProductBuilderContributor(2, ctx => Assert.Fail("contributor2 should not be invoked"));

            IProductBuilder productBuilder = new ProductBuilder(new[] { contributor1, contributor2 });

            var product = productBuilder.ProductFromProductViewData(productViewData, modelStateDictionary, new MockHttpRequest());
        }
    }

    public class MockProductBuilderContributor : IProductBuilderContributor
    {
        readonly int order;
        readonly Action<ProductBuildingContext> contextAction;

        public MockProductBuilderContributor(int order, Action<ProductBuildingContext> contextAction)
        {
            this.order = order;
            this.contextAction = contextAction;
        }

        public void ContributeTo(ProductBuildingContext context)
        {
            contextAction(context);
        }

        public int Order
        {
            get { return order; }
        }
    }

    public class MockHttpRequest : HttpRequestBase
    {
        
    }
}
// ReSharper restore InconsistentNaming