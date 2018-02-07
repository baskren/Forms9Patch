using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Forms9Patch
{

    internal class HeaderCell<TContent> : Cell<TContent> where TContent : View, new()
    {
        protected override void UpdateHeights()
        {
            if (View is BaseCellView view)
            {
                if (BindingContext is GroupWrapper groupWrapper)
                    view.RequestedRowHeight = groupWrapper.BestGuessGroupHeaderHeight();
                else if (BindingContext != null)
                    throw new Exception("Forms9Patch.GroupHeaderTemplate only works with Forms9Patch.ListView.  HeaderCell can only be used with a GroupWrapper as BindingContext.");

                view.SeparatorHeight = 0;

                DebugMessage("Set Height=[" + view.HeightRequest + "]");
                Height = view.HeightRequest;
                DebugMessage("Height=[" + Height + "]");
            }
            else
                throw new Exception("Forms9Patch.GroupHeaderTemplate only works with Forms9Patch.ListView.  HeaderCell can only be used with a BaseCellView as the View.");
        }
    }

    // the non-group header version of Cell
    internal class ItemCell<TContent> : Cell<TContent> where TContent : View, new()
    {
        protected override void UpdateHeights()
        {
            if (View is BaseCellView view)
            {
                if (BindingContext is ItemWrapper itemWrapper)
                {
                    view.RequestedRowHeight = itemWrapper.BestGuessItemRowHeight();
                    view.SeparatorHeight = itemWrapper.SeparatorHeight;
                }
                else if (BindingContext != null)
                    throw new Exception("Forms9Patch.DataTemplateSelector only works with Forms9Patch.ListView.  ItemCell can only be used with a ItemWrapper as the BindingContext.");

                DebugMessage("Set Height=[" + view.HeightRequest + "]");
                Height = view.HeightRequest;
                DebugMessage("Height=[" + Height + "]");
            }
            else 
                throw new Exception("Forms9Patch.DataTemplateSelector only works with Forms9Patch.ListView.  ItemCell can only be used with a BaseCellView as the View.");
        }
    }

    // The purpose of this class it to:
    // - capture and manage the height of Forms9Patch.ListView cells
    // - set proper BindingContext to a cell's content view 
    // In the future, it may be also to manage cell separators.
    internal class Cell<TContent> : ViewCell where TContent : View, new()
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
                System.Diagnostics.Debug.WriteLine("[" + GetType() + "." + callerName + ":" + lineNumber + "]["+InstanceId+"] " + message);
        }
        #endregion


        #region Fields
        internal int InstanceId { get; private set; }
        static int _instances;
        internal BaseCellView BaseCellView = new BaseCellView();
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.DataTemplateSelector.Cell`1"/> class.
        /// </summary>
        public Cell()
        {
            InstanceId = _instances++;
            //System.Diagnostics.Debug.WriteLine("\t\t\t{0} start", this.GetType());
            View = BaseCellView;
            BaseCellView.ContentView = new TContent();
            BaseCellView.ContentView.PropertyChanged += OnBaseCellPropertyChanged;

            UpdateHeights();
        }

        /// <summary>
        /// Triggered before a property is changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected override void OnPropertyChanging(string propertyName = null)
        {
            base.OnPropertyChanging(propertyName);
            if (propertyName == BindingContextProperty.PropertyName && View != null)
                View.BindingContext = null;
        }

        void OnBaseCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == VisualElement.HeightRequestProperty.PropertyName)
            {
                DebugMessage("e.PropertyName=[" + e.PropertyName + "]");
                UpdateHeights();
            }
        }

    /// <summary>
    /// Triggered by a change in the binding context.
    /// </summary>
    protected override void OnBindingContextChanged()
        {
            DebugMessage("Enter BindingContext=[" + BindingContext + "]");

            if (View != null)
            {
                // Even though the above call will likely cause BaseCellView.BindingContextChanged => BaseCellView.UpdateHeights => Cell.OnBaseCellPropertyChanged => UpdateHeights, it is not a sure thing (BaseCellView.ContentView.HeightRequest before it is called might be the same as new value since it is not updated here).
                UpdateHeights();
                View.BindingContext = BindingContext;
            }

            DebugMessage("Tunnelling down to base.OnBindingContextChanged");
            base.OnBindingContextChanged();
            DebugMessage("Returned from base.OnBindingContextChanged");

            DebugMessage("Exit");
        }

        protected virtual void UpdateHeights()
        {
            throw new NotImplementedException();
        }
    }
}