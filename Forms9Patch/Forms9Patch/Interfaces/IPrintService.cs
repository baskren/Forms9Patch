namespace Forms9Patch
{
	/// <summary>
	/// Print service.
	/// </summary>
	public interface IPrintService
	{
		/// <summary>
		/// Print the specified viewToPrint and jobName.
		/// </summary>
		/// <param name="viewToPrint">View to print.</param>
		/// <param name="jobName">Job name.</param>
		void Print(Xamarin.Forms.WebView viewToPrint, string jobName);

		/// <summary>
		/// Cans the print.
		/// </summary>
		/// <returns><c>true</c>, if print was caned, <c>false</c> otherwise.</returns>
		bool CanPrint();
	}
}
