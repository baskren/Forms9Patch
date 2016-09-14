using System;
namespace Forms9Patch
{
	/// <summary>
	/// Accessory position.
	/// </summary>
	public enum AccessoryPosition
	{
		/// <summary>
		/// No accessories will be shown
		/// </summary>
		None=0,
		/// <summary>
		/// Accessory will be on the starting (left) side if the AccessText != null
		/// </summary>
		Start=1,
		/// <summary>
		/// Accessory will be on the ending (right) side if the AccessoryText != null
		/// </summary>
		End=2,
	}
}

