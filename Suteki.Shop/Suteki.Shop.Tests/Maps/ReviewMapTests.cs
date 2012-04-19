// ReSharper disable InconsistentNaming
using System;
using System.Linq;
using NUnit.Framework;
using NHibernate.Linq;
using Suteki.Common.TestHelpers;

namespace Suteki.Shop.Tests.Maps
{
    [TestFixture]
    public class ReviewMapTests : MapTestBase
    {
        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void ShouldBeAbleToSaveAndRetrieveReviews()
        {
            InSession(session =>
            {
                var product = new Product
                {
                    Name = "My Cool Product"
                };
                session.Save(product);

                var review = new Review
                {
                    Approved = true,
                    Text = "Great product",
                    Reviewer = "Mike",
                    Product = product
                };

                session.Save(review);

                var comment = new Comment
                {
                    Approved = true,
                    Text = "Great site",
                    Reviewer = "John"
                };
                session.Save(comment);
            });

            Console.WriteLine("IComment");
            InSession(session =>
            {
                var comments = session.Query<IComment>().OrderByDescending(c => c.Id);
                foreach (var comment in comments)
                {
                    Console.WriteLine("{0}: {1}", comment.Id, comment.Reviewer);
                }
            });

            Console.WriteLine("Reviews");
            InSession(session =>
            {
                var reviews = session.Query<Review>();
                foreach (var review in reviews)
                {
                    Console.WriteLine("{0}: {1}", review.Id, review.Reviewer);
                }
            });
        }
    }
}
// ReSharper restore InconsistentNaming