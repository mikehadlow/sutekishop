using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace Suteki.Common.AddIns
{
    // From http://www.codeproject.com/KB/aspnet/ASP2UserControlLibrary.aspx

    public class AssemblyResourceVirtualPathProvider : VirtualPathProvider
    {
        private readonly Dictionary<string, Assembly> nameAssemblyCache;

        public AssemblyResourceVirtualPathProvider()
        {
            nameAssemblyCache = new Dictionary<string, Assembly>(StringComparer.InvariantCultureIgnoreCase);
        }

        private static bool IsAppResourcePath(string virtualPath)
        {
            string checkPath = VirtualPathUtility.ToAppRelative(virtualPath);

            return checkPath.StartsWith("~/App_Resource/",
                                        StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool FileExists(string virtualPath)
        {
            return (IsAppResourcePath(virtualPath) ||
                    base.FileExists(virtualPath));
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsAppResourcePath(virtualPath))
            {
                return new AssemblyResourceFile(nameAssemblyCache, virtualPath);
            }

            return base.GetFile(virtualPath);
        }

        public override CacheDependency GetCacheDependency(
            string virtualPath,
            IEnumerable virtualPathDependencies,
            DateTime utcStart)
        {
            if (IsAppResourcePath(virtualPath))
            {
                return null;
            }

            return base.GetCacheDependency(virtualPath,
                                           virtualPathDependencies, utcStart);
        }

        private class AssemblyResourceFile : VirtualFile
        {
            private readonly IDictionary<string, Assembly> nameAssemblyCache;
            private readonly string assemblyPath;

            public AssemblyResourceFile(IDictionary<string, Assembly> nameAssemblyCache, string virtualPath) :
                base(virtualPath)
            {
                this.nameAssemblyCache = nameAssemblyCache;
                assemblyPath = VirtualPathUtility.ToAppRelative(virtualPath);
            }

            public override Stream Open()
            {
                // ~/App_Resource/WikiExtension.dll/WikiExtension/Presentation/Views/Wiki/Index.aspx (or .ascx)
                var parts = assemblyPath.Split(new[] { '/' }, 4);

                if (parts.Length != 4 || parts[0] != "~" || parts[1] != "App_Resource")
                {
                    throw new SutekiCommonException("Wrong number of parts in assmbly path: '{0}'. Expected ~/App_Resource/<assmblyFileName>/<Path to view>.aspx (or .ascx)");
                }

                var assemblyName = parts[2];
                var resourceName = parts[3].Replace('/', '.');

                Assembly assembly;

                lock (nameAssemblyCache)
                {
                    if (!nameAssemblyCache.TryGetValue(assemblyName, out assembly))
                    {
                        var path = Path.Combine(HttpRuntime.BinDirectory, assemblyName);
                        assembly = Assembly.LoadFrom(path);

                        // TODO: Assert is not null
                        nameAssemblyCache[assemblyName] = assembly;
                    }
                }

                if (assembly == null)
                {
                    throw new SutekiCommonException("Could not load AddIn assembly '{0}' when attempting to load AddIn view '{1}'", assemblyName, assemblyPath);
                }

                var resourceStream = assembly.GetManifestResourceStream(resourceName);

                if (resourceStream == null)
                {
                    throw new SutekiCommonException("Could not load AddIn view. Failed to find resource '{0}' in assembly '{1}'", resourceName, assemblyName);
                }

                return resourceStream;
            }
        }
    }
}