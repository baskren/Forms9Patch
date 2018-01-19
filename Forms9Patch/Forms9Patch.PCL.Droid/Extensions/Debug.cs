using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Forms9Patch.Droid
{
	internal static class Debug
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static string GetCurrentMethod ()
		{
			var st = new StackTrace ();
			StackFrame sf = st.GetFrame (1);

			return sf.GetMethod().Name;
		}
	}
}

