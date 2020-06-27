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
        /// <param name="html"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
		Task<ToFileResult> ToPngAsync(string html, string fileName, int width);

        /// <summary>
        /// WebView to PNG interface
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<ToFileResult> ToPngAsync(WebView webView, string fileName, int width);
    }


}
