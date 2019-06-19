using System;
using System.Linq;
using System.Reflection;
using P42.Utils;

namespace Forms9Patch
{
    public static class EmbeddedResourceExtensions
    {
        public static Assembly FindAssemblyForResource(string resourceId, Assembly assembly=null)
        {
            if (assembly?.GetManifestResourceNames().Contains(resourceId) ?? false)
                return assembly;
            var assemblyName = resourceId.Substring(0, resourceId.IndexOf(".Resources.", StringComparison.Ordinal));
            assembly = ReflectionExtensions.GetAssemblyByName(assemblyName);
            var names = assembly?.GetManifestResourceNames();
            if (assembly?.GetManifestResourceNames().Contains(resourceId) ?? false)
                return assembly;
            assembly = Xamarin.Forms.Application.Current.GetType().Assembly;
            if (assembly?.GetManifestResourceNames().Contains(resourceId) ?? false)
                return assembly;
            foreach (var assm in ReflectionExtensions.GetAssemblies())
            {
                if (assm.GetManifestResourceNames().Contains(resourceId))
                    return assm;
            }
            return null;
        }

        public static Assembly FindAssemblyForMultiResource(string resourceId, Assembly assembly = null)
        {
            if (assembly?.GetManifestResourceNames().Any(id=> id.StartsWith(resourceId, StringComparison.Ordinal)) ?? false)
                return assembly;
            var assemblyName = resourceId.Substring(0, resourceId.IndexOf(".Resources.", StringComparison.Ordinal));
            assembly = ReflectionExtensions.GetAssemblyByName(assemblyName);
            var names = assembly?.GetManifestResourceNames();
            if (assembly?.GetManifestResourceNames().Any(id => id.StartsWith(resourceId, StringComparison.Ordinal)) ?? false)
                return assembly;
            assembly = Xamarin.Forms.Application.Current.GetType().Assembly;
            if (assembly?.GetManifestResourceNames().Any(id => id.StartsWith(resourceId, StringComparison.Ordinal)) ?? false)
                return assembly;
            foreach (var assm in ReflectionExtensions.GetAssemblies())
            {
                if (assm.GetManifestResourceNames().Any(id => id.StartsWith(resourceId, StringComparison.Ordinal)))
                    return assm;
            }
            return null;
        }

    }
}
