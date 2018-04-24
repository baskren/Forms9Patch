using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Forms9Patch
{
	/// <summary>
	/// MarkdownLabel Formatted string.
	/// </summary>
	[Xamarin.Forms.ContentProperty ("Text")]
	abstract class F9PFormattedString : INotifyPropertyChanged
	{
		internal ObservableCollection<Span> _spans; 

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Forms9Patch.F9PFormattedString"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Forms9Patch.F9PFormattedString"/>.</returns>
		public override string ToString() {
			return Text;
		}

		internal bool ContainsActionSpan
		{
			get
			{
				foreach (var span in _spans)
					if (span is ActionSpan)
						return true;
				return false;
			}
		}
			
		internal string _string;
		/// <summary>
		/// Gets or sets source text.
		/// </summary>
		/// <value>The text.</value>
		public string Text {
			get => _string; 
			set {
				_string = value;
				OnPropertyChanged ("String");
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.F9PFormattedString"/> class.
		/// </summary>
		protected F9PFormattedString() {
			Text = "";
			_spans = new ObservableCollection<Span> ();
			_spans.CollectionChanged += OnCollectionChanged;

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Forms9Patch.F9PFormattedString"/> class.
		/// </summary>
		/// <param name="s">S.</param>
		protected F9PFormattedString(string s) : this() {
			Text = s;
		}

		#region Operators
		/// <param name="formatted">Formatted.</param>
		public static explicit operator string (F9PFormattedString formatted) {
			return formatted?.Text;
		}

		/*
		public static implicit operator BaseFormattedString (string text) {
			return new BaseFormattedString (text);
		}

		public static explicit operator FormattedString (Xamarin.Forms.FormattedString formsFormattedString) {
			var result = new FormattedString (formsFormattedString);
			return result;
		}
		*/
		#endregion


		#region INotifyPropertyChanged implementation

		/// <summary>
		/// Occurs when property changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;


		void OnCollectionChanged (object sender, NotifyCollectionChangedEventArgs e) {
			if (e.OldItems != null) {
				IEnumerator enumerator = e.OldItems.GetEnumerator ();
				try {
					while (enumerator.MoveNext ()) {
						var span = enumerator.Current as Span;
						if (span != null) {
							span.PropertyChanged -= OnItemPropertyChanged;
						}
					}
				}
				finally {
					var disposable = enumerator as IDisposable;
					disposable?.Dispose ();
				}
			}
			if (e.NewItems != null) {
				IEnumerator enumerator = e.NewItems.GetEnumerator ();
				try {
					while (enumerator.MoveNext ()) {
						var span2 = enumerator.Current as Span;
						if (span2 != null) {
							span2.PropertyChanged += OnItemPropertyChanged;
						}
					}
				}
				finally {
					var disposable = enumerator as IDisposable;
					disposable?.Dispose ();
				}
			}
			OnPropertyChanged ("Spans");
		}

		void OnItemPropertyChanged (object sender, PropertyChangedEventArgs e) {
			OnPropertyChanged ("Spans");
		}

		/// <summary>
		/// Raises the property changed event.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected virtual void OnPropertyChanged (string propertyName = null) {
			PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
		}


		#endregion




	}

}

