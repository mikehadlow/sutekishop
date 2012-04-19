using System;
using System.Net;
using System.Net.Mail;
using Suteki.Common.Extensions;

namespace Suteki.Common.Services
{
    public class EmailSender : IEmailSender
    {
        readonly string smtpServer;
    	int port = 25;
        readonly string fromAddress;
        private readonly string username;
        private readonly string password;

        public int Port
    	{
			get { return port; }
			set { port = value; }
    	}

        public EmailSender(string smtpServer, string fromAddress, string username, string password)
        {
            this.smtpServer = smtpServer;
        	this.fromAddress = fromAddress;
            this.username = username;
            this.password = password;
        }

        public EmailSender(string smtpServer, string fromAddress)
        {
            this.smtpServer = smtpServer;
            this.fromAddress = fromAddress;
        }

        [Obsolete("Please use the overload with the isBodyHtml parameter. This method defaults that parameter to false")]
        public void Send(string toAddress, string subject, string body)
        {
            Send(new[] { toAddress }, subject, body, false);
        }

		[Obsolete("Please use the overload with the isBodyHtml parameter. This method defaults that parameter to false")]
		public void Send(string[] toAddress, string subject, string body)
		{
			Send(toAddress, subject, body, false);
		}

		public void Send(string toAddress, string subject, string body, bool bodyIsHtml)
		{
			Send(new[] { toAddress }, subject, body, bodyIsHtml);
		}

        public void Send(string[] toAddress, string subject, string body, bool bodyIsHtml)
        {
            // if the smtpServer is not configured, just return
            if (smtpServer == "") return;

            SendMessage(BuildMessage(subject, body, bodyIsHtml, toAddress));
        }

        public virtual void SendMessage(MailMessage message)
        {
            var smtpClient = new SmtpClient(smtpServer)
            {
                Port = port, 
                UseDefaultCredentials = false
            };

            if (HasNetworkCredentials())
            {
                smtpClient.Credentials = new NetworkCredential(username, password);
            }

            smtpClient.Send(message);
        }

        public virtual MailMessage BuildMessage(string subject, string body, bool bodyIsHtml, string[] toAddress)
        {
            var message = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = subject,
                Body = body,
                IsBodyHtml = bodyIsHtml,
            };

            toAddress.ForEach(a => message.To.Add(a));
            return message;
        }

        public bool HasNetworkCredentials()
        {
            return (!string.IsNullOrEmpty(username)) && (!string.IsNullOrEmpty(password));
        }
    }
}