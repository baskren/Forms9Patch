using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Foundation;
using MobileCoreServices;
using P42.Utils;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Diagnostics;

[assembly: Dependency(typeof(Forms9Patch.iOS.DataTransferService))]
namespace Forms9Patch.iOS
{
    public class DataTransferService : Forms9Patch.IDataTransfer
    {
    }
}