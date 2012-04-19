using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;

namespace Suteki.Common.Tests.Spikes
{
    public class WindsorBugs
    {
        public void AllInterfaces_not_working_with_generic_interfaces()
        {
            var container = new WindsorContainer().Register(
                AllTypes.FromThisAssembly().BasedOn(typeof(IThing<>)).WithService.AllInterfaces()
                );

            // Throws Object reference not set to an instance of an object.
            var things = container.ResolveAll<IThing<B>>();

            Assert.That(things.Length == 1);
        }

        public class A{}
        public class B{}

        public interface IThing<T>{}
        public class Thing : IThing<A>, IThing<B> {}
    }
}