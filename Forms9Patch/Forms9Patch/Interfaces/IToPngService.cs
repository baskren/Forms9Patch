using System;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace Forms9Patch
{
    /// <summary>
    /// Html to pdf service.
    /// </summary>
    public interface IToPngService
    {
        /// <summary>
        /// Html to PNG interface
        /// </summary>
        /// <param name="popup"></param>
        /// <param name="html"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
		Task<ToFileResult> ToPngAsync(string html, string fileName);

        /// <summary>
        /// WebView to PNG interface
        /// </summary>
        /// <param name="popup"></param>
        /// <param name="webView"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<ToFileResult> ToPngAsync(WebView webView, string fileName);
    }


}
