using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(Forms9Patch.Droid.OsInfoService))]
namespace Forms9Patch.Droid
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    class OsInfoService : IOsInformationService
    {
        public Version Version
        {
            get
            {
                var revision = Android.OS.Build.VERSION.Release;
                int count = revision.Count(f => f == '.');
                if (count == 0)
                    revision += ".0.0";
                if (count == 1)
                    revision += ".0";

                return new Version(revision);
            }
        }
    }
}
