using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
//using Windows.ApplicationModel.DataTransfer;

[assembly: Dependency(typeof(Forms9Patch.UWP.ClipboardService))]
namespace Forms9Patch.UWP
{
    public class ClipboardService : Forms9Patch.IClipboardService
    {
        public Windows.ApplicationModel.DataTransfer.DataPackage DataPackage = new Windows.ApplicationModel.DataTransfer.DataPackage();

        public ClipboardData Data
        {
            get
            {
                var result = new ClipboardData();
                var dataPackageView = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();
                if (dataPackageView.Contains(Windows.ApplicationModel.DataTransfer.StandardDataFormats.Text))
                    result.PlainText = dataPackageView.GetText();
                if (dataPackageView.Contains(Windows.ApplicationModel.DataTransfer.StandardDataFormats.Html))
                    result.HtmlText = dataPackageView.GetHtmlText();
                return result;
            }
            set
            {
                if (value == null)
                    return;
                var dataPackage = new Windows.ApplicationModel.DataTransfer.DataPackage();
                if (!string.IsNullOrEmpty(value.PlainText))
                    dataPackage.SetText(value.PlainText);
                if (!string.IsNullOrEmpty(value.HtmlText))
                    dataPackage.SetHtmlFormat(value.HtmlText);
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
            }
        }
    }

    static class DataPackageViewExtensions
    {
        public static string GetText(this Windows.ApplicationModel.DataTransfer.DataPackageView dpv)
        {
            var task = Task.Run(async () =>
            {
                var result = await dpv.GetTextAsync();
                return result;
            });
            return task.Result;
        }

        public static string GetHtmlText(this Windows.ApplicationModel.DataTransfer.DataPackageView dpv)
        {
            var task = Task.Run(async () =>
            {
                var result = await dpv.GetHtmlFormatAsync();
                return result;
            });
            return task.Result;
        }


    }
}