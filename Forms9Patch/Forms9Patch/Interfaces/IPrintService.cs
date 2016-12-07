namespace Forms9Patch
{
	public interface IPrintService
	{
		void Print(Xamarin.Forms.WebView viewToPrint, string jobName);
		bool CanPrint();
	}
}
