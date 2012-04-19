// ReSharper disable InconsistentNaming
using System;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using Suteki.Common.Events;

namespace Suteki.Common.Tests.Events
{
    [TestFixture]
    public class DomainEventsTests 
    {
        [Test]
        public void Should_be_able_to_test_raising_a_domain_event()
        {
            var @event = new SomethingAwesomeHappened();
            IDomainEvent rasisedEvent = null;

            DomainEvent.RaiseAction = e => rasisedEvent = e;
            DomainEvent.Raise(@event);

            rasisedEvent.ShouldBeTheSameAs(@event);

            DomainEvent.RaiseAction = null;
        }

        [Test]
        public void Should_be_able_to_resolve_the_event_handler()
        {
            using (var container = new WindsorContainer()
                .Register(
                    AllTypes.FromAssembly(Assembly.GetExecutingAssembly())
                        .BasedOn(typeof(IHandle<>)).WithService.Base()
                        .Configure(c => c.LifeStyle.Transient)
                ))
            {
                var handlers = container.ResolveAll<IHandle<SomethingAwesomeHappened>>();

                handlers.Length.ShouldEqual(2);
                handlers[0].ShouldBe<SendEmailWhenSomethingAwesomeHappened>();
                handlers[1].ShouldBe<DoSomethingElseWhenAwesomeHappened>();
            }
        }

        [Test]
        public void Should_execute_handlers_on_event()
        {
            var messages = new System.Collections.Generic.List<string>();

            using(var container = new WindsorContainer()
                .Register(
                    Component.For<Action<string>>().Instance(messages.Add),
                    AllTypes.FromAssembly(Assembly.GetExecutingAssembly())
                        .BasedOn(typeof(IHandle<>)).WithService.Base()
                        .Configure(c => c.LifeStyle.Transient)
                ))
            {
                DomainEvent.ReturnContainer = () => container;

                var @event = new SomethingAwesomeHappened();
                DomainEvent.Raise(@event);

                DomainEvent.ReturnContainer = null;
            }

            messages.Count.ShouldEqual(2);
            messages[0].ShouldEqual("handler1");
            messages[1].ShouldEqual("handler2");
        }
    }

    public class SomethingAwesomeHappened : IDomainEvent
    {
    }

    public class SendEmailWhenSomethingAwesomeHappened : IHandle<SomethingAwesomeHappened>
    {
        public Action<string> EventTestLogger { get; set; }
        public void Handle(SomethingAwesomeHappened @event)
        {
            EventTestLogger("handler1");
        }
    }

    public class DoSomethingElseWhenAwesomeHappened : IHandle<SomethingAwesomeHappened>
    {
        public Action<string> EventTestLogger { get; set; }
        public void Handle(SomethingAwesomeHappened @event)
        {
            EventTestLogger("handler2");
        }
    }
}
// ReSharper restore InconsistentNaming