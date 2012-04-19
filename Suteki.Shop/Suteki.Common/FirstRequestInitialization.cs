using System;
using System.Web;

namespace Suteki.Common
{
	// from http://mvolo.com/blogs/serverside/archive/2007/11/10/Integrated-mode-Request-is-not-available-in-this-context-in-Application_5F00_Start.aspx
	public static class FirstRequestInitialization
	{
		static bool initializedAlready;
		static readonly Object lockObj = new Object();
		public static void Initialize(HttpContext context, Action initializationAction)
		{
			if (initializedAlready)
			{
				return;
			}
			lock (lockObj)
			{
				if (initializedAlready)
				{
					return;
				}
				initializedAlready = true;
				initializationAction();
			}
		}
	}
}