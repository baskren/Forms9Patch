using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using PCL.Utils;

namespace Forms9Patch.UWP
{
    public sealed class UriImageSourceHandler : IImageSourceHandler
    {
        public async Task<Windows.UI.Xaml.Media.ImageSource> LoadImageAsync(Xamarin.Forms.ImageSource imagesource, CancellationToken cancellationToken = new CancellationToken())
        {
            var uriImageSource = imagesource as Xamarin.Forms.UriImageSource;
            if (uriImageSource?.Uri == null)
                return null;

            //Stream streamImage = await uriImageSource.GetStreamAsync(cancellationToken);
            var getStreamAsyncMethod = uriImageSource.GetType().GetMethodInfo("GetStreamAsync");
            var streamImage = await (Task<Stream>)getStreamAsyncMethod.Invoke(uriImageSource, new object[] { cancellationToken});
            if (streamImage == null || !streamImage.CanRead)
            {
                return null;
            }
            


            using (IRandomAccessStream stream = streamImage.AsRandomAccessStream())
            {
                try
                {
                    if (stream == null)
                        return null;
                   var  bitmapimage = await BitmapFactory.FromStream(stream);
                    return bitmapimage;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Image Loading", $"{nameof(UriImageSourceHandler)} could not load {uriImageSource.Uri}: {ex}");

                    // According to https://msdn.microsoft.com/library/windows/apps/jj191522
                    // this can happen if the image data is bad or the app is close to its 
                    // memory limit
                    return null;
                }
            }
        }
    }
}
