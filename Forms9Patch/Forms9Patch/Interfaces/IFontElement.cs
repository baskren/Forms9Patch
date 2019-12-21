
namespace Forms9Patch
{
	/// <summary>
	/// Interface for Forms9Patch elements with Fonts.
	/// </summary>
	public interface IFontElement
	{
		/// <summary>
		/// Gets the font attributes.
		/// </summary>
		/// <value>The font attributes.</value>
		Xamarin.Forms.FontAttributes FontAttributes { get; }

		/// <summary>
		/// Gets the font family.
		/// </summary>
		/// <value>The font family.</value>
		string FontFamily { get; }

		/// <summary>
		/// Gets the size of the font.
		/// </summary>
		/// <value>The size of the font.</value>
		double FontSize { get; }
	}
}

