using System.ComponentModel;
using System.Reflection;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Enable EmbeddedResource fonts to be used with Xamarin elements
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class EmbeddedResourceFontEffect : Xamarin.Forms.RoutingEffect
    {
        static EmbeddedResourceFontEffect()
        {
            Settings.ConfirmInitialization();
        }

        internal Assembly Assembly { get; }

        /// <summary>
        /// Constructor for Forms9Patch.EmbeddedResourceFontEffect
        /// </summary>
        /// <param name="assembly"></param>
        public EmbeddedResourceFontEffect(Assembly assembly = null) : base("Forms9Patch.EmbeddedResourceFontEffect")
        {
            //if (assembly == null && Device.RuntimePlatform != Device.UWP)
            //    assembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, new object[0]);

            Assembly = assembly;
        }

        /// <summary>
        /// Applies EmbeddedResourceFontEffect to a Xamarin.Forms.VisualElement
        /// </summary>
        /// <returns><c>true</c>, if to was applyed, <c>false</c> otherwise.</returns>
        /// <param name="element">Element.</param>
        /// <param name="assembly">Assembly.</param>
        public static bool ApplyTo(VisualElement element, Assembly assembly = null)
        {
            //if (assembly == null && Device.RuntimePlatform != Device.UWP)
            //    assembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, new object[0]);
            //assembly = assembly ?? Assembly.GetCallingAssembly();
            var effect = new EmbeddedResourceFontEffect(assembly);
            if (effect != null)
            {
                element.Effects.Add(effect);
                return element.Effects.Contains(effect);
            }
            return false;
        }
    }
}
