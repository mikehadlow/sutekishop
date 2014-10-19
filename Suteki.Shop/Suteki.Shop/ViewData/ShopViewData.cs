using System;
using System.Collections.Generic;
using System.Linq;
using MvcContrib.Pagination;
using Suteki.Common.Extensions;
using Suteki.Common.ViewData;

namespace Suteki.Shop.ViewData
{
    public class ShopViewData : ViewDataBase
    {
        public Category Category { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public CategoryViewData CategoryViewData { get; set; }

        public Product Product { get; set; }
        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Role> Roles { get; set; }

        public User User { get; set; }
        public IEnumerable<User> Users { get; set; }

        public Basket Basket { get; set; }

        public Order Order { get; set; }
        public PagedList<Order> Orders { get; set; }
        public OrderSearchCriteria OrderSearchCriteria { get; set; }

        public IEnumerable<Country> Countries { get; set; }
        public Country Country { get; set; }

        public IEnumerable<CardType> CardTypes { get; set; }

        public Postage Postage { get; set; }
        public IEnumerable<Postage> Postages { get; set; }

        public PostageResult PostageResult { get; set; }

        public Card Card { get; set; }

        public IEnumerable<StockItem> StockItems { get; set; }

        public IEnumerable<Content> Contents { get; set; }

		public IEnumerable<OrderStatus> OrderStatuses { get; set; }

    	public MailingListSubscription MailingListSubscription { get; set; }

		public IPagination<MailingListSubscription> MailingListSubscriptions { get; set; }

        public IEnumerable<ProductCategory> ProductCategories { get; set; }

        public bool IsPrint { get; set; }

		public bool HasProducts
		{
			get { return Products != null && Products.Count() != 0; }
		}

    	// attempt at a fluent interface

        public ShopViewData WithCategory(Category category)
        {
            this.Category = category;
            return this;
        }

        public ShopViewData WithCategories(IEnumerable<Category> categories)
        {
            this.Categories = categories;
            return this;
        }

        public ShopViewData WithCategoryViewData(CategoryViewData category)
        {
            this.CategoryViewData = category;
            return this;
        }

        public ShopViewData WithProduct(Product product)
        {
            this.Product = product;
            return this;
        }

        public ShopViewData WithProducts(IEnumerable<Product> products)
        {
            this.Products = products;
            return this;
        }

        public ShopViewData WithRoles(IEnumerable<Role> roles)
        {
            this.Roles = roles;
            return this;
        }

        public ShopViewData WithUser(User user)
        {
            this.User = user;
            return this;
        }

        public ShopViewData WithUsers(IEnumerable<User> users)
        {
            this.Users = users;
            return this;
        }

        public ShopViewData WithBasket(Basket basket)
        {
            this.Basket = basket;
            return this;
        }

        public ShopViewData WithOrders(PagedList<Order> orders)
        {
            this.Orders = orders;
            return this;
        }

        public ShopViewData WithOrder(Order order)
        {
            this.Order = order;
            return this;
        }

        public ShopViewData WithOrderSearchCriteria(OrderSearchCriteria orderSearchCriteria)
        {
            this.OrderSearchCriteria = orderSearchCriteria;
            return this;
        }

        public ShopViewData WithCountries(IEnumerable<Country> countries)
        {
            this.Countries = countries;
            return this;
        }

        public ShopViewData WithCountry(Country country)
        {
            this.Country = country;
            return this;
        }

        public ShopViewData WithCardTypes(IEnumerable<CardType> cardTypes)
        {
            this.CardTypes = cardTypes;
            return this;
        }

        public ShopViewData WithPostage(Postage postage)
        {
            this.Postage = postage;
            return this;
        }

        public ShopViewData WithPostages(IEnumerable<Postage> postages)
        {
            this.Postages = postages;
            return this;
        }

        public ShopViewData WithTotalPostage(PostageResult postageResult)
        {
            this.PostageResult = postageResult;
            return this;
        }

        public ShopViewData WithCard(Card card)
        {
            this.Card = card;
            return this;
        }

        public ShopViewData WithStockItems(IEnumerable<StockItem> stockItems)
        {
            this.StockItems = stockItems;
            return this;
        }

        public ShopViewData WithContents(IEnumerable<Content> contents)
        {
            this.Contents = contents;
            return this;
        }

		public ShopViewData WithOrderStatuses(IEnumerable<OrderStatus> orderStatuses)
		{
			this.OrderStatuses = orderStatuses;
			return this;
		}

		public ShopViewData WithSubscription(MailingListSubscription subscription)
		{
			this.MailingListSubscription = subscription;
			return this;
		}

    	public ShopViewData WithSubscriptions(IPagination<MailingListSubscription> subscriptions)
    	{
			this.MailingListSubscriptions = subscriptions;
			return this;
    	}

        public ShopViewData WithProductCategories(IEnumerable<ProductCategory> productCategories)
        {
            this.ProductCategories = productCategories;
            return this;
        }

        public ShopViewData WithProductCategory(ProductCategory productCategory)
        {
            this.ProductCategory = productCategory;
            return this;
        }

        public ProductCategory ProductCategory { get; set; }
    }

    /// <summary>
    /// So you can write 
    /// ShopView.Data.WithProducts(myProducts);
    /// </summary>
    public class ShopView
    {
        public static ShopViewData Data { get { return new ShopViewData(); } }
    }
}
