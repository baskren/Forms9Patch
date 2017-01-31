// /*******************************************************************
//  *
//  * PopupPage.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;
using System.Linq;

namespace Forms9Patch
{
	/// <summary>
	/// Page that supports popups
	/// </summary>
	public class RootPage : Page
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.RootPage"/> class.
		/// </summary>
		/// <param name="contentPage">Content page.</param>
		public RootPage(Page contentPage) : base()
		{
			if (contentPage == null)
				throw new NotSupportedException("RootPage must be supported with a valid Page instance");
			PageController.InternalChildren.Add(contentPage);

			var navPage = contentPage as NavigationPage;
			if (navPage != null)
			{
				navPage.Popped += (sender, e) => RemovePopups(true);

				navPage.Pushed += (sender, e) => RemovePopups(false);

				navPage.PoppedToRoot += (sender, e) => RemovePopups(true);
			}
		}


		IPageController PageController => this as IPageController;

		internal void AddPopup(PopupBase popup)
		{
			//Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(popup, Bounds);
			popup.ManualLayout(Bounds);
			if (!PageController.InternalChildren.Contains(popup))
				PageController.InternalChildren.Add(popup);
			popup.PresentedAt = DateTime.Now;
		}

		internal void RemovePopup(PopupBase popup)
		{
			if (popup.Retain)
				return;
			while (PageController.InternalChildren.Contains(popup))
			{
				PageController.InternalChildren.Remove(PageController.InternalChildren.Last());
				//Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(popup, new Rectangle(0, 0, -1, -1));
			}
		}


		internal void RemovePopups(bool popping)
		{
			for (int i = PageController.InternalChildren.Count() - 1; i > 0; i--)
			{
					var popup = PageController.InternalChildren[i] as PopupBase;
					if (popup != null && (popup.PresentedAt.AddSeconds(2) < DateTime.Now || popping))
						PageController.InternalChildren.RemoveAt(i);
			}
		}

		/// <summary>
		/// Ons the back button pressed.
		/// </summary>
		/// <returns><c>true</c>, if back button pressed was oned, <c>false</c> otherwise.</returns>
		protected override bool OnBackButtonPressed()
		{
			if (PageController.InternalChildren.Count() > 1)
			{
				PageController.InternalChildren.Remove(PageController.InternalChildren.Last());
				return true;
			}
			return base.OnBackButtonPressed();
		}
	}
}
