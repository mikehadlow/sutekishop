using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Binders;
using Suteki.Common.Models;
using Suteki.Common.Repositories;

namespace Suteki.Common.Tests.Binders
{
    [TestFixture]
    public class EntityModelBinderTests
    {
        IRepositoryResolver repositoryResolver;
        IRepository parentRepository;
        IRepository childRepository;

        EntityModelBinder entityModelBinder;
        ControllerContext controllerContext;
        HttpRequestBase request;

        Parent parent;

        [SetUp]
        public void SetUp()
        {
            parent = new Parent
            {
                Id = 10,
                Name = "Parent from repository",
                Child = new Child
                {
                    Id = 3,
                    Name = "Child of Parent from repository"
                }
            };

            parentRepository = new FakeRepository(id => parent);
            childRepository = new FakeRepository(id => parent.Child);

            repositoryResolver = MockRepository.GenerateStub<IRepositoryResolver>();
            repositoryResolver.Stub(r => r.GetRepository(typeof (Parent))).Return(parentRepository);
            repositoryResolver.Stub(r => r.GetRepository(typeof (Child))).Return(childRepository);

            entityModelBinder = new EntityModelBinder(repositoryResolver);
            controllerContext = new ControllerContext
            {
                HttpContext = MockRepository.GenerateStub<HttpContextBase>()
            };
            request = MockRepository.GenerateStub<HttpRequestBase>();
            controllerContext.HttpContext.Stub(x => x.Request).Return(request);

            entityModelBinder.SetModelBinderDictionary(new ModelBinderDictionary { DefaultBinder = entityModelBinder });
        }

        [Test]
        public void Should_get_child_from_repository()
        {
            var values = new NameValueCollection
            {
                { "Id", "10" },
                { "Name", "The Parent" },
                { "Child.Id", "3" },
                { "Child.Name", "The Child" }
            };

            var bindingContext = new ModelBindingContext
            {
                ModelMetadata = GetModelMetadata(null),
                ValueProvider = new NameValueCollectionValueProvider(values, CultureInfo.GetCultureInfo("EN-GB"))
            };

            entityModelBinder.Accept(new EntityBindAttribute { Fetch = false });
            var boundParent = entityModelBinder.BindModel(controllerContext, bindingContext) as Parent;

            boundParent.ShouldNotBeNull("Parent is null");
            boundParent.Id.ShouldEqual(10);
            boundParent.Name.ShouldEqual("The Parent");
            boundParent.ShouldBeNotBeTheSameAs(parent);

            boundParent.Child.Id.ShouldEqual(3);
            boundParent.Child.ShouldBeTheSameAs(boundParent.Child);

            // expect the Child.Name to have been updated, even though the actual child entity is sourced
            // from the repository
            boundParent.Child.Name.ShouldEqual("The Child");
            
            PrintErrors(bindingContext.ModelState);
        }

        [Test]
        public void Should_not_get_child_from_repository_if_id_is_not_given()
        {
            var values = new NameValueCollection
            {
                { "Id", "10" },
                { "Name", "The Parent" },
                { "Child.Name", "The Child" }
            };

            var bindingContext = new ModelBindingContext
            {
                ModelMetadata = GetModelMetadata(null),
                ValueProvider = new NameValueCollectionValueProvider(values, CultureInfo.GetCultureInfo("EN-GB"))
            };

            entityModelBinder.Accept(new EntityBindAttribute { Fetch = false });
            var boundParent = entityModelBinder.BindModel(controllerContext, bindingContext) as Parent;

            boundParent.ShouldNotBeNull("Parent is null");
            boundParent.Id.ShouldEqual(10);
            boundParent.Name.ShouldEqual("The Parent");
            boundParent.ShouldBeNotBeTheSameAs(parent);

            boundParent.Child.Id.ShouldEqual(0);
            boundParent.Child.Name.ShouldEqual("The Child");
            boundParent.Child.ShouldBeNotBeTheSameAs(parent.Child);

            PrintErrors(bindingContext.ModelState);
        }

        [Test]
        public void Should_get_parent_from_repository_if_fetch_is_true()
        {
            var values = new NameValueCollection
            {
                { "Id", "10" },
                { "Child.Name", "Updated child name" }
            };

            var bindingContext = new ModelBindingContext
            {
                ModelMetadata = GetModelMetadata(null),
                ValueProvider = new NameValueCollectionValueProvider(values, CultureInfo.GetCultureInfo("EN-GB"))
            };

            entityModelBinder.Accept(new EntityBindAttribute { Fetch = true });
            var boundParent = entityModelBinder.BindModel(controllerContext, bindingContext) as Parent;

            boundParent.ShouldNotBeNull("Parent is null");
            boundParent.Id.ShouldEqual(10);
            boundParent.Name.ShouldEqual("Parent from repository");
            boundParent.ShouldBeTheSameAs(parent);

            boundParent.Child.Id.ShouldEqual(3);
            boundParent.Child.Name.ShouldEqual("Updated child name");
            boundParent.Child.ShouldBeTheSameAs(parent.Child);

            PrintErrors(bindingContext.ModelState);
        }

        [Test]
        public void Should_get_child_if_child_id_is_missing()
        {
            var values = new NameValueCollection
            {
                { "Id", "10" }
            };

            var bindingContext = new ModelBindingContext
            {
                ModelMetadata = GetModelMetadata(null),
                ValueProvider = new NameValueCollectionValueProvider(values, CultureInfo.GetCultureInfo("EN-GB"))
            };

            entityModelBinder.Accept(new EntityBindAttribute { Fetch = true });
            var boundParent = entityModelBinder.BindModel(controllerContext, bindingContext) as Parent;

            boundParent.ShouldNotBeNull("Parent is null");
            boundParent.Id.ShouldEqual(10);
            boundParent.Name.ShouldEqual("Parent from repository");
            boundParent.ShouldBeTheSameAs(parent);

            boundParent.Child.Id.ShouldEqual(3);
            boundParent.Child.ShouldBeTheSameAs(parent.Child);
        }

        [Test]
        public void Should_not_get_parent_from_repository_if_no_id_is_given()
        {
            var values = new NameValueCollection
            {
                { "Name", "Parent Name" },
                { "Child.Name", "Child Name" }
            };

            var bindingContext = new ModelBindingContext
            {
                ModelMetadata = GetModelMetadata(null),
                ValueProvider = new NameValueCollectionValueProvider(values, CultureInfo.GetCultureInfo("EN-GB"))
            };

            entityModelBinder.Accept(new EntityBindAttribute { Fetch = true }); // but id not set
            var boundParent = entityModelBinder.BindModel(controllerContext, bindingContext) as Parent;

            boundParent.ShouldNotBeNull("Parent is null");
            boundParent.Id.ShouldEqual(0);
            boundParent.Name.ShouldEqual("Parent Name");
            boundParent.ShouldBeNotBeTheSameAs(parent);

            boundParent.Child.Id.ShouldEqual(0);
            boundParent.Child.Name.ShouldEqual("Child Name");
            boundParent.Child.ShouldBeNotBeTheSameAs(parent.Child);

            PrintErrors(bindingContext.ModelState);
        }

        private static ModelMetadata GetModelMetadata(Func<object> modelAccessor)
        {
            return new ModelMetadata(
                new DataAnnotationsModelMetadataProvider(),
                null,
                modelAccessor,
                typeof(Parent),
                null);
        }

        static void PrintErrors(IDictionary<string, ModelState> modelState)
        {
            foreach (var key in modelState.Keys)
            {
                foreach (var error in modelState[key].Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                    Assert.Fail("Model binding errors occured");
                }
            }
        }

        private class Parent : IEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Child Child { get; set; }
        }

        private class Child : IEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}