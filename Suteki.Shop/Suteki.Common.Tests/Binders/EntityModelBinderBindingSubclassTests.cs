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
    public class EntityModelBinderBindingSubclassTests
    {
        EntityModelBinder entityModelBinder;
        const int myBaseId = 89;
        MySubclass mySubclass;

        ControllerContext controllerContext;
        HttpRequestBase request;

        [SetUp]
        public void SetUp()
        {
            mySubclass = new MySubclass {Id = myBaseId, Name = "the name"};

            var repository = MockRepository.GenerateStub<IRepository>();
            var repositoryResolver = MockRepository.GenerateStub<IRepositoryResolver>();

            repositoryResolver.Stub(r => r.GetRepository(typeof (MyBase))).Return(repository);
            repositoryResolver.Stub(r => r.GetRepository(typeof (MySubclass))).Return(repository);
            repository.Stub(r => r.GetById(myBaseId)).Return(mySubclass);
            
            entityModelBinder = new EntityModelBinder(repositoryResolver);

            controllerContext = new ControllerContext
            {
                HttpContext = MockRepository.GenerateStub<HttpContextBase>()
            };
            request = MockRepository.GenerateStub<HttpRequestBase>();
            controllerContext.HttpContext.Stub(x => x.Request).Return(request);
        }

        [Test]
        public void Binder_should_bind_subclass_name_correctly()
        {
            var values = new NameValueCollection
            {
                { "Id", myBaseId.ToString() },
                { "Name", "The New Name" }
            };

            var bindingContext = new ModelBindingContext
            {
                // bind only works if the subclass type is specified in the model metadata
                ModelMetadata = GetModelMetadata(null, typeof(MySubclass)),
                ValueProvider = new NameValueCollectionValueProvider(values, CultureInfo.GetCultureInfo("EN-GB"))
            };

            entityModelBinder.BindModel(controllerContext, bindingContext);

            mySubclass.Name.ShouldEqual("The New Name");
        }

        private static ModelMetadata GetModelMetadata(Func<object> modelAccessor, Type theBoundType)
        {
            return new ModelMetadata(
                new DataAnnotationsModelMetadataProvider(),
                null,
                modelAccessor,
                theBoundType,
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
    }

    public class MyBase : IEntity
    {
        public int Id { get; set; }
    }

    public class MySubclass : MyBase
    {
        public string Name { get; set; }
    }
}