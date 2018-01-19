using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
    /// <summary>
    /// Provides information about the operating system in which the app is running
    /// </summary>
    public static class OsInfoService
    {
        static IOsInformationService _service;
        static IOsInformationService Service
        {
            get
            {
                Settings.ConfirmInitialization();
                _service = _service ?? Xamarin.Forms.DependencyService.Get<IOsInformationService>();
                if (_service == null)
                    throw new ServiceNotAvailableException("OsInfoService not available");
                return _service;
            }
        }

        /// <summary>
        /// Operating system version
        /// </summary>
        public static Version Version => Service.Version;

    }
}
