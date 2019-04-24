using Android.App;
using System;
//using Xamarin.Forms;
using Dalvik.SystemInterop;
using System.Reflection;
using System.Collections.Generic;

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

        #endregion


        #region Initialization
        public static void Initialize(Android.App.Activity activity, string licenseKey = null)
        {
            _initizalized = true;
            Activity = activity;
            Context = activity as Android.Content.Context;
            // these don't work because we get the notification AFTER Xamarin did ... and it runs through all of the subscribers anyway.
            //Xamarin.Forms.Platform.Android.FormsAppCompatActivity.BackPressed += OnBackPressed;
            //Xamarin.Forms.Platform.Android.FormsApplicationActivity.BackPressed += OnBackPressed;
            FormsGestures.Droid.Settings.Init(Context);
            Rg.Plugins.Popup.Popup.Init(Activity, null);
            if (licenseKey != null)
                System.Console.WriteLine("Forms9Patch is now open source using the MIT license ... so it's free, including for commercial use.  Why?  The more people who use it, the faster bugs will be found and fixed - which helps me and you.  So, please help get the word out - tell your friends, post on social media, write about it on the bathroom walls at work!  If you have purchased a license from me, please don't get mad - you did a good deed.  They really were not that expensive and you did a great service in encouraging me keep working on Forms9Patch.");
        }

        private static bool OnBackPressed(object sender, EventArgs e)
        {

            var result = Rg.Plugins.Popup.Popup.SendBackPressed();
            System.Diagnostics.Debug.WriteLine("result=[" + result + "]");
            return result;
        }


        static bool _initizalized;
        void ISettings.LazyInit()
        {
            if (_initizalized)
                return;
            _initizalized = true;
            Activity = Context as Android.App.Activity;
            FormsGestures.Droid.Settings.Init(Activity);
            Rg.Plugins.Popup.Popup.Init(Activity, null);
        }
        #endregion
    }
}

namespace Forms9Patch
{
    internal class LinkerInclude
    {
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
    }
}
