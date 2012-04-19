using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;

namespace Suteki.Common.Windsor
{
    /// <summary>
    /// ArrayResolver from:
    /// http://hammett.castleproject.org/?p=257
    /// </summary>
    public class ArrayResolver : ISubDependencyResolver
    {
        private readonly IKernel kernel;

        public ArrayResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public object Resolve(CreationContext context, ISubDependencyResolver parentResolver,
                              ComponentModel model,
                              DependencyModel dependency)
        {
            return kernel.ResolveAll(dependency.TargetType.GetElementType(), null);
        }

        public bool CanResolve(CreationContext context, ISubDependencyResolver parentResolver,
                               ComponentModel model,
                               DependencyModel dependency)
        {
            return dependency.TargetType != null &&
                   dependency.TargetType.IsArray &&
                   dependency.TargetType.GetElementType().IsInterface;
        }
    }
}