using System.Configuration;

namespace Suteki.Common.Services
{
	public interface IAppSettings
	{
		string GetSetting(string key);
	}

	public class AppSettings : IAppSettings
	{
		public static string UseSsl = "useSsl";
		
		public string GetSetting(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}
	}
}