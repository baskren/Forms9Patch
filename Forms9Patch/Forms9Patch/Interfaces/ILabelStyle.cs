using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Interface for Forms9Patch Label styling
    /// </summary>
    public interface ILabelStyle : IFontElement
    {
        /// <summary>
        /// Color of label's text
        /// </summary>
        Color TextColor { get; set; }

        /// <summary>
        /// Label's horizontal alignment
        /// </summary>
        TextAlignment HorizontalTextAlignment { get; set; }
        /// <summary>
        /// Labe's vertical alignment
        /// </summary>
        TextAlignment VerticalTextAlignment { get; set; }
        /// <summary>
        /// Label's line break mode
        /// </summary>
        LineBreakMode LineBreakMode { get; set; }
        /// <summary>
        /// Label's AutoFit algorithm
        /// </summary>
        AutoFit AutoFit { get; set; }
        /// <summary>
        /// Manual override for number of lines to AutoFit or truncate
        /// </summary>
        int Lines { get; set; }
        /// <summary>
        /// Lower limit of AutoFit font size
        /// </summary>
        double MinFontSize { get; set; }

    }
}
