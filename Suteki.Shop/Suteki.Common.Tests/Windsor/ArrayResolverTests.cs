using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using NUnit.Framework;
using Castle.MicroKernel;
using Castle.MicroKernel.Handlers;
using Suteki.Common.Windsor;

namespace Suteki.Common.Tests.Windsor
{
    [TestFixture]
    public class ArrayResolverTests
    {
        [Test, ExpectedException(typeof(HandlerException))]
        public void DoesNotResolveArraysByDefault()
        {
            var kernel = new DefaultKernel();

            kernel.Register(
                Component.For<Thing>(),
                Component.For<ISubThing>().ImplementedBy<First>(),
                Component.For<ISubThing>().ImplementedBy<Second>(),
                Component.For<ISubThing>().ImplementedBy<Third>()
                );

            var thing = kernel.Resolve<Thing>();
        }
        
        [Test]
        public void ShouldResolveArrayOfDependencies()
        {
            var kernel = new DefaultKernel();
            kernel.Resolver.AddSubResolver(new ArrayResolver(kernel));

            kernel.Register(
                Component.For<Thing>(),
                Component.For<ISubThing>().ImplementedBy<First>(),
                Component.For<ISubThing>().ImplementedBy<Second>(),
                Component.For<ISubThing>().ImplementedBy<Third>()
                );

            var thing = kernel.Resolve<Thing>();

            Assert.That(thing.SubThings.Count, Is.EqualTo(3));
            Assert.That(thing.SubThings[0], Is.InstanceOf(typeof(First)));
            Assert.That(thing.SubThings[1], Is.InstanceOf(typeof(Second)));
            Assert.That(thing.SubThings[2], Is.InstanceOf(typeof(Third)));
        }

        [Test]
        public void DoesNotDiscoverCircularDependencies()
        {
            var kernel = new DefaultKernel();
            kernel.Resolver.AddSubResolver(new ArrayResolver(kernel));

            // a circular reference exception should be thrown here
            kernel.Register(
                Component.For<Thing>(),
                Component.For<ISubThing>().ImplementedBy<Circular>()
                );

            // this crashes the test framework!
            // var thing = kernel.Resolve<Thing>();
        }

        public class Thing
        {
            readonly List<ISubThing> subThings = new List<ISubThing>();

            public Thing(params ISubThing[] subThings)
            {
                this.subThings.AddRange(subThings);
            }

            public List<ISubThing> SubThings
            {
                get { return subThings; }
            }
        }

        public interface ISubThing {}

        public class First : ISubThing {}
        public class Second : ISubThing {}
        public class Third : ISubThing {}

        public class Circular : ISubThing
        {
            public Thing Thing { get; private set; }

            public Circular(Thing thing)
            {
                this.Thing = thing;
            }
        }
    }
}