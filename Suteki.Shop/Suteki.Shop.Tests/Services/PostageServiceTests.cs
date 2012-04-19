using System;
using System.Linq;
using NUnit.Framework;
using Suteki.Common.Models;
using Suteki.Common.Repositories;
using Suteki.Shop.Services;
using Suteki.Shop.Tests.Models;
using System.Collections.Generic;
using Rhino.Mocks;

namespace Suteki.Shop.Tests.Services
{
    [TestFixture]
    public class PostageServiceTests
    {
        private IPostageService postageService;
        private IRepository<Postage> postageRepository;

        [SetUp]
        public void SetUp()
        {
            postageRepository = MockRepository.GenerateMock<IRepository<Postage>>();

            postageService = new PostageService(postageRepository);

            var postages = PostageTests.CreatePostages();
            postageRepository.Stub(pr => pr.GetAll()).Return(postages);
        }

        [Test]
        public void CalculatePostage_WhenNoPostagesGivenThenUseFlatRate()
        {
            // total weight = 350
            var basket = BasketTests.Create350GramBasket();
            var postages = new List<Postage>().AsQueryable();

            postageRepository.BackToRecord();
            postageRepository.Expect(pr => pr.GetAll()).Return(postages);
            postageRepository.Replay();

            Assert.IsFalse(postageService.CalculatePostageFor(basket).Phone, "phone is true");
            Assert.That(postageService.CalculatePostageFor(basket).Price.Amount, Is.EqualTo(10.00M), "Incorrect price calculated");
        }

        [Test]
        public void CalculatePostage_ShouldUseFlatRateOnMaxWeightBand()
        {
            var basket = BasketTests.Create450GramBasket();

            Assert.IsFalse(postageService.CalculatePostageFor(basket).Phone, "phone is true");
            Assert.That(postageService.CalculatePostageFor(basket).Price.Amount, Is.EqualTo(10.00M), "Incorrect price calculated");
        }

        [Test]
        public void CalculatePostage_ShouldPhoneIfPhoneOnMaxWeightIsTrue()
        {
            var basket = BasketTests.Create450GramBasket();

            // replace the order contact (AskIfMaxWeight is true, FlatRate is 123.45)
            basket.Country = new Country
            {
                PostZone = new PostZone {Multiplier = 2.5M, AskIfMaxWeight = true, FlatRate = new Money(123.45M)}
            };

            Assert.That(postageService.CalculatePostageFor(basket).Phone, Is.True, "phone is false");
        }
    }
}
