// /*******************************************************************
//  *
//  * HashSetExt.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using System.Collections.Generic;
namespace Forms9PatchAuth
{
	public static class HashSetExt
	{
		public static string Description<T>(this IEnumerable<T> hash)
		{
			if (hash == null)
				return null;
			return string.Join(",", hash);
		}
	}
}
