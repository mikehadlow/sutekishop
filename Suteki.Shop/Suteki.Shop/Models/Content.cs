using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Web.Mvc;
using Suteki.Common;
using Suteki.Common.Extensions;
using Suteki.Common.Models;
using System.Web.Mvc;
using Suteki.Shop.Controllers;
using Suteki.Shop.Models.ModelHelpers;
using Suteki.Common.HtmlHelpers;

namespace Suteki.Shop
{
    public class Content : IOrderable, IActivatable, IUrlNamed, INamedEntity
    {
        public virtual int Id { get; set; }

        string name;
        [Required(ErrorMessage = "Name is required")]
        public virtual string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Content.Name cannot be set to a null or empty value");
                }
                name = value;
                UrlName = Name.ToUrlFriendly();
            }
        }

        public virtual string UrlName { get; set; }
        public virtual int Position { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual Menu ParentContent { get; set; } 

        IList<Content> contents = new List<Content>();
        public virtual IList<Content> Contents
        {
            get { return contents; }
            set { contents = value; }
        }

        public virtual bool IsTextContent
        {
            get
            {
                return this is ITextContent;
            }
        }

        public virtual bool IsMenu
        {
            get
            {
                return this is Menu;
            }
        }

        public virtual bool IsActionContent
        {
            get
            {
                return this is ActionContent;
            }
        }

        public virtual string Type
        {
            get
            {
                if (IsMenu) return "Menu";
                if (IsTextContent) return "Page";
                if (IsActionContent) return "Action";
                throw new ApplicationException("Unknown Type");
            }
        }

        public virtual bool HasSubMenu
        {
            get
            {
                if (ParentContent != null)
                {
                    return ParentContent.Id != Menu.MainMenuId;
                }
                return false;
            }
        }

        public virtual Menu SubMenu
        {
            get
            {
                var thisMenu = this.CastAs<Menu>();
                if (thisMenu != null)
                {
                    if (thisMenu.IsMainMenu) return null;
                    if (thisMenu.Menu != null && Menu.IsMainMenu) return thisMenu;
                }

				return Menu == null ? null : Menu.SubMenu;
            }
        }

        public virtual Menu Menu
        {
            get
            {
                if (Id == Menu.MainMenuId) return null;
               /* var menu = Content1 as Menu;
                if (menu == null)
                    throw new NoMenuException("Parent Content Should Always be a Menu");*/
                return ParentContent.CastAs<Menu>();
            }
        }

        public virtual MvcHtmlString Link(HtmlHelper htmlHelper)
        {
            if (Id == 0) return MvcHtmlString.Empty;
            return htmlHelper.ActionLink<CmsController>(c => c.Index(UrlName), Name);
        }

        public virtual string Url(UrlHelper urlHelper)
		{
			return urlHelper.Action<CmsController>(c => c.Index(UrlName));
		}

        public virtual MvcHtmlString EditLink(HtmlHelper htmlHelper)
        {
            return MvcHtmlString.Create("&nbsp;");
        }

        public virtual bool CanEdit(User user)
        {
            if (Id == 0) return false;
            return user.IsAdministrator;
        }
    }
}
