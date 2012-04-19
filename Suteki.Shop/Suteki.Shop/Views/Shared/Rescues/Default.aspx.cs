using System;
using System.Web.Mvc;
using Castle.Core.Logging;

namespace Suteki.Shop.Views.Shared.Rescues
{
    public partial class Default : ViewPage<HandleErrorInfo>
    {
        protected override void OnLoad(EventArgs e)
        {
            LogException(ViewData.Model.Exception);
        }

        public void LogException(Exception exception)
        {
            var controller = this.ViewContext.Controller as Controllers.ControllerBase;
            if (controller == null)
            {
                return;
            }
            ILogger logger = controller.Logger;
            if (logger == null)
            {
                return;
            }
            logger.Error("Exception in Suteki Shop", exception);
        }
    }
}