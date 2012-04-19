using System;
using System.Web;
using Castle.Windsor;

namespace Suteki.Common.Events
{
    public class DomainEvent
    {
        public static Action<IDomainEvent> RaiseAction { get; set; }
        public static Func<IWindsorContainer> ReturnContainer { get; set; }

        public static void Raise<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
        {
            if (@event == null)
            {
                throw new ArgumentNullException("@event");
            }

            if(RaiseAction != null)
            {
                RaiseAction(@event);
                return;
            }

            if (ContainerUnavailable()) return;

            var container = GetContainer();
            var handlers = container.ResolveAll<IHandle<TEvent>>();

            foreach (var handler in handlers)
            {
                handler.Handle(@event);
                container.Release(handler);
            }
        }

        private static bool ContainerUnavailable()
        {
            return (ReturnContainer == null) && (HttpContext.Current == null);
        }

        private static IWindsorContainer GetContainer()
        {
            if (ReturnContainer != null)
            {
                return ReturnContainer();
            }

            if (HttpContext.Current == null)
            {
                throw new SutekiCommonException("DomainEvent.Raise can only be used in a web application. " +
                    "For testing, please set the DomainEvent.RaiseAction delegate before calling Raise.");
            }

            var accessor = HttpContext.Current.ApplicationInstance as IContainerAccessor;
            if (accessor == null)
            {
                throw new SutekiCommonException("The Global.asax Application class does not " +
                    "implement IContainerAccessor.");
            }
            return accessor.Container;
        }

        // test helpers

        public static DomainEventReset TurnOff()
        {
            RaiseAction = e => { };
            return new DomainEventReset();
        }

        public static DomainEventReset TestWith(Action<IDomainEvent> raiseAction)
        {
            RaiseAction = raiseAction;
            return new DomainEventReset();
        }

        public static void Reset()
        {
            RaiseAction = null;
        }
    }

    public class DomainEventReset : IDisposable
    {
        public void Dispose()
        {
            DomainEvent.Reset();
        }
    }
}