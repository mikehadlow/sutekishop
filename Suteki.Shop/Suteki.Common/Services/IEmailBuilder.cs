using System.Collections.Generic;

namespace Suteki.Common.Services
{
	/// <summary>
	/// Provide the base method and property to build email
	/// </summary>
	public interface IEmailBuilder
	{
		/// <summary>
		/// Get the email content
		/// </summary>
		/// <returns>Return the email content.</returns>
		string GetEmailContent(string templateName, IDictionary<string, object> viewdata);
	}
}
