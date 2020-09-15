using Xamarin.Forms;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// DO NOT USE: Used by Forms9Patch.ListView as a placeholder for a cell when being dragged
    /// </summary>
    [DesignTimeVisible(true)]
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    class BlankCellView : BaseCellView
    {
        readonly BoxView _boxView = new BoxView
        {
            HeightRequest = 60,
            BackgroundColor = Color.Transparent
        };

        /// <summary>
        /// DO NOT USE: Initializes a new instance of the <see cref="T:Forms9Patch.BlankCellView"/> class.
        /// </summary>
        public BlankCellView()
        {
            ContentView = _boxView;
            //TODO: bind _boxView.HeightRequest
        }

        /// <summary>
        /// Triggered by a change in a property
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                base.OnPropertyChanged(propertyName);

                if (propertyName == HeightRequestProperty.PropertyName)
                    _boxView.HeightRequest = HeightRequest;
            });
        }
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

