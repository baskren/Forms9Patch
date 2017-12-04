using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms9Patch
{
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

        public static Version Version => Service.Version;

    }
}
