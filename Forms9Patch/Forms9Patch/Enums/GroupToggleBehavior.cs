using System;

namespace Forms9Patch
{
	/// <summary>
	/// What happens when a segment is selected
	/// </summary>
	public enum GroupToggleBehavior
	{
		/// <summary>
		/// No buttons are Toggle
		/// </summary>
		None,
		/// <summary>
		/// Only one button can be in the selected state
		/// </summary>
		Radio,
		/// <summary>
		/// None, any or all the buttons can be in the selected state
		/// </summary>
		Multiselect,
	}

	/// <summary>
    /// Extensions to Forms9Patch.GroupToggleBehavior
    /// </summary>
	public static class GroupToggleBehaviorExtensions
    {
		/// <summary>
        /// Converts a Forms9Patch.GroupToggleBehavior to a Xamarin.Forms.SelectionMode
        /// </summary>
        /// <param name="behavior"></param>
        /// <returns></returns>
		public static Xamarin.Forms.SelectionMode ToXfSelectionMode(this GroupToggleBehavior behavior)
        {
			if (behavior == GroupToggleBehavior.None)
				return Xamarin.Forms.SelectionMode.None;
			if (behavior == GroupToggleBehavior.Radio)
				return Xamarin.Forms.SelectionMode.Single;
			return Xamarin.Forms.SelectionMode.Multiple;
        }
    }
}

