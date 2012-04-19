using Suteki.Common.Events;
using Suteki.Shop.Events;
using Suteki.Shop.Services;

namespace Suteki.Shop.Handlers
{
    public class CreateNewBasketOnOrderConfirm : IHandle<OrderConfirmed>
    {
        readonly IBasketService basketService;

        public CreateNewBasketOnOrderConfirm(IBasketService basketService)
        {
            this.basketService = basketService;
        }

        public void Handle(OrderConfirmed orderConfirmed)
        {
            basketService.CreateNewBasketForCurrentUser();
        }
    }
}