//using System;


namespace Forms9Patch
{
	/// <summary>
	/// Display metrics.
	/// </summary>
	public static class Display
	{
		#region Extension Static Properties
		static float _density = 160;
		/// <summary>
		/// The density (resolution) of the screen (dpi)
		/// </summary>
		/// <value>screen density (dpi)</value>
		public static float Density {
			get { return _density;}
			set { _density = value; }
		}

		static float _scale = 1;
		/// <summary>
		/// The scale (relative to 160 dpi) of the screen
		/// </summary>
		/// <value>screen scale (1x=160dpi)</value>
		public static float Scale {
			get { return _scale; }
			set { _scale = value; }
		}

		static float _width = 320;
		/// <summary>
		/// The width (pixels) of the screen
		/// </summary>
		/// <value>screen width (pixels)</value>
		public static float Width {
			get { return _width; }
			set { _width = value; }
		}

		static float _height = 640;
		/// <summary>
		/// The hieght (pixels) of the screen
		/// </summary>
		/// <value>screen height (pixels)</value>
		public static float Height {
			get { return _height; }
			set { _height = value; }
		}
		#endregion

	}
}

