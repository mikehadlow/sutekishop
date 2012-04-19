using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;

namespace Suteki.Common.Tests.Binders
{
    [TestFixture, Explicit]
    public class DefaultModelBinderSpike
    {
        private DefaultModelBinder binder;
        ControllerContext controllerContext;
        HttpRequestBase request;

        [SetUp]
        public void SetUp()
        {
            binder = new DefaultModelBinder();
            controllerContext = new ControllerContext
            {
                HttpContext = MockRepository.GenerateStub<HttpContextBase>()
            };
            request = MockRepository.GenerateStub<HttpRequestBase>();
            controllerContext.HttpContext.Stub(x => x.Request).Return(request);
        }

        [Test]
        public void Lets_see_how_it_works()
        {
            var bindingContext = new ModelBindingContext
            {
                ModelMetadata = GetModelMetadata(null),
                ValueProvider = new FakeValueProvider()
            };

            var entity = binder.BindModel(controllerContext, bindingContext) as BindableEntity;

            PrintEntity(entity);
            PrintErrors(bindingContext.ModelState);
        }

        [Test]
        public void Provide_some_data()
        {
            var values = new NameValueCollection
            {
                { "Id", "10" },
                { "Name", "The Bound Thing" },
                { "Date", "30/1/2010" },
                { "Reference", "41b16710-1f27-4bb3-8231-f8bba148396c" },
                { "Price", "123.45" }
            };

            var bindingContext = new ModelBindingContext
            {
                ModelMetadata = GetModelMetadata(null),
                ValueProvider = new NameValueCollectionValueProvider(values, CultureInfo.GetCultureInfo("EN-GB"))
            };

            var entity = binder.BindModel(controllerContext, bindingContext) as BindableEntity;

            PrintEntity(entity);
            PrintErrors(bindingContext.ModelState);
        }

        [Test]
        public void Is_it_case_sensitive()
        {
            var values = new NameValueCollection
            {
                { "id", "10" },
                { "name", "The Bound Thing" },
                { "date", "30/1/2010" },
                { "reference", "41b16710-1f27-4bb3-8231-f8bba148396c" },
                { "price", "123.45" }
            };

            var bindingContext = new ModelBindingContext
            {
                ModelMetadata = GetModelMetadata(null),
                ValueProvider = new NameValueCollectionValueProvider(values, CultureInfo.GetCultureInfo("EN-GB"))
            };

            var entity = binder.BindModel(controllerContext, bindingContext) as BindableEntity;

            PrintEntity(entity);
            PrintErrors(bindingContext.ModelState);
        }

        [Test]
        public void Use_a_prefix()
        {
            var values = new NameValueCollection
            {
                { "foo.Id", "10" },
                { "foo.Name", "The Bound Thing" },
                { "foo.Date", "30/1/2010" },
                { "foo.Reference", "41b16710-1f27-4bb3-8231-f8bba148396c" },
                { "foo.Price", "123.45" }
            };

            var bindingContext = new ModelBindingContext
            {
                ModelName = "foo",
                ModelMetadata = GetModelMetadata(null),
                ValueProvider = new NameValueCollectionValueProvider(values, CultureInfo.GetCultureInfo("EN-GB"))
            };

            var entity = binder.BindModel(controllerContext, bindingContext) as BindableEntity;

            PrintEntity(entity);
            PrintErrors(bindingContext.ModelState);
        }

        [Test]
        public void Include_child_entity_value()
        {
            var values = new NameValueCollection
            {
                { "Id", "10" },
                { "Name", "The Bound Thing" },
                { "Date", "30/1/2010" },
                { "Reference", "41b16710-1f27-4bb3-8231-f8bba148396c" },
                { "Price", "123.45" },
                { "ChildEntity.Name", "The Child Thing"}
            };

            var bindingContext = new ModelBindingContext
            {
                ModelMetadata = GetModelMetadata(null),
                ValueProvider = new NameValueCollectionValueProvider(values, CultureInfo.GetCultureInfo("EN-GB"))
            };

            var entity = binder.BindModel(controllerContext, bindingContext) as BindableEntity;

            PrintEntity(entity);
            PrintErrors(bindingContext.ModelState);
        }

        [Test]
        public void Providing_a_model_means_it_gets_updated()
        {
            var providedEntity = new BindableEntity
            {
                Id = 4,
                Name = "The Original Name",
                Date = new DateTime(1900, 1, 1),
                Reference = Guid.NewGuid(),
                Price = 111.11M,
                ChildEntity = new ChildEntity
                {
                    Name = "The Orginal Child"
                }
            };

            var values = new NameValueCollection
            {
                { "Id", "10" },
                { "Name", "The Bound Thing" },
                { "Date", "30/1/2010" },
                { "Reference", "41b16710-1f27-4bb3-8231-f8bba148396c" },
                { "Price", "123.45" },
                { "ChildEntity.Name", "The Child Thing"}
            };

            var bindingContext = new ModelBindingContext
            {
                ModelMetadata = GetModelMetadata(() => providedEntity),
                ValueProvider = new NameValueCollectionValueProvider(values, CultureInfo.GetCultureInfo("EN-GB"))
            };

            var entity = binder.BindModel(controllerContext, bindingContext) as BindableEntity;

            PrintEntity(entity);
            PrintErrors(bindingContext.ModelState);
        }

        private static ModelMetadata GetModelMetadata(Func<object> modelAccessor)
        {
            return new ModelMetadata(
                new DataAnnotationsModelMetadataProvider(), 
                null,
                modelAccessor,
                typeof (BindableEntity), 
                null);
        }

        static void PrintErrors(IDictionary<string, ModelState> modelState)
        {
            foreach (var key in modelState.Keys)
            {
                foreach (var error in modelState[key].Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
        }

        public void PrintEntity(BindableEntity entity)
        {
            if (entity == null)
            {
                Console.WriteLine("entity is null");
            }

            Console.Out.WriteLine("entity.Id = {0}", entity.Id);
            Console.Out.WriteLine("entity.Name = {0}", entity.Name);
            Console.Out.WriteLine("entity.Date = {0}", entity.Date);
            Console.Out.WriteLine("entity.Reference = {0}", entity.Reference);
            Console.Out.WriteLine("entity.Price = {0}", entity.Price);
            Console.Out.WriteLine("entity.ChildEntity.Name = {0}", entity.ChildEntity==null ? "<null>" : entity.ChildEntity.Name);
        }
    }

    public class FakeValueProvider : IValueProvider
    {
        public bool ContainsPrefix(string prefix)
        {
            Console.WriteLine("ContainsPrefix prefix = '{0}'", prefix);
            return true;
        }

        public ValueProviderResult GetValue(string key)
        {
            Console.WriteLine("GetValue key = '{0}'", key);
            return null;
        }
    }

    public class BindableEntity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required!!")]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public Guid Reference { get; set; }
        public decimal Price { get; set; }

        [Required(ErrorMessage = "ChildEntity was not provided")]
        public ChildEntity ChildEntity { get; set; }
    }

    public class ChildEntity
    {
        public string Name { get; set; }
    }
}