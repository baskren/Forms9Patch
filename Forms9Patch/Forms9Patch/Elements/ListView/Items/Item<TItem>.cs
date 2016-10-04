using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	class Item<TItem> : Item {

		#region Properties
		//public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(TItem),  typeof(Item<TItem>), default(TItem));
		public new TItem Source {
			get { return (TItem)GetValue(SourceProperty); }
			set { 
				if (debugProperties) System.Diagnostics.Debug.WriteLine ("Value: update from ["+(TItem)GetValue(SourceProperty)+"] to [" + value + "]");
				//if (!this.Value.Equals(value))
				if (!value.Equals(Source))
					SetValue(SourceProperty, value);
			}
		}
		#endregion


		#region Convenience
		internal void ShallowCopy(Item<TItem> other) {
			SeparatorColor = other.SeparatorColor;
			SeparatorLeftIndent = other.SeparatorLeftIndent;
			SeparatorRightIndent = other.SeparatorRightIndent;
			SeparatorIsVisible = other.SeparatorIsVisible;
			CellBackgroundColor = other.CellBackgroundColor;
			SelectedCellBackgroundColor = other.SelectedCellBackgroundColor;
			StartAccessory = other.StartAccessory;
			EndAccessory = other.EndAccessory;
			Source = other.Source;
		}
		#endregion

	}
}
