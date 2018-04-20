using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Haptics service.
    /// </summary>
    public static class KeyClicksService
    {
        static KeyClicksService()
        {
            Settings.ConfirmInitialization();
        }

        static IKeyClicksService _service;
        static IKeyClicksService Service
        {
            get
            {
                _service = _service ?? DependencyService.Get<IKeyClicksService>();
                if (_service == null)
                {
                    System.Diagnostics.Debug.WriteLine("KeyClicksService is not available");
                    //    throw new ServiceNotAvailableException("KeyClicksService is not available");
                }
                return _service;
            }
        }

        /// <summary>
        /// Feedback the specified effect and mode.
        /// </summary>
        /// <param name="effect">Effect.</param>
        /// <param name="mode">Mode.</param>
        public static void Feedback(HapticEffect effect, KeyClicks mode = KeyClicks.Default)
        {
            if (mode == KeyClicks.Off)
                return;
            Service?.Feedback(effect, mode);
        }

    }
}
