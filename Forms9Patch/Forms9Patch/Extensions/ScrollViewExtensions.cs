using System;
using System.Threading.Tasks;

namespace Forms9Patch
{
    public static class ScrollViewExtensions
    {
        public static async Task ScrollToBottomAync(this Xamarin.Forms.ScrollView scrollView, bool animated = true)
            => await scrollView.ScrollToAsync(Math.Max(0, scrollView.ScrollY), Math.Max(0, scrollView.Content.Height - scrollView.Height), animated);

        public static async Task ScrollToRightAsync(this Xamarin.Forms.ScrollView scrollView, bool animated = true)
            => await scrollView.ScrollToAsync(Math.Max(0, scrollView.Content.Width - scrollView.Width), Math.Max(0, scrollView.ScrollY), animated);

        public static async Task ScrollToTopAsync(this Xamarin.Forms.ScrollView scrollView, bool animated = true)
            => await scrollView.ScrollToAsync(Math.Max(0, scrollView.ScrollY), 0, animated);

        public static async Task ScrollToLeftAsync(this Xamarin.Forms.ScrollView scrollView, bool animated = true)
            => await scrollView.ScrollToAsync(0, Math.Max(0, scrollView.ScrollY), animated);

        public static async Task ScrollToEndAsync(this Xamarin.Forms.ScrollView scrollView, bool animated = true)
            => await scrollView.ScrollToAsync(Math.Max(0, scrollView.Content.Width - scrollView.Width), Math.Max(0, scrollView.Content.Height - scrollView.Height), animated);

        public static async Task ScrollToStartAsync(this Xamarin.Forms.ScrollView scrollView, bool animated = true)
            => await scrollView.ScrollToAsync(0, 0, animated);
    }
}
