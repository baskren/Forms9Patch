using System;

namespace Forms9Patch
{
    /// <summary>
    /// Menu item.
    /// </summary>
	public interface IMenuItem
    {
        /// <summary>
        /// Image for segment's icon (alternative to IconText)
        /// </summary>
        Forms9Patch.Image IconImage { get; set; }

        /// <summary>
        /// HtmlText for segment's icon (alternative to IconImage)
        /// </summary>
        string IconText { get; set; }

        /// <summary>
        /// Segment's text
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Segment's text with Html markup
        /// </summary>
        string HtmlText { get; set; }
    }
}