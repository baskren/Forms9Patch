using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

namespace Forms9Patch.UWP
{
    public sealed class FileImageSourceHandler : IImageSourceHandler
    {
        public async Task<Windows.UI.Xaml.Media.ImageSource> LoadImageAsync(Xamarin.Forms.ImageSource imagesource, CancellationToken cancellationToken = new CancellationToken())
        {
            WriteableBitmap bitmapimage = null;
            if (imagesource is FileImageSource filesource)
            {
                using (FileStream stream = new FileStream(filesource.File, FileMode.Open, FileAccess.Read))
                {
                    if (stream == null)
                        return null;
                    bitmapimage = await BitmapFactory.FromStream(stream);
                }
            }
            return bitmapimage;
        }
    }
}
