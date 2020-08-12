using System.ComponentModel;

namespace Forms9Patch
{
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    class ItemWrapper<TItem> : ItemWrapper
    {

        #region Properties
        public new TItem Source
        {
            get => (TItem)GetValue(SourceProperty);
            set
            {
                if (!Equals(value, Source))
                    SetValue(SourceProperty, value);
            }
        }
        #endregion


        #region Convenience
        internal void ShallowCopy(ItemWrapper<TItem> other)
        {
            CellBackgroundColor = other.CellBackgroundColor;
            SelectedCellBackgroundColor = other.SelectedCellBackgroundColor;
            Source = other.Source;
        }
        #endregion


        public override string ToString()
        {
            //return base.ToString();
            return "ItemWrapper[" + Source.ToString() + "]";
        }
    }
}
