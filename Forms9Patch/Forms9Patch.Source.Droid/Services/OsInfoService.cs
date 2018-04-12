using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(Forms9Patch.Droid.OsInfoService))]
namespace Forms9Patch.Droid
{
    class OsInfoService : IOsInformationService
    {
        public Version Version
        {
            get
            {
                var revision = Android.OS.Build.VERSION.Release;
                return new Version(revision);
            }
        }
    }
}
