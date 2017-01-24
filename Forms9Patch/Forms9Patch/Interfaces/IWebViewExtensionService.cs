using PCL.Utils;
namespace Forms9Patch
{
	/// <summary>
	/// Print service.
	/// </summary>
	public interface IWebViewExtensionService
	{
		/// <summary>
		/// Print the specified viewToPrint and jobName.
		/// </summary>
		/// <param name="viewToPrint">View to print.</param>
		/// <param name="jobName">Job name.</param>
		void Print(Xamarin.Forms.WebView webView, string jobName);

		/// <summary>
		/// Cans the print.
		/// </summary>
		/// <returns><c>true</c>, if print was caned, <c>false</c> otherwise.</returns>
		bool CanPrint();

	}
}
