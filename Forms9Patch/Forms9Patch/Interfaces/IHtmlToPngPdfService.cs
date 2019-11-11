using System;
using System.Threading.Tasks;
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
		Task<HtmlToPngResult> ToPngAsync(ActivityIndicatorPopup popup, string html, string fileName);
	}

	/// <summary>
	/// Pdf done arguments.
	/// </summary>
	public class HtmlToPngResult 
	{
        /// <summary>
        /// Flags if the Result is an error;
        /// </summary>
        public bool IsError { get; private set; }


		/// <summary>
		/// Either the path to the PNG or, if IsError, an error message
		/// </summary>
		/// <value>The path.</value>
		public string Result
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.PdfDoneArgs"/> class.
		/// </summary>
		/// <param name="path">Path.</param>
		internal HtmlToPngResult(bool isError, string result)
		{
            IsError = isError;
            Result = result;
		}
	}
}
