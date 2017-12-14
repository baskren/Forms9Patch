using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Enable EmbeddedResource fonts to be used with Xamarin elements
    /// </summary>
    public class EmbeddedResourceFontEffect : Xamarin.Forms.RoutingEffect
    {
        internal Assembly Assembly { get; }

        /// <summary>
        /// Constructor for Forms9Patch.EmbeddedResourceFontEffect
        /// </summary>
        /// <param name="assembly"></param>
        public EmbeddedResourceFontEffect(Assembly assembly=null) : base("Forms9Patch.EmbeddedResourceFontEffect")
        {
            if (assembly == null && Device.RuntimePlatform != Device.UWP)
                assembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, new object[0]);

            Assembly = assembly;
        }

    }
}
