using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch Settings (for use by Forms9Patch PCL code).
    /// </summary>
    [DesignTimeVisible(true)]
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

        /// <summary>
        /// Enable ability to email user for help when unusual crash is encountered
        /// </summary>
        public static bool IsRequestUserHelpEnabled;

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

        /// <summary>
        /// Used internally to communicate with user when perplexing exception is triggered;
        /// </summary>
        /// <param name="e"></param>
        /// <param name="path"></param>
        /// <param name="lineNumber"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        internal static async Task RequestUserHelp(Exception e, [System.Runtime.CompilerServices.CallerFilePath] string path = null, [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = -1, [System.Runtime.CompilerServices.CallerMemberName] string methodName=null)
        {
            if (IsRequestUserHelpEnabled)
            {
                using (var popup = Forms9Patch.PermissionPopup.Create("I believe I need your help ...", "My name is Ben and I am the developer of this application (or at least the part the just didn't work).  Unfortunately you have managed to trigger a bug that, for the life of me, I cannot reproduce - and therefore fix!  Would you be willing to email me so I can learn more about what just happened?"))
                {
                    popup.IsVisible = true;
                    await popup.WaitForPoppedAsync();
                    if (popup.PermissionState == PermissionState.Ok)
                    {
                        var info = "Exception Type: "+e.GetType()+"\n\nMessage: " + e.Message + "\n\nMethod: " + methodName + "\n\nLine Number: " + lineNumber + "\n\nPath: " + path + "\n\nCall Stack Trace: " + e.StackTrace;
                        try
                        {
                            var message = new EmailMessage
                            {
                                Subject = "Help with Windows Printing Bug",
                                Body = "I'm willing to help.  Below is some information about what happened.\n\n" + info, 
                                To = new List<string> { "ben@buildcalc.com" },
                                //Cc = ccRecipients,
                                //Bcc = bccRecipients
                            };
                            await Email.ComposeAsync(message);
                        }
                        catch (Exception)
                        {
                            using (var toast = Forms9Patch.Toast.Create("Email not available", "Email cannot be opened directly from this app ... but I still would like your help.  Could you email me at ben@buildcalc.com and include (via copy and paste into your email) the below information?  Thank you for considering this! \n\n <b>INFORMATION:</b>\n\n" + info)) { }
                        }
                    }
                }

            }
        }
    }
}

