using Xamarin.Forms;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch Frame layout.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
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
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                base.OnPropertyChanged(propertyName);

                if (propertyName == HasShadowProperty.PropertyName)
                    InvalidateMeasure();
            });
        }
        #endregion
    }
}
