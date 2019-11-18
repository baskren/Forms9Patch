using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using XPlatform = Xamarin.Forms.Platform.UWP.Platform;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Core;
using Forms9Patch.Elements.Popups.Core;

[assembly: Dependency(typeof(Forms9Patch.UWP.PopupPlatformUWP))]
namespace Forms9Patch.UWP
{
	[Preserve(AllMembers = true)]
	class PopupPlatformUWP : IPopupPlatform
	{
		private IPopupNavigation PopupNavigationInstance => PopupNavigation.Instance;

		public event EventHandler OnInitialized
		{
			add => Settings.OnInitialized += value;
			remove => Settings.OnInitialized -= value;
		}

		public bool IsInitialized => Settings.IsInitialized;

		public bool IsSystemAnimationEnabled => true;

		[Preserve]
		public PopupPlatformUWP()
		{
			SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
		}

		private async void OnBackRequested(object sender, BackRequestedEventArgs e)
		{
			if (PopupNavigationInstance.PopupStack.LastOrDefault() is PopupPage lastPopupPage
				&& !(lastPopupPage.DisappearingTransactionTask != null || lastPopupPage.SendBackButtonPressed()))
			{
				e.Handled = true;
				await PopupNavigationInstance.PopAsync();
			}

		}

		public async Task AddAsync(PopupPage p)
		{
			if (p is PopupPage page
				&& new global::Windows.UI.Xaml.Controls.Primitives.Popup() is Windows.UI.Xaml.Controls.Primitives.Popup popup
				&& page.GetOrCreateRenderer() is PopupPageRenderer renderer)
			{
				page.Parent = Application.Current?.MainPage;
				renderer.Prepare(popup);
				popup.Child = renderer.ContainerElement;
				popup.IsOpen = true;
				page.ForceLayout();
				await Task.Delay(5);
			}
		}

		public async Task RemoveAsync(PopupPage page)
		{
			var renderer = (PopupPageRenderer)page.GetOrCreateRenderer();
			var popup = renderer.Container;

			if (popup != null)
			{
				renderer.Destroy();

				Cleanup(page);
				page.Parent = null;
				popup.Child = null;
				popup.IsOpen = false;
			}

			await Task.Delay(5);
		}

		internal static void Cleanup(VisualElement ve)
		{
			if (ve is VisualElement element
				&& XPlatform.GetRenderer(element) is IVisualElementRenderer elementRenderer)
			{
				foreach (Element descendant in element.Descendants())
				{
					if (descendant is VisualElement child
						&& XPlatform.GetRenderer(child) is IVisualElementRenderer childRenderer)
					{
						childRenderer.Dispose();
						XPlatform.SetRenderer(child, null);
					}
				}
				elementRenderer.Dispose();
				XPlatform.SetRenderer(element, null);
			}
		}
	}
}
