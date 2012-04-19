using NUnit.Framework;
using Suteki.Common.Services;

namespace Suteki.Common.Tests.Services
{
    [TestFixture]
    public class EmailSenderTests
    {
        private EmailSender emailSender;

        private const string smtpAddress = "smtp.sutekishop.co.uk";
        private const string fromAddress = "info@sutekishop.co.uk";
        private const string username = "admin";
        private const string password = "adm1n";

        [SetUp]
        public void SetUp()
        {
            emailSender = new EmailSender(smtpAddress, fromAddress, username, password);
        }

        [Test]
        public void HasNetworkCredentials_Should_return_true()
        {
            Assert.That(emailSender.HasNetworkCredentials(), Is.True);
        }

        [Test]
        public void BuildMessage_Should_return_correct_MailMessage()
        {
            var subject = "The subject";
            var body = "The body";
            var isHtml = true;
            var toAddresses = new[]
            {
                "mike@sutekishop.co.uk",
                "info@sutekishop.co.uk"
            };

            var message = emailSender.BuildMessage(subject, body, isHtml, toAddresses);

            Assert.That(message.Subject, Is.EqualTo(subject));
            Assert.That(message.Body, Is.EqualTo(body));
            Assert.That(message.IsBodyHtml, Is.True);
            Assert.That(message.To.Count, Is.EqualTo(2));
            Assert.That(message.To[0].Address, Is.EqualTo(toAddresses[0]));
            Assert.That(message.To[1].Address, Is.EqualTo(toAddresses[1]));
        }
    }
}