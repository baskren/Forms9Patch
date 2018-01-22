using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Xamarin.Forms;

namespace Forms9Patch
{
    static class AssemblyExtensions
    {
        public static Assembly AssemblyFromResourceId(string resourceId)
        {
            if (string.IsNullOrWhiteSpace(resourceId))
                return null;
            Assembly assembly = null;
            var resourcePath = resourceId.Split('.').ToList();
            if (resourceId.Contains(".Resources."))
            {
                var resourceIndex = resourcePath.IndexOf("Resources");
                var asmName = string.Join(".", resourcePath.GetRange(0, resourceIndex));
                var appAsm = ApplicationExtensions.GetXamarinFormsApplicationAssembly();
                if (appAsm?.GetName().Name == asmName)
                    return appAsm;

                //if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
                if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
                    assembly = P42.Utils.ReflectionExtensions.GetAssemblyByName(asmName);
                //else if (Device.OS == TargetPlatform.Windows)
                else if (Device.RuntimePlatform == Device.UWP)
                {
                    foreach (var asm in Settings.IncludedAssemblies)
                        if (asm?.GetName().Name == asmName)
                            return asm;
                }
            }
            if (assembly == null)
            {
                for (int i = resourcePath.Count - 1; i < 0; i--)
                {
                    var asmName = string.Join(".", resourcePath.GetRange(0, i));
                    if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
                    {
                        assembly = P42.Utils.ReflectionExtensions.GetAssemblyByName(asmName);
                        if (assembly != null)
                            return assembly;
                    }
                    else if (Device.RuntimePlatform == Device.UWP)
                    {
                        foreach (var asm in Settings.IncludedAssemblies)
                            if (asm?.GetName().Name == asmName)
                                return asm;
                    }

                }
            }
            return assembly;
        }
    }
}
