using System;
using Suteki.Common.Extensions;
using Suteki.Common.Repositories;
using System.Web;

namespace Suteki.Shop.Services
{
    public class BaseControllerService : IBaseControllerService
    {
        public IRepository<Category> CategoryRepository { get; private set; }
        public string GoogleTrackingCode { get; set; }
        public string MetaDescription { get; set; }
        private string shopName;
        private string emailAddress;
        private string copyright;
        private string phoneNumber;
        private string siteCss;
        private string facebookUserId;

        public BaseControllerService(IRepository<Category> categoryRepository)
        {
            this.CategoryRepository = categoryRepository;
        }

        public string EmailAddress
        {
            get 
            {
                if (string.IsNullOrEmpty(emailAddress)) return string.Empty;
                return emailAddress; 
            }
            set { emailAddress = value; }
        }

        public string ShopName
        {
            get
            {
                if (string.IsNullOrEmpty(shopName))
                {
                    return "Suteki Shop";
                }
                return shopName;
            }
            set { shopName = value; }
        }

        public virtual string SiteUrl 
        {
            get
            {

                Uri url = CurrentHttpContext.Request.Url;
                string relativePath = CurrentHttpContext.Request.ApplicationPath;

				if(! relativePath.EndsWith("/"))
				{
					relativePath += "/";
				}

                string port = (url.Port == 80) ? "" : ":{0}".With(url.Port.ToString());

                return "{0}://{1}{2}{3}".With(url.Scheme, url.Host, port, relativePath);
            }
        }

        public virtual HttpContext CurrentHttpContext
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    throw new ApplicationException("There is no current HttpContext");
                }
                return HttpContext.Current;
            }
        }

        public virtual string Copyright
        {
            get
            {
                if (string.IsNullOrEmpty(copyright)) return "Suteki Limited &copy; Copyright 2008";
                return copyright;
            }
            set { copyright = value; }
        }

        public string PhoneNumber
        {
            get
            {
                if (string.IsNullOrEmpty(phoneNumber)) return "";
                return phoneNumber;
            }
            set { phoneNumber = value; }
        }

        public string SiteCss
        {
            get
            {
                if (string.IsNullOrEmpty(siteCss)) return "Site.css";
                return siteCss;
            }
            set { siteCss = value; }
        }

        public string FacebookUserId
        {
            get
            {
                if (string.IsNullOrEmpty(facebookUserId)) return "";
                return facebookUserId;
            }
            set { facebookUserId = value; }
        }
    }
}
