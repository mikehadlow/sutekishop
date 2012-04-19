using System.Linq;
using NUnit.Framework;
using Suteki.Common.Events;
using Suteki.Shop.Exports.Events;

namespace Suteki.Shop.Tests.Models
{
    [TestFixture]
    public class ProductTests 
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void PlainTextDescription_should_strip_HTML_and_quotes()
        {
            var product = new Product
            {
                Description = description
            };
            product.PlainTextDescription.ShouldEqual(expectedPlainTextDescription);
        }

        [Test]
        public void AddSize_should_raise_domain_event()
        {
            const int productId = 97;
            const string productName = "Widget";
            const int sizeId = 34;
            const string sizeName = "Small";

            var product = new Product
            {
                Name = productName,
                Id = productId
            };

            var size = new Size
            {
                Id = sizeId,
                Name = "Small"
            };

            IDomainEvent @event = null;
            using(DomainEvent.TestWith(e => { @event = e; }))
            {
                product.AddSize(size);
            }

            var sizeCreatedEvent = @event as SizeCreatedEvent;
            sizeCreatedEvent.ShouldNotBeNull();
            sizeCreatedEvent.ProductName.ShouldEqual(productName);
            sizeCreatedEvent.SizeName.ShouldEqual(sizeName);
        }

        [Test]
        public void RemoveAllSizes_should_raise_domain_event()
        {
            const string productName = "Widget";

            var product = new Product
            {
                Name = productName
            };

            IDomainEvent @event = null;
            using (DomainEvent.TestWith(e => { @event = e; }))
            {
                product.ClearAllSizes();
            }

            var sizesDeactivatedEvent = @event as SizesDeactivatedEvent;
            sizesDeactivatedEvent.ShouldNotBeNull();
            sizesDeactivatedEvent.ProductName.ShouldEqual(productName);
        }

        [Test]
        public void Changing_product_name_should_raise_domain_event()
        {
            const string oldName = "Widget";
            const string newName = "Gadget";

            var product = new Product();

            IDomainEvent @event = null;
            using (DomainEvent.TestWith(e => { @event = e; }))
            {
                product.Name = oldName;
                @event.ShouldBeNull(); // event should not be raised on initial set.

                product.Name = newName;
            }

            var productNameChangedEvent = @event as ProductNameChangedEvent;
            productNameChangedEvent.ShouldNotBeNull();
            productNameChangedEvent.OldProductName.ShouldEqual(oldName);
            productNameChangedEvent.NewProductName.ShouldEqual(newName);
        }

        [Test]
        public void ActiveReviews_should_only_return_active_reviews()
        {
            var product = new Product();

            product.Reviews.Add(new Review { Id = 1, Approved = false });
            product.Reviews.Add(new Review { Id = 2, Approved = true });
            product.Reviews.Add(new Review { Id = 3, Approved = false });

            product.ActiveReviews.Count().ShouldEqual(1);
            product.ActiveReviews.First().Id.ShouldEqual(2);

        }

        const string description = 
@"<p>This is an exact copy by ""North Sea Clothing"" of the Royal Navy sweater worn during the Atlantic convoys of WW2.</p>
<p>Made in the Uk from 100% English wool. Adapted by a lot of Rockers in the 50's to go under their leathers and we think well worthy of a place on our site and in our shop.</p>
<p>Reasonably fitted with a deep ribbed waistband.</p>

<p>Also available in White. Not all in stock at the moment but we will let you know how long once order placed. We hope within a week.</p>";

        const string expectedPlainTextDescription =
@"This is an exact copy by North Sea Clothing of the Royal Navy sweater worn during the Atlantic convoys of WW2.Made in the Uk from 100% English wool. Adapted by a lot of Rockers in the 50's to go under their leathers and we think well worthy of a place on our site and in our shop.Reasonably fitted with a deep ribbed waistband.Also available in White. Not all in stock at the moment but we will let you know how long once order placed. We hope within a week.";
    }
}