// /*******************************************************************
//  *
//  * StatusBarService.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    internal static class StatusBarService
    {
        static StatusBarService()
        {
            Settings.ConfirmInitialization();
        }

        static IStatusBarService _service;
        static IStatusBarService Service
        {
            get
            {
                _service = _service ?? Xamarin.Forms.DependencyService.Get<IStatusBarService>();
                if (_service == null)
                {
                    System.Diagnostics.Debug.WriteLine("StatusBarService is not available");
                    System.Console.WriteLine("StatusBarService is not available");
                    //throw new ServiceNotAvailableException("StatusBarService is not available");
                }
                return _service;
            }
        }


        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public static double Height
        {
            get
            {
                if (Service == null)
                {
                    //if (Device.OS == TargetPlatform.iOS)
                    if (Device.RuntimePlatform == Device.iOS)
                        return 20;
                    return 0;
                }
                //if (!IsVisible)
                //	return 0;
                return Service.Height;
            }
        }

        public static bool IsVisible
        {
            get
            {
                if (Service == null)
                    return true;
                return Service.IsVisible;
            }
        }
    }
}
