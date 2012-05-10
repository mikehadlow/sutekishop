using System.Collections.Generic;
using System.Linq;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using NUnit.Framework;

namespace Suteki.Shop.Tests.Repositories
{
    public static class MockRepositoryBuilder
    {
		public static IRepository<Content> CreateContentRepository()
		{
			var repository = MockRepository.GenerateStub<IRepository<Content>>();

			var content = new[]
			{
				new Content { Name = "Page 1"},
				new Content { Name = "Page 2"}
			};

			repository.Expect(x => x.GetAll()).Return(content.AsQueryable());
			return repository;
		}

        public static IRepository<User> CreateUserRepository()
        {
            var userRepositoryMock = MockRepository.GenerateStub<IRepository<User>>();
            var users = new List<User>
            {
                new User { Id = 1, Email = "Henry@suteki.co.uk", Role = Role.Administrator,
                    Password = "6C80B78681161C8349552872CFA0739CF823E87B", IsEnabled = true }, // henry1

                new User { Id = 2, Email = "George@suteki.co.uk", Role = Role.Administrator, 
                    Password = "DC25F9DC0DF2BE9E6A83E6F0B26F4B41F57ADF6D", IsEnabled = true }, // george1

                new User { Id = 3, Email = "Sky@suteki.co.uk",  Role = Role.Administrator,
                    Password = "980BC222DA7FDD0D37BE816D60084894124509A1", IsEnabled = true } // sky1
            };

            userRepositoryMock.Expect(ur => ur.GetAll()).Return(users.AsQueryable());

            return userRepositoryMock;
        }

        public static IRepository<Role> CreateRoleRepository()
        {
            var roleRepositoryMock = MockRepository.GenerateStub<IRepository<Role>>();

            var roles = new List<Role>
            {
                new Role { Id = 1, Name = "Administrator" },
                new Role { Id = 2, Name = "Order Processor" },
                new Role { Id = 3, Name = "Customer" },
                new Role { Id = 4, Name = "Guest" }
            };

            roleRepositoryMock.Expect(r => r.GetAll()).Return(roles.AsQueryable());

            return roleRepositoryMock;
        }

        public static IRepository<Category> CreateCategoryRepository()
        {
            var categoryRepositoryMock = MockRepository.GenerateStub<IRepository<Category>>();

            var root = new Category { Id = 1, Parent = null, Name = "root", Image = new Image { Id = 5} };

            var one = new Category { Id = 2, Parent = root, Name = "one" };
            var two = new Category { Id = 3, Parent = root, Name = "two" };
            root.Categories.Add(one);
            root.Categories.Add(two);

            var oneOne = new Category { Id = 4, Parent = one, Name = "oneOne" };
            var oneTwo = new Category { Id = 5, Parent = one, Name = "oneTwo" };
            one.Categories.Add(oneOne);
            one.Categories.Add(oneTwo);

            var oneTwoOne = new Category { Id = 6, Parent = oneTwo, Name = "oneTwoOne" };
            var oneTwoTwo = new Category { Id = 7, Parent = oneTwo, Name = "oneTwoTwo" };
            oneTwo.Categories.Add(oneTwoOne);
            oneTwo.Categories.Add(oneTwoTwo);

            Category[] categories = 
            {
                root,
                one,
                two,
                oneOne,
                oneTwo,
                oneTwoOne,
                oneTwoTwo
            };

            categoryRepositoryMock.Expect(c => c.GetById(1)).Return(root);
            categoryRepositoryMock.Expect(c => c.GetAll()).Return(categories.AsQueryable());

            return categoryRepositoryMock;
        }

        /// <summary>
        /// Asserts that the graph created by CreateCategoryRepository is correct
        /// </summary>
        /// <param name="root"></param>
        public static void AssertCategoryGraphIsCorrect(Category root)
        {
            Assert.IsNotNull(root, "root category is null");

            Assert.IsNotNull(root.Categories[0], "first child category is null");
            Assert.AreEqual("one", root.Categories[0].Name);

            Assert.IsNotNull(root.Categories[0].Categories[1], "second grandchild category is null");
            Assert.AreEqual("oneTwo", root.Categories[0].Categories[1].Name);

            Assert.IsNotNull(root.Categories[0].Categories[1].Categories[0], "first great grandchild category is null");
            Assert.AreEqual("oneTwoOne", root.Categories[0].Categories[1].Categories[0].Name);

            Assert.IsNotNull(root.Categories[0].Categories[1].Categories[1], "second great grandchild category is null");
            Assert.AreEqual("oneTwoTwo", root.Categories[0].Categories[1].Categories[1].Name);
        }

        public static IRepository<Product> CreateProductRepository()
        {
            var productRepositoryMock = MockRepository.GenerateStub<IRepository<Product>>();

            List<Product> products = GetProducts();

            productRepositoryMock.Expect(pr => pr.GetAll()).Return(products.AsQueryable());

            return productRepositoryMock;
        }

        static List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Description = "Description 1", ProductCategories = { new ProductCategory { Category = new Category { Id = 2 } } }},
                new Product { Id = 2, Name = "Product 2", Description = "Description 2", ProductCategories = { new ProductCategory { Category = new Category { Id = 2 } } } },
                new Product { Id = 3, Name = "Product 3", Description = "Description 3", ProductCategories = { new ProductCategory { Category = new Category { Id = 4 } } }},
                new Product { Id = 4, Name = "Product 4", Description = "<p>\"Description 4\"</p>", ProductCategories = { new ProductCategory { Category = new Category { Id = 4 } } }},
                new Product { Id = 5, Name = "Product 5", Description = "Description 5", ProductCategories = { new ProductCategory { Category = new Category { Id = 6 } } } },
                new Product { Id = 6, IsActive = false, Name = "Product 6", Description = "Description 6", ProductCategories = { new ProductCategory { Category = new Category { Id = 6 } } } },
            };
        }

        public static IRepository<Review> CreateReviewRepository()
    	{
            var products = GetProducts();
			var reviews = new List<Review> 
			{
				new Review { Id = 1, Product = products.Single(p => p.Id == 1), Approved = true, Rating = 5, Text = "foo"},
				new Review { Id = 2, Product = products.Single(p => p.Id == 1), Approved = true, Rating = 4, Text = "bar"},
				new Review { Id = 2, Product = products.Single(p => p.Id == 1), Approved = false, Rating = 3, Text = "bar"},
				new Review { Id = 3, Product = products.Single(p => p.Id == 2), Approved = true, Rating = 4, Text = "baz"},
				new Review { Id = 4, Product = products.Single(p => p.Id == 3), Approved = false, Rating = 2, Text = "blah"},
			};

			var repository = MockRepository.GenerateStub<IRepository<Review>>();
			repository.Expect(x => x.GetAll()).Return(reviews.AsQueryable());
			return repository;
    	}
    }
}
