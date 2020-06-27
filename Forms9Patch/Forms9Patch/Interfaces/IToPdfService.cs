using System;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace Forms9Patch
{
    /// <summary>
    /// Html to pdf service.
    /// </summary>
    public interface IToPdfService
    {
        /// <summary>
        /// Html to PNG interface
        /// </summary>
        /// <param name="html"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
		Task<ToFileResult> ToPdfAsync(string html, string fileName, PageSize pageSize, PageMargin margin);

        /// <summary>
        /// WebView to PNG interface
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<ToFileResult> ToPdfAsync(WebView webView, string fileName, PageSize pageSize, PageMargin margin);

        /// <summary>
        /// Determines if PDF printing is available on this platform;
        /// </summary>
        bool IsAvailable { get; }
    }

}
