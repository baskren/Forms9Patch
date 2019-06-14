using System;
using System.Threading.Tasks;

namespace Forms9Patch
{
    /// <summary>
    /// Scroll helper extensions
    /// </summary>
    public static class ScrollViewExtensions
    {
        /// <summary>
        /// Scroll to bottom edge
        /// </summary>
        /// <param name="scrollView"></param>
        /// <param name="animated"></param>
        /// <returns></returns>
        public static async Task ScrollToBottomAync(this Xamarin.Forms.ScrollView scrollView, bool animated = true)
            => await scrollView.ScrollToAsync(Math.Max(0, scrollView.ScrollY), Math.Max(0, scrollView.Content.Height - scrollView.Height), animated);

        /// <summary>
        /// Scroll to right edge
        /// </summary>
        /// <param name="scrollView"></param>
        /// <param name="animated"></param>
        /// <returns></returns>
        public static async Task ScrollToRightAsync(this Xamarin.Forms.ScrollView scrollView, bool animated = true)
            => await scrollView.ScrollToAsync(Math.Max(0, scrollView.Content.Width - scrollView.Width), Math.Max(0, scrollView.ScrollY), animated);

        /// <summary>
        /// Scroll to top edge
        /// </summary>
        /// <param name="scrollView"></param>
        /// <param name="animated"></param>
        /// <returns></returns>
        public static async Task ScrollToTopAsync(this Xamarin.Forms.ScrollView scrollView, bool animated = true)
            => await scrollView.ScrollToAsync(Math.Max(0, scrollView.ScrollY), 0, animated);

        /// <summary>
        /// Scroll to left edge
        /// </summary>
        /// <param name="scrollView"></param>
        /// <param name="animated"></param>
        /// <returns></returns>
        public static async Task ScrollToLeftAsync(this Xamarin.Forms.ScrollView scrollView, bool animated = true)
            => await scrollView.ScrollToAsync(0, Math.Max(0, scrollView.ScrollY), animated);

        /// <summary>
        /// Scroll to bottom right corner
        /// </summary>
        /// <param name="scrollView"></param>
        /// <param name="animated"></param>
        /// <returns></returns>
        public static async Task ScrollToEndAsync(this Xamarin.Forms.ScrollView scrollView, bool animated = true)
            => await scrollView.ScrollToAsync(Math.Max(0, scrollView.Content.Width - scrollView.Width), Math.Max(0, scrollView.Content.Height - scrollView.Height), animated);

        /// <summary>
        /// Scroll to top left corner
        /// </summary>
        /// <param name="scrollView"></param>
        /// <param name="animated"></param>
        /// <returns></returns>
        public static async Task ScrollToStartAsync(this Xamarin.Forms.ScrollView scrollView, bool animated = true)
            => await scrollView.ScrollToAsync(0, 0, animated);
    }
}
