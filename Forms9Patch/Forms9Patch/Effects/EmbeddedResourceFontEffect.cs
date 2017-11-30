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
    public class EmbeddedResourceFontEffect : Xamarin.Forms.RoutingEffect
    {
        internal Assembly Assembly { get; }

        public EmbeddedResourceFontEffect(Assembly assembly=null) : base("Forms9Patch.EmbeddedResourceFontEffect")
        {
            Assembly = assembly ?? (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, new object[0]);
        }

    }
}
