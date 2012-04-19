using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Suteki.Common.Models;

namespace Suteki.Common.HtmlHelpers
{
    public class PostAction<TController>
    {
        readonly HtmlHelper htmlHelper;
        readonly Expression<Action<TController>> action;
        readonly string buttonText;

        const string controllerString = "Controller";

        public PostAction(HtmlHelper htmlHelper, Expression<Action<TController>> action, string buttonText)
        {
            this.htmlHelper = htmlHelper;
            this.action = action;
            this.buttonText = buttonText;
        }

        public void Render()
        {
            var controllerTypeName = typeof (TController).Name;
            if (!typeof (Controller).IsAssignableFrom(typeof (TController)))
            {
                throw new SutekiCommonException("'{0}' is not a controller", controllerTypeName);
            }
            if (!controllerTypeName.EndsWith(controllerString))
            {
                throw new SutekiCommonException("'{0}' does not end with '{1}'", controllerTypeName, controllerString);
            }

            var expressionDetails = GetExpressionDetails(action);

            var controllerName = controllerTypeName.Substring(0, controllerTypeName.Length - controllerString.Length);

            using (htmlHelper.BeginForm(expressionDetails.MethodName, controllerName, FormMethod.Post, new { @class = "postAction" }))
            {
                htmlHelper.ViewContext.Writer.Write("<input type=\"hidden\" id=\"Id\" name=\"Id\" value=\"" + expressionDetails.IdValue + "\" />");
                //htmlHelper.ViewContext.Writer.Write(htmlHelper.Hidden("Id", expressionDetails.IdValue));
                htmlHelper.ViewContext.Writer.Write("<input type=\"submit\" value=\"" + buttonText + "\" class=\"postAction\" />");
            }
        }

        public static ExpressionDetails GetExpressionDetails(Expression<Action<TController>> action)
        {
            var body = action.Body as MethodCallExpression;
            if (body == null)
            {
                throw new SutekiCommonException("the action must be a call to a controller action.");
            }
            if (body.Arguments.Count != 1)
            {
                throw new SutekiCommonException("The controller action should only have one entity argument.");
            }

            var arg0 = body.Arguments[0];
            if (!typeof (IEntity).IsAssignableFrom(arg0.Type))
            {
                throw new SutekiCommonException("The controller action argument must implement IEntity");
            }

            var member = arg0 as MemberExpression;
            if (member == null)
            {
                throw new SutekiCommonException("arg0 is not a MemberExpression");
            }

            var func = Expression.Lambda<Func<IEntity>>(Expression.Convert(arg0, typeof(IEntity))).Compile();
            var entity = func();

            return new ExpressionDetails
            {
                MethodName = body.Method.Name,
                IdValue = entity.Id
            };    
        }

        public struct ExpressionDetails
        {
            public string MethodName { get; set; }
            public int IdValue { get; set; }
        }
    }
}