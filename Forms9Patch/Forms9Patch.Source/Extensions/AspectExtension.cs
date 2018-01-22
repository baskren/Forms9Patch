using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// Forms9Patch Aspect extension.
	/// </summary>
	public static class AspectExtension
	{
		/// <summary>
		/// Converts Xamarin.Forms.Aspect to Forms9Patch.Fill
		/// </summary>
		/// <returns>The Forms9Patch fill.</returns>
		/// <param name="aspect">Xamarin.Forms.Aspect</param>
		public static Fill ToF9pFill(this Aspect aspect) {
			switch (aspect) {
			case Aspect.AspectFill:
				return Fill.AspectFill;
			case Aspect.Fill:
				return Fill.Fill;
			default:
				return Fill.AspectFit;
			}
		}
	}
}

