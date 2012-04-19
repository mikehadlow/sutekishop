using System;

namespace Suteki.Common.Binders
{
	public interface IAcceptsAttribute
	{
		void Accept(Attribute attribute);
	}
}