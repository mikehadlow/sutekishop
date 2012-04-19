// ReSharper disable InconsistentNaming
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using NUnit.Framework;
using Suteki.Common.Models;
using Suteki.Common.HtmlHelpers;
using Suteki.Common.Tests.TestHelpers;

namespace Suteki.Common.Tests.HtmlHelpers
{
    [TestFixture]
    public class PostActionTests
    {
        PostAction<TestController> postAction;
        StringWriter writer;

        [SetUp]
        public void SetUp()
        {
            writer = new StringWriter();
            var htmlHelper = MvcTestHelpers.CreateMockHtmlHelper(writer);
            var entity = new TestEntity {Id = 4};

            postAction = new PostAction<TestController>(htmlHelper, c => c.DoSomething(entity), "the button text");
        }

        [Test]
        public void Render_should_render_the_form_correctly()
        {
            postAction.Render();

            var result = writer.GetStringBuilder().ToString();

            Console.Out.WriteLine("result = {0}", result);
        }

        [Test]
        public void GetExpressionDetails_should_work()
        {
            var entity = new TestEntity { Id = 4 };
            Expression<Action<TestController>> action = c => c.DoSomething(entity);

            var expressionDetails = PostAction<TestController>.GetExpressionDetails(action);

            expressionDetails.MethodName.ShouldEqual("DoSomething");
            expressionDetails.IdValue.ShouldEqual(4);
        }

        [Test]
        public void GetExpressionDetails_should_work_on_an_enumerated_type()
        {
            var entities = new List<TestEntity>
            {
                new TestEntity { Id = 3 },
                new TestEntity { Id = 5 },
                new TestEntity { Id = 7 }
            };

            foreach (var entity in entities)
            {
                Expression<Action<TestController>> action = c => c.DoSomething(entity);
                var expressionDetails = PostAction<TestController>.GetExpressionDetails(action);

                expressionDetails.MethodName.ShouldEqual("DoSomething");
                expressionDetails.IdValue.ShouldEqual(entity.Id);
            }
        }

        const string expectedResult = @"";
    }

    public class TestController : Controller
    {
        public ActionResult DoSomething(TestEntity entity)
        {
            return null;
        }
    }

    public class TestEntity : IEntity
    {
        public int Id { get; set; }
    }
}
// ReSharper restore InconsistentNaming