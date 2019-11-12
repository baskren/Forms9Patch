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
        /// Html to PNG interface
        /// </summary>
        /// <param name="popup"></param>
        /// <param name="html"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
		Task<HtmlToPngResult> ToPngAsync(ActivityIndicatorPopup popup, string html, string fileName);
    }

    /// <summary>
    /// Result from HtmlToPnd process
    /// </summary>
	public class HtmlToPngResult
    {
        /// <summary>
        /// Flags if the Result is an error;
        /// </summary>
        public bool IsError { get; private set; }

        /// <summary>
        /// Either success or error result
        /// </summary>
		public string Result
        {
            get;
            private set;
        }

        /// <summary>
        /// Html to PNG result
        /// </summary>
        /// <param name="isError"></param>
        /// <param name="result"></param>
		internal HtmlToPngResult(bool isError, string result)
        {
            IsError = isError;
            Result = result;
        }
    }
}
