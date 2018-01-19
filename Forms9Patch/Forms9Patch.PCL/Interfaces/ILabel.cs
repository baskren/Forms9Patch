using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Interface for Forms9Patch elements that have labels
    /// </summary>
    public interface ILabel : ILabelStyle, IElement
    {
        /// <summary>
        /// Plain text
        /// </summary>
        string Text { get; set; }
        /// <summary>
        /// Alternative Markup text
        /// </summary>
        string HtmlText { get; set; }

        /// <summary>
        /// gets the size of the rendered font IF it was changed per AutoFit (-1 if it was not changed)
        /// </summary>
        double FittedFontSize { get; }

        /// <summary>
        /// Allows manual override of FontSize and FittedFontSize to allow manual control of rendered font size
        /// </summary>
        double SynchronizedFontSize { get; set; }
    }
}
