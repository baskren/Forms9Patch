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
            IsInitialized = true;
            Activity = Activity ?? Context as Android.App.Activity;
            Context = Activity as Android.Content.Context;
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
                var v1 = new BaseCellView();
                var v2 = new GroupHeaderView();
                var v3 = new BlankCellView();
                var v4 = new NullItemCellView();
                var v5 = new TextCellViewContent();
                var v6 = new HeaderCell<Label>();
                var v7 = new ItemCell<Label>();
                var v8 = new Cell<Label>();

                var a1 = new Android.Support.V7.Widget.FitWindowsFrameLayout(Activity);

                var p1 = new PopupPlatformDroid();
                var p2 = new PopupPageRenderer(null);
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

#pragma warning disable IDE0040
#pragma warning disable IDE0044 // Add readonly modifier
        static Activity Activity;
        static bool _falseflag;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore IDE0040
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0067:Dispose objects before losing scope", Justification = "<Pending>")]
        static void FalseFlag()
        {
            if (_falseflag)
            {
                var ignore = new Android.Support.V7.Widget.FitWindowsFrameLayout(Activity);
            }
        }

#pragma warning restore IDE0060 // Remove unused parameter
    }
}
