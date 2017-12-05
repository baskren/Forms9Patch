
using System.Collections.Generic;
using System.Reflection;

namespace Forms9Patch
{
	/// <summary>
	/// Interface for platform settings
	/// </summary>
	public interface INativeSettings
	{
		/// <summary>
		/// Gets a value indicating whether this instance is licensed.
		/// </summary>
		/// <value><c>true</c> if this instance is licensed; otherwise, <c>false</c>.</value>
		bool IsLicensed {
			get;
		}

        List<Assembly> IncludedAssemblies { get; }
    }
}

