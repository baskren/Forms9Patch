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
    public sealed class StreamImageSourceHandler : IImageSourceHandler
    {
        public async Task<Windows.UI.Xaml.Media.ImageSource> LoadImageAsync(Xamarin.Forms.ImageSource imagesource, CancellationToken cancellationToken = new CancellationToken())
        {
            WriteableBitmap bitmapimage = null;

            if (imagesource is StreamImageSource streamsource && streamsource.Stream != null)
            {
                using (Stream stream = await ((IStreamImageSource)streamsource).GetStreamAsync(cancellationToken))
                {
                    if (stream == null)
                        return null;
                    bitmapimage = await BitmapFactory.FromStream(stream);
                    //await bitmapimage.SetSourceAsync(stream.AsRandomAccessStream());
                }
            }
            return bitmapimage;
        }
    }
}
