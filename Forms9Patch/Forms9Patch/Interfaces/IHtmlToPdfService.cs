using System;
using PCLStorage;
using Xamarin.Forms;
namespace Forms9Patch
{
	/// <summary>
	/// Html to pdf service.
	/// </summary>
	public interface IHtmlToPdfService
	{
		/// <summary>
		/// Tos the pdf.
		/// </summary>
		/// <param name="html">Html.</param>
		/// <param name="folder">Folder.</param>
		/// <param name="fileName">File name.</param>
		/// <param name="onComplete">On complete.</param>
		void ToPdf(string html, Size size, string fileName, Action<string> onComplete);
	}

	/// <summary>
	/// Pdf done arguments.
	/// </summary>
	public class PdfDoneArgs : EventArgs
	{
		/// <summary>
		/// Gets the file.
		/// </summary>
		/// <value>The file.</value>
		public String Path
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.PdfDoneArgs"/> class.
		/// </summary>
		/// <param name="file">File.</param>
		public PdfDoneArgs(string path)
		{
			Path = path;
		}
	}
}
