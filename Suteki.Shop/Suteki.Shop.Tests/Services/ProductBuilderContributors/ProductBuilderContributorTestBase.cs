using System;
using System.Web;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Events;
using Suteki.Shop.Services;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Tests.Services.ProductBuilderContributors
{
    public abstract class ProductBuilderContributorTestBase
    {
        protected ProductBuildingContext context;
        protected IProductBuilderContributor contributor;

        [SetUp]
        public void SetUp()
        {
            DomainEvent.TurnOff();

            context = new ProductBuildingContext(
                new ProductViewData(), 
                new ModelStateDictionary(), 
                MockRepository.GenerateStub<HttpRequestBase>());

            contributor = InitContributor();
        }

        [TearDown]
        public void TearDown()
        {
            DomainEvent.Reset();
        }

        protected abstract IProductBuilderContributor InitContributor();
    }
}