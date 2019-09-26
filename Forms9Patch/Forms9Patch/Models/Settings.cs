using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch Settings (for use by Forms9Patch PCL code).
    /// </summary>
    public static class Settings
    {
        static Settings()
        {
            P42.Utils.Environment.EmbeddedResourceAssemblyResolver = AssemblyExtensions.AssemblyFromResourceId;
        }

        #region Shadow properties
        /// <summary>
        /// The shadow offset.
        /// </summary>
        public static Point ShadowOffset = new Point(0, 1);

        /// <summary>
        /// The shadow radius.
        /// </summary>
        public static double ShadowRadius = 5;
        #endregion

        internal static TimeSpan MsUntilTapped = TimeSpan.FromMilliseconds(210);


        #region Swipe menu
        /// <summary>
        /// WidthRequest for ListView Cell swipe popup menu.
        /// </summary>
        public static double ListViewCellSwipePopupMenuWidthRequest = 250;
        /// <summary>
        /// FontSize for ListView Cell swipe popup menu text.
        /// </summary>
        public static double ListViewCellSwipePopupMenuFontSize = 16;
        /// <summary>
        /// Color for ListView Cell swipe popup menu text.
        /// </summary>
        public static Color ListViewCellSwipePopupMenuTextColor = Color.FromHex("0076FF");
        /// <summary>
        /// Color for background of ListView Cell swipe popup menu buttons.
        /// </summary>
        public static Color ListViewCellSwipePopupMenuButtonColor = Color.White;
        /// <summary>
        /// OutlineColor for background of ListView Cell swipe popup menu buttons.
        /// </summary>
        public static Color ListViewCellSwipePopupMenuButtonOutlineColor = Color.FromHex("CCC");
        /// <summary>
        /// OutlineWidth for background of ListView Cell swipe popup menu buttons.
        /// </summary>
        public static float ListViewCellSwipePopupMenuButtonOutlineWidth = 0;
        /// <summary>
        /// OutlineRadius for background of ListView Cell swipe popup menu buttons.
        /// </summary>
        public static float ListViewCellSwipePopupMenuButtonOutlineRadius = 6;
        /// <summary>
        /// SeparatorWidth for background of ListView Cell swipe popup menu buttons.
        /// </summary>
        public static float ListViewCellSwipePopupMenuButtonSeparatorWidth = 0;
        /// <summary>
        /// Are haptics active by default?
        /// </summary>
        #endregion



        public static KeyClicks KeyClicks = KeyClicks.Default;

        /// <summary>
        /// Haptic effect to use if HapticEffect = Default;
        /// </summary>
        public static EffectMode HapticEffectMode = EffectMode.Default;

        /// <summary>
        /// Sound effect to use if SoundEffect = Default;
        /// </summary>
        public static EffectMode SoundEffectMode = EffectMode.Default;



        #region Confirm that Forms9Patch has been initialized
        static bool _confirmed;
        internal static void ConfirmInitialization()
        {
            if (_confirmed)
                return;

            if (DependencyService.Get<ISettings>() is ISettings platformSettings)
            {
                platformSettings.LazyInit();
                _confirmed = true;
            }

            if (!_confirmed)
                throw new Exception("Unable to confirmed initialization.  Did you forget to add " +
                    "Forms9Patch." + Device.RuntimePlatform + ".Settings.Initialize() after XamarinForms.Forms.Forms.Init()?");
        }
        #endregion


        #region Native Settings (mostly used for UWP initialization)
        static ISettings _nativeSettings;
        static ISettings NativeSettings
        {
            get
            {
                _nativeSettings = _nativeSettings ?? Xamarin.Forms.DependencyService.Get<ISettings>();
                return _nativeSettings;
            }
        }

        internal static List<Assembly> IncludedAssemblies => NativeSettings?.IncludedAssemblies;
        #endregion


    }
}

