using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using P42.Utils;

namespace Forms9Patch
{
    /// <summary>
    /// Embedded Resource extension methods
    /// </summary>
    public static class EmbeddedResourceExtensions
    {
        static readonly Dictionary<Assembly, string[]> _embeddedResourceNames = new Dictionary<Assembly, string[]>();

        /// <summary>
        /// Finds the assembly that contains an embedded resource matching the resourceId
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Assembly FindAssemblyForResource(string resourceId, Assembly assembly = null)
        {
            if (assembly==null)
            {
                if (resourceId.IndexOf(".Resources.", StringComparison.Ordinal) is int index && index > 0)
                {
                    var assemblyName = resourceId.Substring(0, index);
                    if (ReflectionExtensions.GetAssemblyByName(assemblyName) is Assembly asmA
                        && FindAssemblyForResource(resourceId, asmA) is Assembly)
                        return asmA;
                }
                assembly = Xamarin.Forms.Application.Current?.GetType().Assembly;
                if (FindAssemblyForResource(resourceId, assembly) is Assembly)
                    return assembly;
                foreach (var asmB in ReflectionExtensions.GetAssemblies())
                {
                    if (!asmB.IsDynamic && FindAssemblyForResource(resourceId, asmB) is Assembly)
                        return asmB;
                }
            }
            else
            {
                if (_embeddedResourceNames.TryGetValue(assembly, out string[] names))
                    return names.Contains(resourceId) ? assembly : null;
                names = assembly.GetManifestResourceNames();
                if (names != null)
                {
                    _embeddedResourceNames[assembly] = names;
                    return names.Contains(resourceId) ? assembly : null;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds a Forms9Patch 
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Assembly FindAssemblyForMultiResource(string resourceId, Assembly assembly = null)
        {
            if (assembly?.GetManifestResourceNames().Any(id => id.StartsWith(resourceId, StringComparison.Ordinal)) ?? false)
                return assembly;
            if (resourceId.IndexOf(".Resources.", StringComparison.Ordinal) is int index && index > 0)
            {
                var assemblyName = resourceId.Substring(0, index);
                assembly = ReflectionExtensions.GetAssemblyByName(assemblyName);
                if (assembly?.GetManifestResourceNames().Any(id => id.StartsWith(resourceId, StringComparison.Ordinal)) ?? false)
                    return assembly;
            }
            assembly = Xamarin.Forms.Application.Current?.GetType().Assembly;
            if (assembly?.GetManifestResourceNames().Any(id => id.StartsWith(resourceId, StringComparison.Ordinal)) ?? false)
                return assembly;
            foreach (var assm in ReflectionExtensions.GetAssemblies())
            {
                if (!assm.IsDynamic && assm.GetManifestResourceNames().Any(id => id.StartsWith(resourceId, StringComparison.Ordinal)))
                    return assm;
            }
            return null;
        }

    }
}
