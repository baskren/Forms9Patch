// /*******************************************************************
//  *
//  * DenominatorSpan.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using PCL.Utils;
namespace Forms9Patch
{
	class DenominatorSpan : Span, ICopiable<DenominatorSpan>
	{
		internal const string SpanKey = "Denominator";

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.DenominatorSpan"/> class.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		public DenominatorSpan(int start, int end) : base (start, end) {
			Key = SpanKey;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.DenominatorSpan"/> class.
		/// </summary>
		/// <param name="span">Span.</param>
		public DenominatorSpan(DenominatorSpan span) : this (span.Start, span.End) {
		}

		public void PropertiesFrom(DenominatorSpan source)
		{
			base.PropertiesFrom(source);
		}

		public override Span Copy()
		{
			return new DenominatorSpan(Start, End);
		}
	}
}
