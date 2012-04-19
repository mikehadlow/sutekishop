namespace Suteki.Shop.Services
{
    public interface IEncryptionService
    {
        string Decrypt(string encryptedString);
        string Encrypt(string stringToEncrypt);

        void EncryptCard(Card card);
        void DecryptCard(Card card);

        string PrivateKey { get; set; }
    }
}
