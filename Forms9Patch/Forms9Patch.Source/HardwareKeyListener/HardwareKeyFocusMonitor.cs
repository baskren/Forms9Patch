using System;
using Xamarin.Forms;

namespace Forms9Patch
{
    public static class HardwareKeyFocusMonitor
    {

        /*

        static HardwareKeyFocusMonitor()
        {
            FocusMonitor.FocusedElementChanged += OnFocusElementChanged;
        }

        static void OnFocusElementChanged(object sender, EventArgs e)
        {
            FocusedElement = sender as VisualElement;
        }

        public static bool Enabled
        {
            get => FocusMonitor.Enabled;
            internal set => FocusMonitor.Enabled = value;
        }

        internal static VisualElement _element;
        public static VisualElement FocusedElement
        {
            get { return _element; }
            set
            {
                if (_element != value)
                {
                    System.Diagnostics.Debug.WriteLine("==============================================");
                    System.Diagnostics.Debug.WriteLine("HardwareKeyFocusElement Was=[" + _element + "]");
                    _element = value;
                    ElementChanged?.Invoke(value, EventArgs.Empty);
                    System.Diagnostics.Debug.WriteLine("HardwareKeyFocusElement  Is=[" + _element + "]");
                    System.Diagnostics.Debug.WriteLine("==============================================");
                    System.Diagnostics.Debug.WriteLine("");
                }
            }
        }

        public static VisualElement DefaultElement { get; set; }

        public static event EventHandler ElementChanged;

*/
    }
}