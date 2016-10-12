// /*******************************************************************
//  *
//  * NumeratorSpan.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
namespace Forms9Patch
{
	class NumeratorSpan: Span
	{
		internal const string SpanKey = "Numerator";

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.NumeratorSpan"/> class.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		public NumeratorSpan(int start, int end) : base(start, end) {
			Key = SpanKey;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.NumeratorSpan"/> class.
		/// </summary>
		/// <param name="span">Span.</param>
		public NumeratorSpan(NumeratorSpan span) : this(span.Start, span.End) {
		}
	}
}
