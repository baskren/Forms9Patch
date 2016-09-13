using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	public interface IItem
	{
		bool SeparatorIsVisible { get; }

		Color SeparatorColor { get; }

		double SeparatorHeight { get; }

		double SeparatorLeftIndent { get; }

		double SeparatorRightIndent { get; }

		Color CellBackgroundColor { get; }

		Color SelectedCellBackgroundColor { get; }

		AccessoryPosition AccessoryPosition { get; }

		Func<IItem, string> AccessoryText { get; }

		bool IsSelected { get; }

		object Source { get; }

		int Index { get; }
	}
}

