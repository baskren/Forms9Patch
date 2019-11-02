using Xamarin.Forms;
using System.ComponentModel;

namespace Forms9Patch
{
    [DesignTimeVisible(true)]
    /// <summary>
    /// Forms9Patch Frame layout.
    /// </summary>
    public class Frame : Forms9Patch.ContentView
    {
        #region Constructor
        static Frame()
        {
            Settings.ConfirmInitialization();
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Forms9Patch.Frame"/> class.
        /// </summary>
        public Frame()
        {
            Padding = new Thickness(20);
        }

        #endregion


        #region OnPropertyChanged
        /// <summary>
        /// Called when a property has changed
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanged(propertyName));
                return;
            }

            base.OnPropertyChanged(propertyName);

            if (propertyName == HasShadowProperty.PropertyName)
                InvalidateMeasure();
        }
        #endregion
    }
}
