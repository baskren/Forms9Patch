using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Forms9Patch.iOS
{
	static class Debug
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static string GetCurrentMethod ()
		{
			StackTrace st = new StackTrace ();
			StackFrame sf = st.GetFrame (1);

			return sf.GetMethod().Name;
		}
	}
}

