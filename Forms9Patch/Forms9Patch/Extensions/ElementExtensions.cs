using System;
using Xamarin.Forms;
using P42.Utils;

namespace Forms9Patch
{
    /// <summary>
    /// Visual element extensions.
    /// </summary>
    public static class ElementExtensions
    {

        /// <summary>
        /// Fine an ancestor of this element of a type that is inherited from T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public static T AncestorOfBaseType<T>(this Element element) where T : Element
        {
            var parent = element?.Parent;
            while (parent != null)
            {
                if (parent is T)
                    return (T)parent;
                parent = parent.Parent;
            }
            return null;
        }

        /// <summary>
        /// Finds Ancestor element of the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public static T Ancestor<T>(this Element element) where T : Element
        {
            var parent = element?.Parent;
            while (parent != null)
            {
                if (parent.GetType() == typeof(T))
                    return (T)parent;
                parent = parent.Parent;
            }
            return null;
        }


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool IsInNativeLayout(this VisualElement e)
        {
            var isInNativeLayout = (bool)e.GetPropertyValue(nameof(IsInNativeLayout));
            return isInNativeLayout;
        }

        public static void SetIsInNativeLayout(this VisualElement e, bool value)
        {
            e.SetPropertyValue(nameof(IsInNativeLayout), value);
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


        static Interfaces.IRendererResolver _rendererResolver;
        static Interfaces.IRendererResolver RendererResolver
        {
            get
            {
                if (_rendererResolver is null)
                    _rendererResolver = DependencyService.Resolve<Interfaces.IRendererResolver>();
                return _rendererResolver;
            }
        }

        /// <summary>
        /// Returns platform renderer for VisualElement (or null)
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static object GetRenderer(this VisualElement element)
            => RendererResolver?.GetRenderer(element);

        /// <summary>
        /// Tests if VisualElement has a Xamarin.Forms platform renderer attached
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool HasRenderer(this VisualElement element)
            => GetRenderer(element) != null;
    }


}
