using System;
using NUnit.Framework;
using Suteki.Common.Validation;
using Suteki.Shop.Services;
using System.Security.Cryptography;

namespace Suteki.Shop.Tests.Services
{
    [TestFixture]
    public class EncryptionServiceTests
    {
        EncryptionService encryptionService;

        const string publicKey = @"BgIAAACkAABSU0ExAAQAAAEAAQC/CAaD64BWCkRSfm2RlGclcnZ2k2GMGDjoCgTwwdzqZ7dYx+gd3CeCOuvsh3KwB7OKyBiZjHnIk2au9iZ9RGxqYd1Iw741t4e3Yad0hEyLeXyrl6Xs/EfshwGRLD81djLiQvjZkSDZYNUpGpwH7jDnbdcVo6HfwyHXPaM5IC561g==";
        const string privateKey = @"BwIAAACkAABSU0EyAAQAAAEAAQC/CAaD64BWCkRSfm2RlGclcnZ2k2GMGDjoCgTwwdzqZ7dYx+gd3CeCOuvsh3KwB7OKyBiZjHnIk2au9iZ9RGxqYd1Iw741t4e3Yad0hEyLeXyrl6Xs/EfshwGRLD81djLiQvjZkSDZYNUpGpwH7jDnbdcVo6HfwyHXPaM5IC561hGrXGWVPLkQj59/CSGniIIUB6PUQx9gIfr5pdU/mC7h2VLsQCAGH5TH7tQ89gSiXpaGpBX7XvJyVh5stmysAfjPVqBxZEAg9jn0c7lo4tZs3V+hUIkZH8xP+4ANe6XtgncWp0+PRz9TY1cQxI6L41GzdEKeIJCGqmCuY9ty2mPdISd93SXpfgXgFDx9iccooe8nWtM83JREnMuBBBAld7PoSxEy8IzS3GVhIsvcCc9iKdgaGCFRZkBQKxyIkJ4BQME4qlcXBx9Wt3QZwm55tcT8SFrNznc4e1vPmMQ19pZJhI8SuLIo6sTe0KEFK0es5upABwGdqcHWo70uZXRJrB9MkwkHRuRGlkSEcFbP0xiIEg5FHZn3cumqpEb1TMf5iFaK+rknKRrl1DBpUrGPb8uFlyycbIYELTtigIVqF9XPgaXHM5mToIAYI0TFQxaylb6bsVWz8xKL0DynAn7HBZSAkYtNeeeCDU/ED0arUH+kPMMsTBqjQSxXrinRTvAOwcpftKk8flC4l9cEKX5S8b/6VgBK/wD6LHnOTdXtTlDgRmxCOFgpoU8kMv8HeuyrW2uVdSbGXi/OoiKRASMpaS0=";

        const string creditCardNumber = "1111111111111117";
        const string securityCode = "345";

        [SetUp]
        public void SetUp()
        {
            encryptionService = new EncryptionService(publicKey, privateKey);
        }

        [Test]
        public void Encrypt_ShouldEncryptACreditCardNumber()
        {
            string encryptedCardNumber = encryptionService.Encrypt(creditCardNumber);
            string decryptedCardNumber = encryptionService.Decrypt(encryptedCardNumber);

            Assert.That(decryptedCardNumber, Is.EqualTo(creditCardNumber));
        }

        [Test]
        public void EncryptCard_ShouldEncryptACreditCard()
        {
            Card card = new Card
            {
                Number = creditCardNumber,
                SecurityCode = securityCode
            };

            encryptionService.EncryptCard(card);

            Assert.That(card.Number, Is.Not.EqualTo(creditCardNumber));
            Assert.That(card.SecurityCode, Is.Not.EqualTo(securityCode));

            encryptionService.DecryptCard(card);

            Assert.That(card.Number, Is.EqualTo(creditCardNumber));
            Assert.That(card.SecurityCode, Is.EqualTo(securityCode));
        }

        /// <summary>
        /// Use this test to generate the public and private keys as encoded XML
        /// </summary>
        [Test, Explicit]
        public void GenerateKeys()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            string privateKey = Convert.ToBase64String(rsa.ExportCspBlob(true));
            Console.WriteLine("\nPrivateKey:\n{0}", privateKey);

            string publicKey = Convert.ToBase64String(rsa.ExportCspBlob(false));
            Console.WriteLine("\nPublicKey:\n{0}", publicKey);
        }

        public bool ThereIsNoXOR()
        {
            return false ^ false;
        }

        [Test, ExpectedException(typeof(ValidationException))]
        public void Decrypt_ShouldThrowValidationExceptionOnInvalidPrivateKey()
        {
            var encryptedCard = encryptionService.Encrypt(creditCardNumber);
            var bytes = Convert.FromBase64String(privateKey);
            bytes[10] = 0;
            var invalidKey = Convert.ToBase64String(bytes);
            encryptionService.PrivateKey = invalidKey;
            encryptionService.Decrypt(encryptedCard);
        }
    }
}
