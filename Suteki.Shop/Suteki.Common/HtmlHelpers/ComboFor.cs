using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Suteki.Common.Extensions;
using Suteki.Common.Models;
using Suteki.Common.Repositories;

namespace Suteki.Common.HtmlHelpers
{
    public interface IComboFor<TEntity, TModel> where TEntity : class, INamedEntity
    {
        ComboFor<TEntity, TModel> Multiple();
        string BoundTo(Expression<Func<TModel, TEntity>> propertyExpression, Expression<Func<TEntity, bool>> whereClause);
        string BoundTo(Expression<Func<TModel, TEntity>> propertyExpression, string propertyNamePrefix);
        string BoundTo(Expression<Func<TModel, TEntity>> propertyExpression);
        string BoundTo(Expression<Func<TModel, TEntity>> propertyExpression, Expression<Func<TEntity, bool>> whereClause, string propertyNamePrefix);
        string BoundTo(string propertyToBind, IEnumerable<int> selectedIds);
    }

    public class ComboFor<TEntity, TModel> : IComboFor<TEntity, TModel>, IRequireHtmlHelper<TModel> where TEntity : class, INamedEntity
    {
        readonly IRepository<TEntity> repository;
        protected Expression<Func<TEntity, bool>> WhereClause { get; set; }
        protected string PropertyNamePrefix { get; set; }
        protected bool mutiple = false;

        public ComboFor(IRepository<TEntity> repository)
        {
            this.repository = repository;
        }

        public ComboFor<TEntity, TModel> Multiple()
        {
            mutiple = true;
            return this;
        }

        public string BoundTo(Expression<Func<TModel, TEntity>> propertyExpression, Expression<Func<TEntity, bool>> whereClause, string propertyNamePrefix)
        {
            WhereClause = whereClause;
            PropertyNamePrefix = propertyNamePrefix;
            return BoundTo(propertyExpression);
        }

        public string BoundTo(Expression<Func<TModel, TEntity>> propertyExpression, Expression<Func<TEntity, bool>> whereClause)
        {
            WhereClause = whereClause;
            return BoundTo(propertyExpression);
        }

        public string BoundTo(Expression<Func<TModel, TEntity>> propertyExpression, string propertyNamePrefix)
        {
            PropertyNamePrefix = propertyNamePrefix;
            return BoundTo(propertyExpression);
        }

        public string BoundTo(Expression<Func<TModel, TEntity>> propertyExpression)
        {
            var getPropertyValue = propertyExpression.Compile();
            var propertyName = (!String.IsNullOrEmpty(PropertyNamePrefix) ? PropertyNamePrefix : "")
                + Utils.ExpressionHelper.GetDottedPropertyNameFromExpression(propertyExpression) + ".Id";

            var viewDataModelIsNull = (!typeof(TModel).IsValueType) && HtmlHelper.ViewData.Model == null;
            var selectedId = viewDataModelIsNull ? 0 : getPropertyValue(HtmlHelper.ViewData.Model).Id;
            return BuildCombo(propertyName, selectedId);
        }

        public string BoundTo(string propertyToBind, IEnumerable<int> selectedIds)
        {
            return BuildCombo(propertyToBind, selectedIds.ToArray());
        }

        public string BoundTo(string propertyToBind, IEnumerable<int> selectedIds, Expression<Func<TEntity, bool>> whereClause)
        {
            WhereClause = whereClause;
            return BuildCombo(propertyToBind, selectedIds.ToArray());
        }

        public override string ToString()
        {
            return BuildCombo(typeof(TEntity).Name + "Id");
        }

        protected virtual string BuildCombo(string htmlId, params int[] selectedIds)
        {
            if (string.IsNullOrEmpty(htmlId))
            {
                throw new ArgumentException("htmlId can not be null or empty");
            }

            var selectListItems = GetSelectListItems(selectedIds);
            var result = DropDownListBuilder.DropDownList(htmlId, selectListItems, mutiple);
            return result;
        }

        protected IEnumerable<SelectListItem> GetSelectListItems(int[] selectedIds)
        {
            var queryable = repository.GetAll();
            if (WhereClause != null)
            {
                queryable = queryable.Where(WhereClause);
            }
            var enumerable = queryable.AsEnumerable();

            if (typeof(TEntity).IsOrderable())
            {
                enumerable = enumerable.Select(x => (IOrderable)x).InOrder().Select(x => (TEntity)x);
            }
            
            if (typeof(TEntity).IsActivatable())
            {
                enumerable = enumerable.Select(x => (IActivatable)x).Where(a => a.IsActive).Select(x => (TEntity)x);
            }
            
            var items = enumerable
                .Select(e => new SelectListItem
                {
                    Selected = selectedIds.Any(id => id == e.Id), Text = e.Name, Value = e.Id.ToString()
                });

            return items;
        }

        public HtmlHelper<TModel> HtmlHelper { get; set; }
    }

    /// <summary>
    /// Not using HtmlHelper.DropDownList because it tries to replace selected items from binding state.
    /// </summary>
    public class DropDownListBuilder
    {
        public static string DropDownList(string htmlId, IEnumerable<SelectListItem> selectListItems, bool allowMultiple)
        {
            // Convert each ListItem to an <option> tag
            var listItemBuilder = new StringBuilder();

            foreach (SelectListItem item in selectListItems)
            {
                listItemBuilder.AppendLine(ListItemToOption(item));
            }


            var tagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };
//            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("name", htmlId, true /* replaceExisting */);
            tagBuilder.GenerateId(htmlId);
            if (allowMultiple)
            {
                tagBuilder.MergeAttribute("multiple", "multiple");
            }

            return tagBuilder.ToString();
        }

        public static string ListItemToOption(SelectListItem item)
        {
            var builder = new TagBuilder("option")
            {
                InnerHtml = HttpUtility.HtmlEncode(item.Text)
            };
            if (item.Value != null)
            {
                builder.Attributes["value"] = item.Value;
            }
            if (item.Selected)
            {
                builder.Attributes["selected"] = "selected";
            }
            return builder.ToString(TagRenderMode.Normal);
        }
    }
}