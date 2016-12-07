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
			if (element == null)
				return null;
			var page = element as Page;
			while (page == null && element.Parent != null)
			{
				element = element.Parent;
				page = element as Page;
			}
			Page lastSoloPage = page;
			if (page != null)
			{
				// ok we have a page ... is it the right kind?
				while (page != null && page.Parent != null)
				{
					if (!(page is MasterDetailPage /* && Device.OS == TargetPlatform.Android*/))
						lastSoloPage = page;
					page = page.Parent as Page;
				}
			}
			return lastSoloPage;
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
