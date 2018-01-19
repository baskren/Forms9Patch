using System;
using Xamarin.Forms;
namespace Forms9Patch
{
	/// <summary>
	/// Html to pdf service.
	/// </summary>
	public interface IHtmlToPngPdfService
	{
		/// <summary>
		/// Tos the png.
		/// </summary>
		/// <param name="html">Html.</param>
		/// <param name="fileName">File name.</param>
		/// <param name="onComplete">On complete.</param>
		void ToPng(string html, string fileName, Action<string> onComplete);
	}

	/// <summary>
	/// Pdf done arguments.
	/// </summary>
	public class AttachmentDoneArgs : EventArgs
	{
		/// <summary>
		/// Gets the path.
		/// </summary>
		/// <value>The path.</value>
		public string Path
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.PdfDoneArgs"/> class.
		/// </summary>
		/// <param name="path">Path.</param>
		public AttachmentDoneArgs(string path)
		{
			Path = path;
		}
	}
}
