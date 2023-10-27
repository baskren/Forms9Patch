using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Forms9Patch
{
    /// <summary>
    /// Class with helper methods intended to ask for user assistance in debugging application using Forms9Patch
    /// </summary>
    public class Debug
    {
        /// <summary>
        /// Enable ability to email user for help when unusual crash is encountered
        /// </summary>
        public static bool IsRequestUserHelpEnabled;

        /// <summary>
        /// Email address to which to send requests
        /// </summary>
        public static string RequestUserHelpEmailToAddress = "help@buildcalc.com";

        /// <summary>
        /// Title of PermissionPopup presented to user where "YES" would send an email asking for help with bug
        /// </summary>
        public static string RequestUserHelpMissiveTitle = "I believe I need your help ...";

        /// <summary>
        /// Body of PermissionPopup presented to user where "YES" would send an email asking for help with bug
        /// </summary>
        public static string RequestUserHelpMissiveMessage = "My name is Ben and I am the developer of this application (or at least the part the just didn't work).  Unfortunately you have managed to trigger a bug that, for the life of me, I cannot reproduce - and therefore fix!  Would you be willing to email me so I can learn more about what just happened?";

        /// <summary>
        /// Used internally to communicate with user when perplexing exception is triggered;
        /// </summary>
        /// <param name="e">Exception thrown that prompted request for help</param>
        /// <param name="additionalInfo">Any additional info you would like shared with you </param>
        /// <param name="path">Path to file where this request was called</param>
        /// <param name="lineNumber">Linenumber of where this request was called</param>
        /// <param name="methodName">Name of method from which this request was called</param>
        /// <returns></returns>
        public static async Task RequestUserHelp(Exception e, string additionalInfo= null, [System.Runtime.CompilerServices.CallerFilePath] string path = null, [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = -1, [System.Runtime.CompilerServices.CallerMemberName] string methodName = null)
        {
            if (IsRequestUserHelpEnabled)
            {
                using (var popup = PermissionPopup.Create(RequestUserHelpMissiveTitle, RequestUserHelpMissiveMessage))
                {
                    popup.IsVisible = true;
                    await popup.WaitForPoppedAsync();
                    if (popup.PermissionState == PermissionState.Ok)
                    {
                        var info = "";
                        if (!(e is null))
                            info += "Exception Type: " + e.GetType() + "\n\nMessage: " + e.Message;
                        info += "\n\nMethod: " + methodName + "\n\nLine Number: " + lineNumber + "\n\nPath: " + path;
                        if (!string.IsNullOrWhiteSpace(additionalInfo))
                            info += "\n\nAdditionalInfo: " + additionalInfo;
                        info += "\n\nDeviceType: " + Xamarin.Essentials.DeviceInfo.DeviceType;
                        info += "\n\nIdiom: " + Xamarin.Essentials.DeviceInfo.Idiom;
                        info += "\n\nManufacturer : " + Xamarin.Essentials.DeviceInfo.Manufacturer;
                        info += "\n\nModel : " + Xamarin.Essentials.DeviceInfo.Model;
                        info += "\n\nPlatform : " + Xamarin.Essentials.DeviceInfo.Platform;
                        info += "\n\nVersion : " + Xamarin.Essentials.DeviceInfo.Version;
                        info += "\n\nVersionString : " + Xamarin.Essentials.DeviceInfo.VersionString;

                        var traceText = e?.StackTrace;
                        if (string.IsNullOrWhiteSpace(traceText))
                        {
                            try
                            {

                                var stackTrace = new System.Diagnostics.StackTrace();
                                traceText = stackTrace.ToString();
                            }
                            catch (Exception) { }
                        }

                        info += "\n\nStack Trace: " + traceText;// ?? Environment.StackTrace);
                        try
                        {
                            var message = new EmailMessage
                            {
                                Subject = "Help with bug",
                                Body = "I'm willing to help.  Below is some information about what happened.\n\n" + info,
                                To = new List<string> { RequestUserHelpEmailToAddress },
                                //Cc = ccRecipients,
                                //Bcc = bccRecipients
                            };
                            await Email.ComposeAsync(message);
                        }
                        catch (Exception)
                        {
                            using (var toast = Toast.Create("Email not available", "Email cannot be opened directly from this app ... but I still would like your help.  Could you email me at ben@buildcalc.com and include (via copy and paste into your email) the below information?  Thank you for considering this! \n\n <b>INFORMATION:</b>\n\n" + info)) { }
                        }
                    }
                }

            }
        }
    }
}
