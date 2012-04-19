namespace Suteki.Shop.Services
{
    public interface ISizeService
    {
        ISizeService WithValues(System.Collections.Specialized.NameValueCollection form);
        void Update(Product product);
    }
}
