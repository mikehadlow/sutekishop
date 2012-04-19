using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Suteki.Common.Extensions;

namespace Suteki.Common.AddIns
{
    public class AddinControllerBase : Controller
    {
        /// <summary>
        /// Override View to provde a resource path for the view file.
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="masterName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override ViewResult View(string viewName, string masterName, object model)
        {
            if (viewName == null)
            {
                throw new ArgumentNullException("viewName", "You must provide a view name in AddinControllers");
            }

            var assembly = this.GetType().Assembly;
            var assemblyFileName = Path.GetFileName(assembly.Location);
            var assmblyName = assembly.GetName().Name;
            var controllerName = this.GetType().Name;
            if (!controllerName.EndsWith("Controller"))
            {
                throw new ApplicationException(
                    "Controllers must have a name ending with Controller, e.g: CustomerController");
            }
            var controllerShortenedName = controllerName.Substring(0, controllerName.Length - 10);

            // ~/App_Resource/Mike.MefAreas.AddIn.dll/Mike.MefAreas.AddIn/Views/Customer/
            var viewPath = string.Format("~/App_Resource/{0}/{1}/Views/{2}/", assemblyFileName, assmblyName, controllerShortenedName);

            // Suteki.Shop.StockControl.AddIn.Views.StockControl.List.ascx
            var resourcePath = "{0}.Views.{1}.{2}.".With(assmblyName, controllerShortenedName, viewName);
            var extension = "aspx";
            if (!ResourceExists(resourcePath + extension))
            {
                extension = "ascx";
                if (!ResourceExists(resourcePath + extension))
                {
                    throw new SutekiCommonException("Could not find view resource at '{0}aspx' or '{0}ascx' in assembly {1}", 
                        resourcePath,
                        GetType().Assembly);
                }
            }

            return base.View(viewPath + viewName + "." + extension, masterName, model);
        }

        private bool ResourceExists(string resrouceName)
        {
            return GetType().Assembly.GetManifestResourceNames().Any(x => x == resrouceName);
        }
    }
}