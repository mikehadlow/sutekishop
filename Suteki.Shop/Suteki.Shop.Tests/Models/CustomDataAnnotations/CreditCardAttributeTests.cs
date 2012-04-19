using System.Text.RegularExpressions;
using NUnit.Framework;
using Suteki.Shop.Models.CustomDataAnnotations;

namespace Suteki.Shop.Tests.Models.CustomDataAnnotations
{
    [TestFixture]
    public class CreditCardAttributeTests 
    {
        CreditCardAttribute creditCardAttribute;

        [SetUp]
        public void SetUp()
        {
            creditCardAttribute = new CreditCardAttribute();
        }

        [Test]
        public void IsCreditCard_ShouldSuccessfullyValidateACreditCard()
        {
            const string validCardNumber = "1111111111111117";
            creditCardAttribute.IsValid(validCardNumber).ShouldBeTrue();
        }

        public void IsCreditCard_ShouldNotValidateAnInvalidCreditCard()
        {
            const string validCardNumber = "1111111111211117";
            creditCardAttribute.IsValid(validCardNumber).ShouldBeFalse();
        }

        [Test]
        public void IsCreditCard_ShouldSuccessfullyValidateACreditCardWithSpaces()
        {
            const string validCardNumber = "1111 1111 1111 1117";
            creditCardAttribute.IsValid(validCardNumber).ShouldBeTrue();
        }

        [Test]
        public void IsCreditCard_ShouldSuccessfullyValidateACreditCardWithDashes()
        {
            const string validCardNumber = "1111-1111-1111-1117";
            creditCardAttribute.IsValid(validCardNumber).ShouldBeTrue();
        }

        [Test]
        public void IsCreditCard_ShouldSuccessfullyValidatate19DigitCreditCardNumbers()
        {
            const string validCardNumber = "1111 1111 1111 1111 113";
            creditCardAttribute.IsValid(validCardNumber).ShouldBeTrue();
        }

        [Test]
        public void RegexNumberOnlySpike()
        {
            const string value = "111 222-333abc";
            const string expected = "111222333";
            var result = Regex.Replace(value, "[^0-9]", "");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void SholdNotValidateAnyOldString()
        {
            const string invalidNumber = "Lloyds TSB";
            creditCardAttribute.IsValid(invalidNumber).ShouldBeFalse();
        }
    }
}