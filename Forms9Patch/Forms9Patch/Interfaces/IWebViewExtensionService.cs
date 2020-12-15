using System;
using System.Threading.Tasks;
using P42.Utils;
namespace Forms9Patch
{
    /// <summary>
    /// Print service.
    /// </summary>
    public interface IPrintService
    {
        /// <summary>
        /// Print the specified webView and jobName.
        /// </summary>
        /// <param name="webView">Web view.</param>
        /// <param name="jobName">Job name.</param>
        /// <param name="failAction">What to do if the method fails</param>
        Task PrintAsync(Xamarin.Forms.WebView webView, string jobName, FailAction failAction = FailAction.ShowAlert);

        /// <summary>
        /// Print the specified HTML with jobName
        /// </summary>
        /// <param name="html"></param>
        /// <param name="jobName"></param>
        /// <param name="failAction">What to do if the method fails</param>
        Task PrintAsync(string html, string jobName, FailAction failAction = FailAction.ShowAlert);

        /// <summary>
        /// Cans the print.
        /// </summary>
        /// <returns><c>true</c>, if print was caned, <c>false</c> otherwise.</returns>
        bool CanPrint();

    }
}
