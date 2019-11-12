using Forms9Patch.Elements.Popups.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.UWP.Settings))]
namespace Forms9Patch.UWP
{
    public class Settings : ISettings
    {
        #region Fields
        static internal Windows.UI.Xaml.Application Application;

        public List<Assembly> IncludedAssemblies => AssembliesToInclude;
        #endregion


        #region Properties
        internal static bool IsInitialized { get; private set; }
        #endregion


        #region Events
        internal static event EventHandler OnInitialized;
        #endregion


        #region Initialization
        public static void Initialize(Windows.UI.Xaml.Application app, string licenseKey = null)
        {
            //Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            Init();
            if (licenseKey != null)
                System.Console.WriteLine("Forms9Patch is now open source using the MIT license ... so it's free, including for commercial use.  Why?  The more people who use it, the faster bugs will be found and fixed - which helps me and you.  So, please help get the word out - tell your friends, post on social media, write about it on the bathroom walls at work!  If you have purchased a license from me, please don't get mad - you did a good deed.  They really were not that expensive and you did a great service in encouraging me keep working on Forms9Patch.");
        }

        void ISettings.LazyInit()
        {
            if (IsInitialized)
                return;
            Init();
        }

        static void Init()
        {
            IsInitialized = true;
            Application = Windows.UI.Xaml.Application.Current;
            FormsGestures.UWP.Settings.Init(Windows.UI.Xaml.Application.Current);
            OnInitialized?.Invoke(null, EventArgs.Empty);
            LinkAssemblies();
        }
        #endregion

#pragma warning disable CC0057 // Unused parameters
#pragma warning disable IDE0060 // Remove unused parameter
        public static void OnBackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore CC0057 // Unused parameters
        {
            var popupNavigationInstance = PopupNavigation.Instance;
            //e.Handled = false;
            if (popupNavigationInstance.PopupStack.Count > 0)
            {
                var lastPage = popupNavigationInstance.PopupStack.Last();

                var isPreventClose = // lastPage.IsBeingDismissed || 
                    lastPage.SendBackButtonPressed();

                if (!isPreventClose)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                    {
                        await popupNavigationInstance.PopAsync();
                    });
                }

                e.Handled = true;
            }

        }




        static Windows.UI.Xaml.ResourceDictionary GetResources()
        {
            return new Windows.UI.Xaml.ResourceDictionary
            {
                Source = new Uri("ms-appx:///Forms9Patch.UWP/Forms9Patch.UWP.EnhancedListView.Resources.xbf")
            };
        }

        #region Xamarin.Forms.UWP Requirements
        static List<Assembly> _forms9PatchAssemblies = null;
        internal static List<Assembly> Forms9PatchAssemblies
        {
            get
            {
                if (_forms9PatchAssemblies == null)
                {
                    _forms9PatchAssemblies = new List<Assembly>();
                    try { _forms9PatchAssemblies.Add(typeof(Forms9Patch.Settings).GetTypeInfo().Assembly); }
                    catch (Exception) { throw new Exception("Cannot load Forms9Patch assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(Settings).GetTypeInfo().Assembly); }
                    catch (Exception) { throw new Exception("Cannot load Forms9Patch.UWP assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(FormsGestures.Listener).GetTypeInfo().Assembly); }
                    catch (Exception) { throw new Exception("Cannot load FormsGestures assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(P42.NumericalMethods.Search1D).GetTypeInfo().Assembly); }
                    catch (Exception) { throw new Exception("Cannot load NumericalMethods assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(P42.Utils.DownloadCache).GetTypeInfo().Assembly); }
                    catch (Exception) { throw new Exception("Cannot load P42.Utils assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(Windows.ApplicationModel.Core.AppListEntry).GetTypeInfo().Assembly); }
                    catch (Exception) { throw new Exception("Cannot load Windows.ApplicationModel.Core assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(Newtonsoft.Json.JsonConvert).GetTypeInfo().Assembly); }
                    catch (Exception) { throw new Exception("Cannot load Newtonsoft.Json assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(SkiaSharp.SKBitmap).GetTypeInfo().Assembly); }
                    catch (Exception) { throw new Exception("Cannot load SkiaSharp assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(SkiaSharp.Views.UWP.SKXamlCanvas).GetTypeInfo().Assembly); }
                    catch (Exception) { throw new Exception("Cannot load SkiaSharp.Views.UWP assembly"); }

                    try { _forms9PatchAssemblies.Add(typeof(SharpDX.Direct2D1.Factory).GetTypeInfo().Assembly); }
                    catch (Exception) { throw new Exception("Cannot load SharpDX.Direct2D1 assembly"); }

                }
                return _forms9PatchAssemblies;
            }
        }

        static List<Assembly> _assembliesToInclude;
        public static List<Assembly> AssembliesToInclude
        {
            get
            {
                if (_assembliesToInclude == null)
                {
                    _assembliesToInclude = new List<Assembly>();
                    _assembliesToInclude.AddRange(Forms9PatchAssemblies);
                }
                return _assembliesToInclude;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0067:Dispose objects before losing scope", Justification = "<Pending>")]
        private static void LinkAssemblies()
        {
            Xamarin.Forms.DependencyService.Register<ApplicationInfoService>();
            Xamarin.Forms.DependencyService.Register<DescendentBounds>();
            Xamarin.Forms.DependencyService.Register<InstalledFont>();
            Xamarin.Forms.DependencyService.Register<FontService>();
            Xamarin.Forms.DependencyService.Register<HtmlToPngService>();
            Xamarin.Forms.DependencyService.Register<KeyboardService>();
            Xamarin.Forms.DependencyService.Register<OsInfoService>();
            Xamarin.Forms.DependencyService.Register<WebViewExtensionsService>();
            Xamarin.Forms.DependencyService.Register<Settings>();
            Xamarin.Forms.DependencyService.Register<PopupPlatformUWP>();
            Xamarin.Forms.DependencyService.Register<SharingService>();
            Xamarin.Forms.DependencyService.Register<ClipboardService>();

            if (false.Equals(true))
            {

                // Effects
                var e1 = new EmbeddedResourceFontEffect();
                var e2 = new SliderStepSizeEffect();

                // Hardware Key Listener
                var h1 = new HardwareKeyListenerEffect();
                var h2 = new HardwareKeyPageRenderer();

                // Popup
                var P1 = new PopupPageRenderer();

                // Renderers
                var r1 = new EnhancedListViewRenderer();
                var r2 = new LabeLRenderer();

            }
        }
        #endregion
    }
}
