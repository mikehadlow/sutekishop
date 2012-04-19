// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class CommentAnswerControllerTests
    {
        private CommentAnswerController commentAnswerController;
        private IRepository<IComment> commentRepsitory;
        private const int commentId = 78;

        [SetUp]
        public void SetUp()
        {
            commentRepsitory = MockRepository.GenerateStub<IRepository<IComment>>();
            commentAnswerController = new CommentAnswerController((IRepository<IComment>) commentRepsitory);
        }

        [Test]
        public void Edit_GET_should_display_comment_answer_form()
        {
            var comment = new Comment
            {
                Id = commentId,
                Reviewer = "mikey",
                Text = "some text",
                Answer = "the answer"
            };

            commentRepsitory.Stub(x => x.GetById(commentId)).Return(comment);

            var viewData = commentAnswerController.Edit(commentId)
                .ReturnsViewResult()
                .ForView("Edit")
                .WithModel<CommentAnswerViewData>();

            viewData.CommentId.ShouldEqual(commentId);
            viewData.Reviewer.ShouldEqual(comment.Reviewer);
            viewData.Text.ShouldEqual(comment.Text);
            viewData.Answer.ShouldEqual(comment.Answer);
        }

        [Test]
        public void Edit_POST_should_update_comment_answer()
        {
            var comment = new Comment
            {
                Id = commentId,
                Reviewer = "mikey",
                Text = "some text",
                Answer = "the answer"
            };

            commentRepsitory.Stub(x => x.GetById(commentId)).Return(comment);

            var commentViewData = new CommentAnswerViewData
            {
                CommentId = commentId,
                Reviewer = "mikey",
                Text = "some text",
                Answer = "the new answer"
            };

            commentAnswerController.Edit(commentViewData)
                .ReturnsRedirectToRouteResult()
                .ToAction("Index")
                .ToController("Reviews");

            comment.Answer.ShouldEqual(commentViewData.Answer);
        }
    }
}

// ReSharper restore InconsistentNaming
