using NUnit.Framework;

namespace Suteki.Shop.Tests.Models
{
    [TestFixture]
    public class TextContentTests
    {
        [Test]
        public void UrlName_ShouldReturnAnyNonNameCharactersAsUnderscores()
        {
            const string name = "That's how (he &) I like £$$$";
            const string expectedName = "That_s_how__he____I_like_____";

            var textContent = new TextContent
            {
                Name = name
            };

            var urlName = textContent.UrlName;

            Assert.That(urlName, Is.EqualTo(expectedName));
        }
    }
}
