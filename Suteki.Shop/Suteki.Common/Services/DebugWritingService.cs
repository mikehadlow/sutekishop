using System;
using System.Diagnostics;

namespace Suteki.Common.Services
{
    // A service that just writes out its lifecycle events. Used to test IoC configuration

    public interface IDebugWritingService
    {
        
    }

    public class DebugWritingService : IDebugWritingService
    {
        public DebugWritingService()
        {
            Debug.WriteLine("DebugWritingService constructor called");
        }
    }

    public class DisposableDebugWritingService : IDebugWritingService, IDisposable
    {

        public DisposableDebugWritingService(params Type[] types)
        {
            Debug.WriteLine("DisposableDebugWritingService constructor called");

            foreach (var type in types)
            {
                Debug.WriteLine(string.Format("Configured with type: {0}", type.Name));
            }
        }

        public void Dispose()
        {
            Debug.WriteLine("DisposableDebugWritingService Dispose called");
        }
    }
}