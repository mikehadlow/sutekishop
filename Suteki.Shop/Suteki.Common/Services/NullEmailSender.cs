using System;

namespace Suteki.Common.Services
{
    public class NullEmailSender : IEmailSender
    {
		[Obsolete("Please use the overload with the isBodyHtml parameter. This method defaults that parameter to false")]
        public void Send(string toAddress, string subject, string body)
        {
            // do nothing
        }

		[Obsolete("Please use the overload with the isBodyHtml parameter. This method defaults that parameter to false")]
		public void Send(string[] toAddress, string subject, string body)
		{
			// do nothing
		}

    	public void Send(string toAddress, string subject, string body, bool isBodyHtml)
    	{
			// do nothing
    	}

    	public void Send(string[] toAddress, string subject, string body, bool isBodyHtml)
    	{
			// do nothing
    	}
    }
}
