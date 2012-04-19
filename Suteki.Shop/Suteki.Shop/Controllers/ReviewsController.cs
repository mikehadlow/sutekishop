using System.Linq;
using System.Web.Mvc;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Shop.ActionResults;
using Suteki.Shop.Filters;
using Suteki.Shop.Repositories;
using MvcContrib;
namespace Suteki.Shop.Controllers
{
	public class ReviewsController : ControllerBase
	{
		readonly IRepository<Review> reviewRepository;
	    readonly IRepository<IComment> commentsRepository;
		readonly IRepository<Product> productRepository;

		public ReviewsController(
            IRepository<Review> reviewRepository, 
            IRepository<Product> productRepository, 
            IRepository<IComment> commentsRepository)
		{
			this.reviewRepository = reviewRepository;
		    this.commentsRepository = commentsRepository;
		    this.productRepository = productRepository;
		}

		[AdministratorsOnly]
		public ActionResult Index()
		{
			return View("Index", commentsRepository.GetAll().Unapproved().ToList());
		}

		public ActionResult Show(int id)
		{
			var product = productRepository.GetById(id);
			return View(product);
		}

		public ActionResult New(int id)
		{
			var product = productRepository.GetById(id);

			return View(new Review
			{
				Product = product
			});
		}

		[AcceptVerbs(HttpVerbs.Post), UnitOfWork]
		public ActionResult New(Review review)
		{
            if (ModelState.IsValid)
			{
				reviewRepository.SaveOrUpdate(review);
				return this.RedirectToAction(x => x.Submitted(review.Product.Id));
			}

			return View(review);
		}

		public ActionResult Submitted(int id)
		{
			return View(productRepository.GetById(id));
		}

		[AdministratorsOnly, AcceptVerbs(HttpVerbs.Post), UnitOfWork]
		public ActionResult Approve(int id)
		{
            var review = commentsRepository.GetById(id);
			review.Approved = true;

			return this.RedirectToAction(x => x.Index());
		}

		[AdministratorsOnly, AcceptVerbs(HttpVerbs.Post), UnitOfWork]
		public ActionResult Delete(int id)
		{
            var review = commentsRepository.GetById(id);
            commentsRepository.DeleteOnSubmit(review);

			return new RedirectToReferrerResult();
		}

        [HttpGet, UnitOfWork]
	    public ActionResult AllApproved()
        {
            var comments = commentsRepository.GetAll().Approved().OrderByDescending(r => r.Id).ToList();
            return View("AllApproved", comments);
        }
	}
}