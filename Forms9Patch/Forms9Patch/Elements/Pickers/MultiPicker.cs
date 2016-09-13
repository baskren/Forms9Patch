using System;
using System.Collections;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Multi picker.
	/// </summary>
	public class MultiPicker : SinglePicker
	{
		#region Properties
		#endregion


		#region Fields
		#endregion


		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.MultiPicker"/> class.
		/// </summary>
		public MultiPicker()
		{
			_lowerGradient.StartColor = _overlayColor.WithAlpha(0);
			_upperGradient.EndColor = _overlayColor.WithAlpha(0);
			_basePicker.SelectBy = SelectBy.Default;
			_absLayout.Children.Remove(_lowerEdge);
			_absLayout.Children.Remove(_upperEdge);
			_basePicker.AccessoryPosition = AccessoryPosition.Start;
			_basePicker.GroupToggleBehavior = GroupToggleBehavior.Multiselect;
			_basePicker.AccessoryText = (IItem arg) => arg.IsSelected ? "✓" : null;
		}
		#endregion


		#region change management
		#endregion
	}
}

