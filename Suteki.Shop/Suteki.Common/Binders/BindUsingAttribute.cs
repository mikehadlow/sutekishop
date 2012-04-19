using System;
using System.Web.Mvc;
using Suteki.Common.Extensions;
using Suteki.Common.Windsor;

namespace Suteki.Common.Binders
{
	public class BindUsingAttribute : CustomModelBinderAttribute
	{
		private readonly Type binderType;

		public BindUsingAttribute(Type binderType)
		{
			if(!typeof(IModelBinder).IsAssignableFrom(binderType))
			{
				throw new InvalidOperationException("Type '{0}' does not implement IModelBinder.".With(binderType.Name));
			}

			this.binderType = binderType;
		}

		public override IModelBinder GetBinder()
		{
		    var binder = (IModelBinder) IocContainer.Resolve(binderType);

			if(binder is IAcceptsAttribute)
			{
				((IAcceptsAttribute)binder).Accept(this);
			}

			return binder;
		}
	}
}