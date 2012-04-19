namespace Suteki.Shop.Services
{
    public interface IBasketService
    {
        Basket GetCurrentBasketForCurrentUser();
        Basket CreateNewBasketForCurrentUser();
    }
}