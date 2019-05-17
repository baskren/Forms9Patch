using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Forms9Patch
{

    internal class HeaderCell<TContent> : Cell<TContent> where TContent : View, new()
    {
        public HeaderCell() : base()
        {
            BaseCellView.IsHeader = true;
        }
    }


    // the non-group header version of Cell
    internal class ItemCell<TContent> : Cell<TContent> where TContent : View, new()
    {
        public ItemCell() : base()
        {
            BaseCellView.IsHeader = false;
        }
    }

    // The purpose of this class it to:
    // - capture and manage the height of Forms9Patch.ListView cells
    // - set proper BindingContext to a cell's content view 
    // In the future, it may be also to manage cell separators.
    internal class Cell<TContent> : ViewCell, ICell_T_Height where TContent : View, new()
    {
        #region debug convenience
        protected bool Debug
        {
            get
            {
                //return (BindingContext is ItemWrapper<string> itemWrapper && itemWrapper.Index == 0);
                return false;
                // return (BindingContext is GroupWrapper wrapper && wrapper.Index == 0);
            }
        }

        protected void DebugMessage(string message, [System.Runtime.CompilerServices.CallerMemberName] string callerName = null, [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            if (Debug)
                System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + callerName + ":" + lineNumber + "][" + InstanceId + "] " + message);
        }
        #endregion


        #region Fields
        internal int InstanceId { get; private set; }

        static int _instances;
        internal BaseCellView BaseCellView = new BaseCellView();
        bool _freshHeight;
        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.DataTemplateSelector.Cell`1"/> class.
        /// </summary>
        public Cell()
        {
            InstanceId = _instances++;
            View = BaseCellView;
            BaseCellView.ContentView = new TContent();
            if (BaseCellView.ContentView is ICellHeight contentView)
                Height = contentView.CellHeight;
        }

        /// <summary>
        /// Triggered before a property is changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected override void OnPropertyChanging(string propertyName = null)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanging(propertyName));
                return;
            }

            base.OnPropertyChanging(propertyName);

            if (propertyName == BindingContextProperty.PropertyName && View != null)
                View.BindingContext = null;
        }

        /// <summary>
        /// Triggered by a change in the binding context.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(OnBindingContextChanged);
                return;
            }

            if (View != null)
                View.BindingContext = BindingContext;

            _freshHeight = true;

            base.OnBindingContextChanged();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            // don't know why the below "&& UWP" was added.  A really bone headed move.
            if (propertyName == nameof(Height))// && Device.RuntimePlatform == Device.UWP)
            {
                UpdateSize();
                _freshHeight = false;
            }
        }

        bool _updatingSize;
        async Task UpdateSize()
        {
            if (_updatingSize && _freshHeight)
                return;
            _updatingSize = true;
            await Task.Delay(200);
            if (this.RealParent != null)
                ForceUpdateSize();
            _updatingSize = false;
        }

    }
}