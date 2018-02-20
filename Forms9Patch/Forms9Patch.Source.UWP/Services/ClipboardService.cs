using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Windows.ApplicationModel.DataTransfer;

[assembly: Dependency(typeof(Forms9Patch.UWP.ClipboardService))]
namespace Forms9Patch.UWP
{
    public class ClipboardService : Forms9Patch.IClipboardService
    {
        public DataPackage DataPackage = new DataPackage();

        public string Value
        {
            get
            {
                DataPackageView dataPackageView = Clipboard.GetContent();
                if (dataPackageView.Contains(StandardDataFormats.Text))
                {

                }
            }
            set
            {
                //UIPasteboard.General.String = value;

            }
        }
    }
}