using System;

namespace Suteki.Common.Services
{
    public interface IEmailSender
    {
		[Obsolete("Please use the overload with the isBodyHtml parameter. This method defaults that parameter to false")]
        void Send(string toAddress, string subject, string body);
		
		[Obsolete("Please use the overload with the isBodyHtml parameter. This method defaults that parameter to false")]
		void Send(string[] toAddress, string subject, string body);
		
		void Send(string toAddress, string subject, string body, bool isBodyHtml);
		void Send(string[] toAddress, string subject, string body, bool isBodyHtml);
    }
}