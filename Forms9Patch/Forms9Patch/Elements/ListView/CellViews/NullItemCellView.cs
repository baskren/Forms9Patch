using Xamarin.Forms;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// Null cell view: DO NOT USE.  Used internally by Forms9Patch.ListView to display null items in a ListView
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    class NullItemCellView : BaseCellView
    {

        readonly ColorGradientBox _upperEdge = new ColorGradientBox
        {
            StartColor = Color.FromRgba(0, 0, 0, 0.24),
            EndColor = Color.FromRgba(0, 0, 0, 0),
            VerticalOptions = LayoutOptions.Start,
            HeightRequest = 6
        };

        readonly ColorGradientBox _lowerEdge = new ColorGradientBox
        {
            StartColor = Color.FromRgba(0, 0, 0, 0),
            EndColor = Color.FromRgba(0, 0, 0, 0.12),
            VerticalOptions = LayoutOptions.EndAndExpand,
            HeightRequest = 2
        };

        readonly Xamarin.Forms.StackLayout _layout = new Xamarin.Forms.StackLayout
        {
            HeightRequest = 10,
            Spacing = 0,
            Orientation = StackOrientation.Vertical,
            BackgroundColor = Color.Transparent
        };

        /// <summary>
        /// DO NOT USE: Initializes a new instance of the <see cref="T:Forms9Patch.NullCellView"/> class.
        /// </summary>
        public NullItemCellView()
        {
            //var filler = new BoxView ();
            //filler.BindingContext = this;
            //filler.SetBinding (VisualElement.HeightRequestProperty,"HeightRequest",BindingMode.OneWay,new RequestedHeightConverter());
            //System.Diagnostics.Debug.WriteLine ("\t\t\t\tcreating NullCellView");
            //_layout.SetBinding (HeightRequestProperty, "RequestedHeight");
            _layout.Children.Add(_upperEdge);
            _layout.Children.Add(_lowerEdge);
            //SeparatorHeight = 0;
            ContentView = _layout;
            BackgroundColor = Color.Transparent;
        }

        /*
		protected override void OnPropertyChanging(string propertyName = null)
		{
			base.OnPropertyChanging(propertyName);
			if (propertyName == BindingContextProperty.PropertyName && BindingContext != null)
			{
				var nullItemWrapper = BindingContext as NullItemWrapper;
				if (nullItemWrapper != null)
					nullItemWrapper.PropertyChanged -= NullItemPropertyChanged;
			}
		}

		/// <summary>
		/// Triggered by a BindingContextChange
		/// </summary>
		protected override void OnBindingContextChanged ()
		{
			_layout.BindingContext = BindingContext;
			base.OnBindingContextChanged ();
			var nullItemWrapper = BindingContext as NullItemWrapper;
			if (nullItemWrapper!=null)
				nullItemWrapper.PropertyChanged += NullItemPropertyChanged;
		}

		void NullItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			_layout.HeightRequest = ((NullItemWrapper)BindingContext).RequestedHeight;
		}
        */
    }

    /*
	public class RequestedHeightConverter : IValueConverter {
		#region IValueConverter implementation
		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (double)value - 7;
		}
		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (double)value + 7;
		}
		#endregion
		
	}
	*/
}

