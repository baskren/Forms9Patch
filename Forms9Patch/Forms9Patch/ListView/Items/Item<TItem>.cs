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
			SeparatorIsVisible = other.SeparatorIsVisible;
			BackgroundColor = other.BackgroundColor;
			Source = other.Source;
		}
		#endregion


		/*
		#region Operations
		public override int GetHashCode() {
			return ID;
		}

		public bool Equals(Item<TItem> other) {
			return other != null && Value.Equals (other.Value);
		}

		public override bool Equals(object obj) {
			return Equals( obj as Item<TItem> );
		}
		#endregion
		*/
	}
}
