// ReSharper disable InconsistentNaming
using System.Linq;
using NUnit.Framework;
using Suteki.Shop.Exports.Events;
using Suteki.Shop.Handlers;
using Suteki.Shop.Services;

namespace Suteki.Shop.Tests.Handlers
{
    [TestFixture]
    public class ChangeInStockStatusOnStockItemInStockChangedTests
    {
        private ChangeInStockStatusOnStockItemInStockChanged handler;
        private DummyRepository<Size> sizeRepository;

        [SetUp]
        public void SetUp()
        {
            sizeRepository = new DummyRepository<Size>();
            handler = new ChangeInStockStatusOnStockItemInStockChanged(sizeRepository);
        }

        [Test]
        public void Handle_should_update_correct_size()
        {
            var widget = new Product { Name = "widget"};
            var gadget = new Product { Name = "gadget"};

            var sizes = new[]
            {
                new Size {Product = widget, Name = "Small", IsActive = false, IsInStock = false},
                new Size {Product = widget, Name = "Medium", IsActive = true, IsInStock = false},
                new Size {Product = widget, Name = "Small", IsActive = true, IsInStock = false}, // should update this one
                new Size {Product = gadget, Name = "Small", IsActive = true, IsInStock = false},
            };

            sizeRepository.GetAllDelegate = () => sizes.AsQueryable();

            var @event = new StockItemInStockChanged("Small", "widget", true);

            handler.Handle(@event);

            sizes[2].IsInStock.ShouldBeTrue();

            sizes[0].IsInStock.ShouldBeFalse();
            sizes[1].IsInStock.ShouldBeFalse();
            sizes[3].IsInStock.ShouldBeFalse();
        }

        [Test]
        public void Handle_should_update_default_size()
        {
            var widget = new Product {Name = "widget"};
            var sizes = new[]
            {
                new Size {Product = widget, Name = "-", IsActive = false, IsInStock = false},
            };

            sizeRepository.GetAllDelegate = () => sizes.AsQueryable();

            var @event = new StockItemInStockChanged("-", "widget", true);

            handler.Handle(@event);

            sizes[0].IsInStock.ShouldBeTrue();
        }
    }
}

// ReSharper restore InconsistentNaming
