using System;

namespace FormsGestures
{
	/// <summary>
	/// FormsGestures Debug Helper
	/// </summary>
	public static class Debug
	{
		/// <summary>
		/// Currents the method.
		/// </summary>
		/// <returns>The method.</returns>
		/// <param name="callerName">Caller name.</param>
		internal static string CurrentMethod([System.Runtime.CompilerServices.CallerMemberName] string callerName = "") {
			return callerName;
		}
			
	}
}

