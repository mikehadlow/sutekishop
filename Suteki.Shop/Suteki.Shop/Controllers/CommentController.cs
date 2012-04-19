using System;
using System.Web.Mvc;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;

namespace Suteki.Shop.Controllers
{
    public class CommentController : ControllerBase
    {
        readonly IRepository<Comment> commentRepository;

        public CommentController(IRepository<Comment> commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        [HttpGet]
        public ViewResult New()
        {
            return View("New");
        }

        [HttpPost, UnitOfWork]
        public ActionResult New(Comment comment)
        {
            if (ModelState.IsValid)
            {
                commentRepository.SaveOrUpdate(comment);
                return RedirectToAction("Confirm");
            }
            return View("New", comment);
        }

        public ViewResult Confirm()
        {
            return View("Confirm");
        }
    }
}