using System;
using Xamarin.Forms;
namespace Forms9Patch
{
	/// <summary>
	/// Visual element extensions.
	/// </summary>
	public static class VisualElementExtensions
	{
		/// <summary>
		/// the element's hosting page.
		/// </summary>
		/// <returns>The page.</returns>
		/// <param name="element">Element.</param>
		public static Page HostingPage(this Element element)
		{
			var page = element as Page;
			while (page == null && element.Parent != null && !(element.Parent is MultiPage<Page>))
			{
				element = element.Parent;
				page = element as Page;
			}
			return page;
		}

		/// <summary>
		/// The topmost page
		/// </summary>
		/// <returns>The page.</returns>
		/// <param name="element">Element.</param>
		public static Page TopPage(this Element element)
		{
			while (element.Parent != null)
				element = element.Parent;
			return HostingPage(element);
		}
	}
}
