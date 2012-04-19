// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Common.TestHelpers;
using Suteki.Shop.ActionResults;
using Suteki.Shop.Controllers;
using Suteki.Shop.Tests.Repositories;

namespace Suteki.Shop.Tests.Controllers
{
	[TestFixture]
	public class ReviewsControllerTests
	{
		ReviewsController controller;
		IRepository<Review> reviewRepository;
        FakeRepository<Product> productRepository;
	    IRepository<IComment> commentRepository;
	    Product product;

		[SetUp]
		public void Setup()
		{
            product = new Product
            {
                Reviews =
                    {
                        new Review(),
                        new Review()
                    }
            };

            productRepository = new FakeRepository<Product>(id =>
            {
                product.Id = id;
                return product;
            });

            reviewRepository = MockRepositoryBuilder.CreateReviewRepository();

            commentRepository = MockRepository.GenerateStub<IRepository<IComment>>();

            var comments = new List<IComment>
	        {
                new Comment{ Approved = true },
                new Comment{ Approved = false },
                new Review{ Approved = true },
                new Comment{ Approved = true }
	        }.AsQueryable();
            commentRepository.Stub(r => r.GetAll()).Return(comments);

            controller = new ReviewsController(reviewRepository, productRepository, commentRepository);
		}

		[Test]
		public void Show_should_get_reviews_for_product()
		{
			controller.Show(1)
				.ReturnsViewResult()
				.WithModel<Product>()
				.AssertAreSame(productRepository.GetById(1), x => x)
				.AssertAreEqual(2, x => x.Reviews.Count());
		}

		[Test]
		public void New_renders_view()
		{
			controller.New(5)
				.ReturnsViewResult()
				.WithModel<Review>()
				.AssertAreSame(product, x => x.Product);
		}

		[Test]
		public void NewWithPost_saves_review()
		{
            var review = new Review { Product = new Product { Id = 5 } };
			
			controller.New(review)
				.ReturnsRedirectToRouteResult()
				.ToAction("Submitted")
                .WithRouteValue("Id", "5");

			reviewRepository.AssertWasCalled(x => x.SaveOrUpdate(review));
		}

		[Test]
		public void NewWithPost_renders_view_on_error()
		{
			controller.ModelState.AddModelError("foo", "bar");
			var review = new Review();

		    controller.New(review)
		        .ReturnsViewResult()
		        .WithModel<Review>()
		        .AssertAreSame(review);
		}

		[Test]
		public void Submitted_renders_view()
		{
			controller.Submitted(1)
				.ReturnsViewResult()
				.WithModel<Product>()
				.AssertAreSame(product);
		}

		[Test]
		public void Index_displays_unapproved_reviews()
		{
			controller.Index()
				.ReturnsViewResult()
                .ForView("Index")
				.WithModel<IEnumerable<IComment>>()
				.AssertAreEqual(1, x => x.Count());
		}

		[Test]
		public void Approve_approves_review()
		{
			var review = new Review();
            commentRepository.Stub(x => x.GetById(5)).Return(review);

			controller.Approve(5)
				.ReturnsRedirectToRouteResult()
				.ToAction("Index");

			review.Approved.ShouldBeTrue();
		}

		[Test]
		public void Delete_deletes_review()
		{
			var review = new Review();
            commentRepository.Expect(x => x.GetById(5)).Return(review);

			controller.Delete(5)
				.ReturnsResult<RedirectToReferrerResult>();

            commentRepository.AssertWasCalled(x => x.DeleteOnSubmit(review));
		}

	    [Test]
	    public void AllApproved_shows_all_reviews()
	    {
	        controller.AllApproved()
	            .ReturnsViewResult()
	            .ForView("AllApproved")
	            .WithModel<IEnumerable<IComment>>()
	            .AssertAreEqual(3, vd => vd.Count());
	    }
	}
}
// ReSharper restore InconsistentNaming
