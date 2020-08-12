using Android.App;
using System;
//using Xamarin.Forms;
using Dalvik.SystemInterop;
using System.Reflection;
using System.Collections.Generic;
using Android.Content.PM;
using Forms9Patch.Elements.Popups.Core;
using Xamarin.Forms;
using System.Linq;

[assembly: Xamarin.Forms.Dependency(typeof(Forms9Patch.Droid.Settings))]
namespace Forms9Patch.Droid
{
    /// <summary>
    /// Forms9Patch Settings.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class Settings : ISettings
    {
        #region Properties
        public static Android.App.Activity Activity { get; private set; }

        static Android.Content.Context _context;
        /// <summary>
        /// An activity is a Context because ???  Android!
        /// </summary>
        /// <value>The context.</value>
        public static Android.Content.Context Context
        {
#pragma warning disable CS0618 // Type or member is obsolete
            get => _context ?? Xamarin.Forms.Forms.Context;
#pragma warning restore CS0618 // Type or member is obsolete
            private set => _context = value;
        }

        public static Android.OS.Bundle Bundle { get; private set; }

        public List<Assembly> IncludedAssemblies => throw new NotImplementedException();

        internal static bool IsInitialized { get; private set; }

        #endregion


        #region Events
        internal static event EventHandler OnInitialized;
        #endregion


        #region Initialization
        public static void Initialize(Android.App.Activity activity, string licenseKey = null)
        {
            Activity = activity;
            if (licenseKey != null)
                System.Console.WriteLine("Forms9Patch is now open source using the MIT license ... so it's free, including for commercial use.  Why?  The more people who use it, the faster bugs will be found and fixed - which helps me and you.  So, please help get the word out - tell your friends, post on social media, write about it on the bathroom walls at work!  If you have purchased a license from me, please don't get mad - you did a good deed.  They really were not that expensive and you did a great service in encouraging me keep working on Forms9Patch.");
            Init();
        }

        private static bool OnBackPressed(object sender, EventArgs e)
            => Popup.SendBackPressed();

        void ISettings.LazyInit()
        {
            if (IsInitialized)
                return;
            Init();
        }

        static void Init()
        {
            //Android.Webkit.WebView.EnableSlowWholeDocumentDraw();
            IsInitialized = true;
            Activity = Activity ?? Context as Android.App.Activity;
            Context = Activity as Android.Content.Context;
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                Android.Webkit.WebView.EnableSlowWholeDocumentDraw();
            // these don't work because we get the notification AFTER Xamarin did ... and it runs through all of the subscribers anyway.
            //Xamarin.Forms.Platform.Android.FormsAppCompatActivity.BackPressed += OnBackPressed;
            //Xamarin.Forms.Platform.Android.FormsApplicationActivity.BackPressed += OnBackPressed;
            FormsGestures.Droid.Settings.Init(Activity);
            OnInitialized?.Invoke(null, EventArgs.Empty);
            LinkAssemblies();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0067:Dispose objects before losing scope", Justification = "<Pending>")]
        private static void LinkAssemblies()
        {
            if (false.Equals(true))
            {
                var r1 = new PopupPlatformDroid();
                var r2 = new PopupPageRenderer(null);
                var r3 = new ColorGradientBoxRenderer(null);
                var r4 = new EnhancedListViewRenderer(null);
                var r5 = new HardwareKeyPageRenderer(null);
                var r6 = new LabelRenderer(null);

                var e1 = new EmbeddedResourceFontEffect();
                var e2 = new EntryClearButtonEffect();
                var e3 = new EntryNoUnderlineEffect();
                var e4 = new SliderStepSizeEffect();
                var e5 = new HardwareKeyListenerEffect();

                var s1 = new ApplicationInfoService();
                var s2 = new AudioService();
                var s3 = new DescendentBounds();
                var s4 = new FontService();
                var s5 = new HapticService();
                var s6 = new KeyboardService();
                var s7 = new OsInfoService();
                var s8 = new PrintService();
                var s9 = new ToPdfService();
                var s10 = new ToPngService();

            }
        }
        #endregion
    }
}

namespace Forms9Patch
{
    internal class LinkerInclude
    {
#pragma warning disable IDE0060 // Remove unused parameter
        public BaseCellView Include(BaseCellView view)
            => new BaseCellView();

        public GroupHeaderView Include(GroupHeaderView view)
            => new GroupHeaderView();

        public BlankCellView Include(BlankCellView view)
            => new BlankCellView();

        public NullItemCellView Include(NullItemCellView view)
            => new NullItemCellView();

        public TextCellViewContent Include(TextCellViewContent view)
            => new TextCellViewContent();

        public HeaderCell<Label> Include(HeaderCell<Label> cell)
            => new HeaderCell<Label>();

        public ItemCell<Label> Include(ItemCell<Label> cell)
            => new ItemCell<Label>();

        public Cell<Label> Include(Cell<Label> cell)
            => new Cell<Label>();


#pragma warning restore IDE0060 // Remove unused parameter
    }
}
