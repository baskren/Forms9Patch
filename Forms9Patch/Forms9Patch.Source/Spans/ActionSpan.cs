// /*******************************************************************
//  *
//  * ActionSpan.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using System.Windows.Input;
using P42.Utils;
using Xamarin.Forms;

namespace Forms9Patch
{
	class ActionSpan : Span, ICopiable<ActionSpan>
	{
		internal const string SpanKey = "Action";

        internal const string NullId = "ActionSpanNullId";

		string _id;
		public string Id
		{
			get { return _id; }
			set
			{
				if (_id == value)
					return;
				_id = value;
				OnPropertyChanged(Key);
			}
		}

		string _href;
		public string Href
		{
			get { return _href; }
			set
			{
				if (_href == value)
					return;
				_href = value;
				OnPropertyChanged(Key);
			}
		}


		public ActionSpan(int start, int end, string id=null, string href=null) : base (start, end) {
			Key = SpanKey;
			Id = id;
			Href = href;

            if (IsEmpty())
                Id = NullId;
		}

		public ActionSpan(ActionSpan span) : this (span.Start, span.End, span.Id) {
		}

		public void PropertiesFrom(ActionSpan source)
		{
			base.PropertiesFrom(source);
			Id = source.Id;
			Href = source.Href;
		}

		public override Span Copy()
		{
			return new ActionSpan(Start, End, Id, Href);
		}

        public bool IsEmpty() => string.IsNullOrEmpty(Id) && string.IsNullOrEmpty(Href);
	}
}
