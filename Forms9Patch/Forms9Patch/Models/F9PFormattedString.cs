using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Forms9Patch
{
    /// <summary>
    /// MarkdownLabel Formatted string.
    /// </summary>
    [Xamarin.Forms.ContentProperty(nameof(Text))]
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    abstract class F9PFormattedString : INotifyPropertyChanged //, IDisposable //, IEquatable<F9PFormattedString>
    {
        internal ObservableCollection<Span> _spans;

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Forms9Patch.F9PFormattedString"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Forms9Patch.F9PFormattedString"/>.</returns>
        public override string ToString()
        {
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
        public string Text
        {
            get => _string;
            set
            {
                _string = value;
                OnPropertyChanged("String");
            }
        }


        #region Construction / Disposal
        /// <summary>
        /// Initializes a new instance of the <see cref="Forms9Patch.F9PFormattedString"/> class.
        /// </summary>
        protected F9PFormattedString()
        {
            Text = "";
            _spans = new ObservableCollection<Span>();
            _spans.CollectionChanged += OnCollectionChanged;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Forms9Patch.F9PFormattedString"/> class.
        /// </summary>
        /// <param name="s">S.</param>
        protected F9PFormattedString(string s) : this()
        {
            Text = s;
        }


        public F9PFormattedString(F9PFormattedString other)
        {
            Text = other.Text;
            if (other._spans != null && other._spans.Count > 0)
            {
                _spans = new ObservableCollection<Span>(other._spans);
                _spans.CollectionChanged += OnCollectionChanged;
            }
        }

        /*
        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                _spans.CollectionChanged -= OnCollectionChanged;
                //var spans = _spans.ToArray();
                _spans.Clear();
                //foreach (var span in spans)
                //    span.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        */
        #endregion


        #region Operators
        /// <param name="formatted">Formatted.</param>
        public static explicit operator string(F9PFormattedString formatted)
        {
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


        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                var enumerator = e.OldItems.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Current is Span span)
                            span.PropertyChanged -= OnItemPropertyChanged;
                    }
                }
                finally
                {
                    var disposable = enumerator as IDisposable;
                    disposable?.Dispose();
                }
            }
            if (e.NewItems != null)
            {
                var enumerator = e.NewItems.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Current is Span span2)
                            span2.PropertyChanged += OnItemPropertyChanged;
                    }
                }
                finally
                {
                    var disposable = enumerator as IDisposable;
                    disposable?.Dispose();
                }
            }
            OnPropertyChanged("Spans");
        }

        void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Spans");
        }

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        /*  This is not yet working.  ConnectionCalc, change CLT layers does not update MmThick;
        #region IEquality
        public bool Equals(F9PFormattedString other)
        {
            if (other is null)
                return false;
            if (_spans is null != other._spans is null)
                return false;
            if (_spans is null && other._spans is null)
                return Text == other.Text;
            if (_spans.Count != other._spans.Count)
                return false;
            for (int i = 0; i < _spans.Count; i++)
                if (_spans[i] != other._spans[i])
                    return false;
            return true;
        }

        public override bool Equals(object obj)
            => obj is F9PFormattedString other ? Equals(other) : false;

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = GetType().GetHashCode();
                foreach (var span in _spans)
                    hashCode += span.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(F9PFormattedString a, F9PFormattedString b)
            => a.Equals(b);

        public static bool operator !=(F9PFormattedString a, F9PFormattedString b)
            => !a.Equals(b);

        #endregion
        */
    }

}

