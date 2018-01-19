using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch Page extensions.
	/// </summary>
	internal static class PageExtensions
	{
		/// <summary>
		/// The window bounds function property.
		/// </summary>
		public static readonly BindableProperty WindowBoundsFunctionProperty = BindableProperty.Create("GetNativeBounds",typeof(Func<VisualElement, Rectangle>), typeof(Page), null);

		/// <summary>
		/// Bounds for child in the Page's coordinates
		/// </summary>
		/// <returns>The for child.</returns>
		/// <param name="page">Page.</param>
		/// <param name="element">Element.</param>
		public static Rectangle BoundsForChild(this Page page, VisualElement element) {
			var func = (Func<VisualElement, Rectangle>) page.GetValue (WindowBoundsFunctionProperty);
			//return func (element);
			return func?.Invoke(element);
		}

		/// <summary>
		/// Sets the function that will return the bounds for child view in page view's coordinates
		/// </summary>
		/// <param name="page">Page.</param>
		/// <param name="func">Func.</param>
		public static void SetBoundsForChildFunction(this Page page, Func<VisualElement, Rectangle> func) {
			page.SetValue (WindowBoundsFunctionProperty, func);
		}
	}
}

