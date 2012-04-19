using System;
using System.Linq;
using System.Collections.Specialized;
using Suteki.Common.Extensions;

namespace Suteki.Shop.Services
{
    public class SizeService : ISizeService
    {
        NameValueCollection form;

        public ISizeService WithValues(NameValueCollection form)
        {
            this.form = form;
            return this;
        }

        public void Update(Product product)
        {
            if (form == null) throw new ApplicationException("form must be set with 'WithValues' before calling Update");

            if (product.DefaultSizeMissing)
            {
                product.AddDefaultSize();
            }

            var keys = form.AllKeys.Where(key => key.StartsWith("size_") && form[key].Length > 0);
            keys.ForEach(key => product.AddSize(new Size { Name = form[key], IsActive = true, IsInStock = true }));
        }
    }
}
