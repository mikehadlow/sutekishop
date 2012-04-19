using System;
using System.Web.Mvc;
using Suteki.Common.Extensions;
using Suteki.Common.Models;
using Suteki.Common.Repositories;

namespace Suteki.Common.Binders
{
    public class EntityBindAttribute : BindUsingAttribute
    {
        public EntityBindAttribute()
            : this(typeof(EntityModelBinder))
        {
        }

        protected EntityBindAttribute(Type binderType)
            : base(binderType)
        {
            // by default we always fetch any model that implements IEntity
            Fetch = true;
        }

        public bool Fetch { get; set; }
    }

    public class EntityModelBinder : DefaultModelBinder, IAcceptsAttribute
    {
        readonly IRepositoryResolver repositoryResolver;
        EntityBindAttribute declaringAttribute;

        public EntityModelBinder(IRepositoryResolver repositoryResolver)
        {
            this.repositoryResolver = repositoryResolver;
        }

        protected override object CreateModel(
            ControllerContext controllerContext, 
            ModelBindingContext bindingContext, 
            Type modelType)
        {
            if (modelType.IsEntity() && FetchFromRepository)
            {
                var id = GetIdFromValueProvider(bindingContext, modelType);
                if (id.HasValue && id.Value != 0)
                {
                    var repository = repositoryResolver.GetRepository(modelType);
                    object entity;
                    try
                    {
                        entity = repository.GetById(id.Value);
                    }
                    finally
                    {
                        repositoryResolver.Release(repository);
                    }
                    return entity;
                }
            }

            // Fall back to default model creation if the target is not an existing entity
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }

        protected override object GetPropertyValue(
            ControllerContext controllerContext, 
            ModelBindingContext bindingContext, 
            System.ComponentModel.PropertyDescriptor propertyDescriptor, 
            IModelBinder propertyBinder)
        {
            // any child entity property which has been changed, needs to be retrieved by calling CreateModel, 
            // we can force BindComplexModel (in DefaultModelBinder) do this by
            // setting the bindingContext.ModelMetadata.Model to null
            var entity = bindingContext.ModelMetadata.Model as IEntity;
            if (entity != null)
            {
                var id = GetIdFromValueProvider(bindingContext, bindingContext.ModelType);
                if (id.HasValue && id.Value != entity.Id)
                {
                    bindingContext.ModelMetadata.Model = null;
                }
            }
            return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }


        private static int? GetIdFromValueProvider(ModelBindingContext bindingContext, Type modelType)
        {
            var fullPropertyKey = CreateSubPropertyName(bindingContext.ModelName, modelType.GetPrimaryKey().Name);
            if (!bindingContext.ValueProvider.ContainsPrefix(fullPropertyKey))
            {
                return null;
            }

            var result = bindingContext.ValueProvider.GetValue(fullPropertyKey);
            if (result == null) return null;
            var idAsObject = result.ConvertTo(typeof (Int32));
            if (idAsObject == null) return null;
            return (int) idAsObject;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            bindingContext.ModelMetadata.ConvertEmptyStringToNull = false;
            var model = base.BindModel(controllerContext, bindingContext);
            ValidateEntity(bindingContext, controllerContext, model);
            return model;
        }

        protected virtual void ValidateEntity(
            ModelBindingContext bindingContext, 
            ControllerContext controllerContext, 
            object entity)
        {
            // override to provide additional validation.
        }

        private bool FetchFromRepository
        {
            get
            {
                // by default we always fetch any model that implements IEntity
                return declaringAttribute == null ? true : declaringAttribute.Fetch;
            }
        }

        public virtual void Accept(Attribute attribute)
        {
            declaringAttribute = (EntityBindAttribute)attribute;	
        }

        // For unit tests
        public void SetModelBinderDictionary(ModelBinderDictionary modelBinderDictionary)
        {
            Binders = modelBinderDictionary;
        }
    }
}