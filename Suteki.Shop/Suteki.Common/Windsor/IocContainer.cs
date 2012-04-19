using System;

namespace Suteki.Common.Windsor
{
    public class IocContainer
    {
        private static Func<Type, object> resolve;
        private static Action<object> release;

        public static T Resolve<T>()
        {
            return (T)Resolve(typeof (T));
        }

        public static object Resolve(Type type)
        {
            if (resolve == null)
            {
                throw new SutekiCommonException(
                    "No resolve function has been set. Call IoC.SetResolveFunction(Func<Type, object> resolveFunction) first");
            }
            return resolve(type);
        }

        public static void Release(object instance)
        {
            if (release == null)
            {
                throw new SutekiCommonException(
                    "No release action has been set. Call IoC.SetReleaseAction(Action<object> releaseAction) first");
            }
            release(instance);
        }

        public static void SetResolveFunction(Func<Type, object> resolveFunction)
        {
            if (resolveFunction == null)
            {
                throw new ArgumentNullException("resolveFunction");
            }

            resolve = resolveFunction;
        }

        public static void SetReleaseAction(Action<object> releaseAction)
        {
            if (releaseAction == null)
            {
                throw new ArgumentNullException("releaseAction");
            }

            release = releaseAction;
        }

        public static void Reset()
        {
            resolve = null;
            release = null;
        }
    }
}