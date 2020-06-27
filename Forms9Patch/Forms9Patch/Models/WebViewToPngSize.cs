using System;
namespace Forms9Patch
{
    class WebViewToPngSize : PageSize
    {
        public Xamarin.Forms.Rectangle Bounds { get; set; }

        public WebViewToPngSize(int width, Xamarin.Forms.Rectangle bounds = default)
        {
            Bounds = bounds;
        }
    }
}
