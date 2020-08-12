using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Interface used to queary current state of Forms9Patch.ListView cells and the ItemSource object bound to the cells view.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    internal interface IItemWrapper
    {
        /// <summary>
        /// The Forms9Patch.ListView hosting this cell
        /// </summary>
        ListView ListView { get; }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        GroupWrapper Parent { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Forms9Patch.IItemWrapper"/> is last item in its parent group.
        /// </summary>
        /// <value><c>true</c> if is last item; otherwise, <c>false</c>.</value>
        bool IsLastItem { get; }

        /// <summary>
        /// Gets the separator visibilty.
        /// </summary>
        /// <value>The separator visibilty.</value>
        Xamarin.Forms.SeparatorVisibility SeparatorVisibility { get; }

        /// <summary>
        /// Gets the color of this cell's separator.
        /// </summary>
        /// <value>The color of the separator.</value>
        Color SeparatorColor { get; }

        /// <summary>
        /// Gets the height of this cell's separator.
        /// </summary>
        /// <value>The height of the separator.</value>
        double RequestedSeparatorHeight { get; }

        /// <summary>
        /// Gets the separator left indent for this cell.
        /// </summary>
        /// <value>The separator left indent.</value>
        double SeparatorLeftIndent { get; }

        /// <summary>
        /// Gets the separator right indent for this cell.
        /// </summary>
        /// <value>The separator right indent.</value>
        double SeparatorRightIndent { get; }



        /// <summary>
        /// Gets the unselected background color for this cell.
        /// </summary>
        /// <value>The color of the cell background.</value>
        Color CellBackgroundColor { get; }

        /// <summary>
        /// Gets the selected background color for this cell.
        /// </summary>
        /// <value>The color of the selected cell background.</value>
        Color SelectedCellBackgroundColor { get; }

        /// <summary>
        /// Gets a value indicating whether this cell is selected.
        /// </summary>
        /// <value><c>true</c> if is selected; otherwise, <c>false</c>.</value>
        bool IsSelected { get; }

        /// <summary>
        /// Gets the ListView's ItemsSource object that is bound to this cell's View
        /// </summary>
        /// <value>The source.</value>
        object Source { get; }

        /// <summary>
        /// Gets the index (within the current group, if any) of this cell.
        /// </summary>
        /// <value>The index.</value>
        int Index { get; }

        /// <summary>
        /// Gets the cell view that is bound to the item.
        /// </summary>
        /// <value>The cell view.</value>
        View CellView { get; }



        /// <summary>
        /// Gets the global height of the row.
        /// </summary>
        /// <value>The height of the row.</value>
        double RequestedRowHeight { get; }

        /// <summary>
        /// Used to record what row height was actually used for cell's height request (takes into account BaseCellView.ContentView.CellHeight value)
        /// </summary>
        double RenderedRowHeight { get; set; }

        double BestGuessItemRowHeight();
    }
}

