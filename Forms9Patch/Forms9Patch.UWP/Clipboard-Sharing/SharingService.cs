using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: Dependency(typeof(Forms9Patch.UWP.SharingService))]
namespace Forms9Patch.UWP
{
    public class SharingService : Forms9Patch.ISharingService
    {
        Forms9Patch.MimeItemCollection _mimeItemCollection;

        public SharingService()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if (_mimeItemCollection != null && _mimeItemCollection.Items.Count > 0)
            {


                DataRequest request = args.Request;

                request.Data.Source(_mimeItemCollection);
                //request.Data.SetText("pizza");

                System.Diagnostics.Debug.WriteLine("DataTransferManager_DataRequested: complete");
            }
        }


        public void Share(Forms9Patch.MimeItemCollection mimeItemCollection, VisualElement target)
        {
            _mimeItemCollection = mimeItemCollection;
            if (_mimeItemCollection.Items.Count>0)
                DataTransferManager.ShowShareUI();
        }
    }
}