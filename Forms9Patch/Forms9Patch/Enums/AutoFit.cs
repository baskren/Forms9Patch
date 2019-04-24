// /*******************************************************************
//  *
//  * LabelFill.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
namespace Forms9Patch
{
    /// <summary>
    /// OBSOLETE: USE AutoFit
    /// </summary>
    [Obsolete("Use AutoFit instead.")]
    public enum LabelFit
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        None = AutoFit.None,
        Width = AutoFit.Width,
        Lines = AutoFit.Lines
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// Label fit options.
    /// </summary>
    public enum AutoFit
	{
		/// <summary>
		/// Perform no auto fit
		/// </summary>
		None,
		/// <summary>
		/// Shrink font from FontSize until MinFontSize to what is required to make the text fit within Lines
		/// </summary>
		Width,
		/// <summary>
		/// Scale font until Lines of text will fill imposed height (either by RequestHeight or parent view) of label.
		/// </summary>
		Lines,
	}
}

