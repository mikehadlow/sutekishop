using System;
using System.Web.Mvc;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Shop.Filters;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Controllers
{
    public class CommentAnswerController : ControllerBase
    {
        private readonly IRepository<IComment> commentRepsitory;

        public CommentAnswerController(IRepository<IComment> commentRepsitory)
        {
            this.commentRepsitory = commentRepsitory;
        }

        [AdministratorsOnly, HttpGet, UnitOfWork]
        public ViewResult Edit(int id)
        {
            var comment = commentRepsitory.GetById(id);
            return View("Edit", new CommentAnswerViewData
            {
                CommentId = comment.Id,
                Text = comment.Text,
                Reviewer = comment.Reviewer,
                Answer = comment.Answer
            });
        }

        [AdministratorsOnly, HttpPost, UnitOfWork]
        public ActionResult Edit(CommentAnswerViewData commentAnswerViewData)
        {
            var comment = commentRepsitory.GetById(commentAnswerViewData.CommentId);
            comment.Answer = commentAnswerViewData.Answer;
            return RedirectToAction("Index", "Reviews");
        }
    }
}