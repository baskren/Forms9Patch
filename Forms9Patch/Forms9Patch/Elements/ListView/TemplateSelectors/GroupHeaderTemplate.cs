using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Forms9Patch
{
    public class GroupHeaderTemplate : Xamarin.Forms.DataTemplate
    {
        public GroupHeaderTemplate(Type groupHeaderViewType) : base(typeof(Cell<>).MakeGenericType(new[] { groupHeaderViewType }))
        {
        }

        class Cell<TContent> : ViewCell where TContent : View, new()
        {

            internal BaseCellView BaseCellView = new BaseCellView();
            readonly ICellHeight _iCellContent;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:Forms9Patch.DataTemplateSelector.Cell`1"/> class.
            /// </summary>
            public Cell()
            {
                //System.Diagnostics.Debug.WriteLine("\t\t\t{0} start", this.GetType());
                View = BaseCellView;
                BaseCellView.Content = new TContent();
                _iCellContent = BaseCellView.Content as ICellHeight;
                if (_iCellContent != null && _iCellContent.CellHeight >= 0)
                    Height = _iCellContent.CellHeight;
                BaseCellView.Content.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == VisualElement.HeightRequestProperty.PropertyName)
                        SetHeight();
                };
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

            /// <summary>
            /// Triggered by a change in the binding context.
            /// </summary>
            protected override void OnBindingContextChanged()
            {
                base.OnBindingContextChanged();
                if (View != null)
                    View.BindingContext = BindingContext;
                SetHeight();
            }

            void SetHeight()
            {
                var iItem = BindingContext as IItemWrapper;
                if (iItem != null)
                {
                    if (_iCellContent != null && _iCellContent.CellHeight >= 0)
                        Height = _iCellContent.CellHeight + 1;
                    else
                        Height = iItem.RowHeight + 1;
                }
                View.HeightRequest = Height;
                //System.Diagnostics.Debug.WriteLine("GroupTemplate.SetHeight View.HeightRequest = ["+Height+"]");
            }
        }

    }
}
