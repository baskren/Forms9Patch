using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using UIKit;

[assembly: Dependency(typeof(Forms9Patch.iOS.OsInfoService))]
namespace Forms9Patch.iOS
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    class OsInfoService : IOsInformationService
    {
        public Version Version
        {
            get
            {
                var revision = UIDevice.CurrentDevice.SystemVersion;
                return new Version(revision);
            }
        }
    }
}
