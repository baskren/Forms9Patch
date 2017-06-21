using System;
using System.Reflection;
using Xamarin.Forms;

namespace Forms9Patch.Extensions
{
    /// <summary>
    /// Application extensions.
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Gets the xamarin forms application assembly.
        /// </summary>
        /// <returns>The xamarin forms application assembly.</returns>
        public static Assembly GetXamarinFormsApplicationAssembly()
        {
            var app = Xamarin.Forms.Application.Current;
            if (app == null)
                return null;
            var appType = app.GetType();
            var appAsmTypeInfo = appType.GetTypeInfo();
            var appAsm = appAsmTypeInfo.Assembly;
            return appAsm;
        }
    }
}
