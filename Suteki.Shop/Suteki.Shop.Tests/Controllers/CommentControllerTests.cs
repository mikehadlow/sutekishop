// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class CommentControllerTests
    {
        CommentController commentController;
        IRepository<Comment> commentRepository;

        [SetUp]
        public void SetUp()
        {
            commentRepository = MockRepository.GenerateStub<IRepository<Comment>>();
            commentController = new CommentController(commentRepository);   
        }

        [Test]
        public void New_should_show_new_view()
        {
            commentController.New()
                .ReturnsViewResult()
                .ForView("New");
        }

        [Test]
        public void New_POST_should_insert_the_new_comment()
        {
            var comment = new Comment();
            commentController.New(comment)
                .ReturnsRedirectToRouteResult()
                .ToAction("Confirm");

            commentRepository.AssertWasCalled(r => r.SaveOrUpdate(comment));
        }

        [Test]
        public void New_POST_should_show_New_view_when_model_errors_occur()
        {
            var comment = new Comment();
            commentController.ModelState.AddModelError("foo", "bar");
            commentController.New(comment)
                .ReturnsViewResult()
                .ForView("New")
                .WithModel<Comment>()
                .AssertAreSame(comment);
        }

        [Test]
        public void Confirm_should_show_confirm_view()
        {
            commentController.Confirm()
                .ReturnsViewResult()
                .ForView("Confirm");
        }
    }
}
// ReSharper restore InconsistentNaming