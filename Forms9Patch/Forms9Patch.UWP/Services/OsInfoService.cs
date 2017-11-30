using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(Forms9Patch.UWP.OsInfoService))]
namespace Forms9Patch.UWP
{
    class OsInfoService : IOsInformationService
    {
        public Version Version
        {
            get
            {
                string deviceFamilyVersion = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
                ulong version = ulong.Parse(deviceFamilyVersion);
                ulong major = (version & 0xFFFF000000000000L) >> 48;
                ulong minor = (version & 0x0000FFFF00000000L) >> 32;
                ulong build = (version & 0x00000000FFFF0000L) >> 16;
                ulong revision = (version & 0x000000000000FFFFL);
                var osVersion = $"{major}.{minor}.{build}.{revision}";
                return new Version(osVersion);
            }
        }
    }
}
