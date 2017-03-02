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
		/*
		static Rectangle _statusBarFrame;
		static double _statusBarHeightAtStart=20;
		/// <summary>
		/// Gets or sets the status bar frame  - used for setup and responding to iOS status bar changes.
		/// </summary>
		/// <value>The status bar frame.</value>
		static public Rectangle StatusBarFrame
		{
			get
			{
				return _statusBarFrame;
			}
			set
			{
				if (_statusBarFrame == default(Rectangle))
					_statusBarHeightAtStart = value.Height;
				_statusBarFrame = value;
			}
		}
		*/


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
			popup.MeasureInvalidated -= OnChildMeasureInvalidated;
		}

		internal void RemovePopup(PopupBase popup)
		{
			if (popup.Retain)
				return;
			/* seems to be causing some problems (race condition?) with presenting a
			while (PageController.InternalChildren.Contains(popup))
			{
				PageController.InternalChildren.Remove(PageController.InternalChildren.Last());
				//Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(popup, new Rectangle(0, 0, -1, -1));
			}
			*/
			if (PageController.InternalChildren.Contains(popup))
				PageController.InternalChildren.Remove(popup);
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
			var masterDetailPage = (PageController.InternalChildren[0] as MasterDetailPage) ?? (PageController.InternalChildren[0] as NavigationPage)?.CurrentPage as MasterDetailPage;
			if (masterDetailPage != null && masterDetailPage.IsPresented)
			{
				masterDetailPage.IsPresented = false;
				return true;
			}
			return base.OnBackButtonPressed();
		}

		bool _ignoreChildren;
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.StackLayout"/> will not invalidate itself when a child changes.
		/// </summary>
		/// <value><c>true</c> if ignore children; otherwise, <c>false</c>.</value>
		public bool IgnoreChildren
		{
			get
			{
				return _ignoreChildren;
			}
			set
			{
				if (_ignoreChildren != value)
				{
					_ignoreChildren = value;
					if (_ignoreChildren)
						foreach (var child in PageController.InternalChildren)
						{
							var view = child as View;
							if (view != null)
								view.MeasureInvalidated -= OnChildMeasureInvalidated;
						}
					else
						foreach (var child in PageController.InternalChildren)
						{
							var view = child as View;
							if (view != null)
								view.MeasureInvalidated += OnChildMeasureInvalidated;
						}
				}
			}
		}


		static double _startingHeight=-1;

		/// <summary>
		/// Layouts the children.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			if (_startingHeight < 0)
				_startingHeight = StatusBarService.Height;
			
			System.Diagnostics.Debug.WriteLine("_startingHeight=["+_startingHeight+"]  StatusBar.Visible=["+StatusBarService.IsVisible+"] StatusBar.Height=["+StatusBarService.Height+"]");
			if (Device.OS == TargetPlatform.iOS && !(PageController.InternalChildren[0] is NavigationPage))
			{


				/* keeping the below around just in case Apple decides to create more permutations for the status bar
				var verticalY = 0.0;
				var verticalHeight = height;
				if (_startingHeight == 20)
				{
					// normal
					if (StatusBarService.Height == 0)
					{
						verticalY = 0;
						verticalHeight = height;
					}
					else if (StatusBarService.Height == 20)
					{
						verticalY = 20;
						verticalHeight = height - 20;
					}
					else if (StatusBarService.Height == 40)
					{
						verticalY = 20;
						verticalHeight = height - 20;
					}
				}
				else if (_startingHeight == 40)
				{
					// call in progress status bar
					if (StatusBarService.Height == 0)
					{
						verticalY = -20;
						verticalHeight = height;
					}
					else if (StatusBarService.Height == 20)
					{
						verticalY = 0;
						verticalHeight = height - 20;
					}
					else if (StatusBarService.Height == 40)
					{
						verticalY = 0;
						verticalHeight = height - 20;
					}
				}
				else if (_startingHeight == 0)
				{
					// hidden status bar
					if (StatusBarService.Height == 0)
					{
						verticalY = 0;
						verticalHeight = height;
					}
					else if (StatusBarService.Height == 20)
					{
						verticalY = 20;
						verticalHeight = height - 20;
					}
					else if (StatusBarService.Height == 40)
					{
						verticalY = 20;
						verticalHeight = height - 20;
					}
				}
				*/
				var verticalY = 20 - Math.Max(20,_startingHeight) + Math.Min(20,StatusBarService.Height);
				var verticalHeight = height - Math.Min(20, StatusBarService.Height);

				base.LayoutChildren(x, verticalY, width, verticalHeight);
			}
			else
				base.LayoutChildren(x, y, width, height);
		}
	}
}
