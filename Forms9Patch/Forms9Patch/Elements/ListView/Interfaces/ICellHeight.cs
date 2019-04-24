using System;
using System.Collections.Generic;
using FormsGestures;
namespace Forms9Patch
{
    /// <summary>
    /// Interface to set the cell height for a cell that contains the content of a Forms9Patch ListView cell.
    /// </summary>
    public interface ICellHeight
    {
        /// <summary>
        /// Gets the height of the cell if the list HasUnevenRows=true.
        /// </summary>
        /// <value>The height of the cell.</value>
        double CellHeight { get; }
    }

    internal interface ICell_T_Height
    {
        double Height { get; set; }
    }
}
