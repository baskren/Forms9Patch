using System;
using System.ComponentModel;

namespace Forms9Patch
{
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    internal class OffsetDataSetBase
    {
        public ItemWrapper ItemWrapper { get; private set; }

        public double Offset { get; private set; }

        public double CellHeight { get; private set; }

        public OffsetDataSetBase(ItemWrapper itemWrapper, double offset, double cellHeight)
        {
            ItemWrapper = itemWrapper;
            Offset = offset;
            CellHeight = cellHeight;

            /* the following is not a good idea because we really can and will agglomerate subGroups into cell views.
            if (itemWrapper is GroupWrapper groupWrapper)
            {
                CellHeight = groupWrapper.GroupHeaderRowHeight;
                if (CellHeight < 0 && groupWrapper.Parent is GroupWrapper parent)
                    CellHeight = parent.GroupHeaderRowHeight;
            }
            if (CellHeight < 0)
            {
                CellHeight = itemWrapper.RowHeight;
                if (CellHeight < 0 && itemWrapper.Parent is GroupWrapper parent)
                    CellHeight = parent.RowHeight;
            }
            */
        }

        public override string ToString()
        {
            return "ItemWrapper[" + ItemWrapper.Description() + "] Offset[" + Offset + "] CellHeight[" + CellHeight + "]";
        }

        public virtual string Description()
        {
            return ItemWrapper.Description() + ", " + Offset + ", " + CellHeight;
        }
    }

    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    internal class DeepDataSet : OffsetDataSetBase
    {
        public int[] Index { get; }

        public int FlatIndex { get; }

        public DeepDataSet(ItemWrapper itemWrapper, double offset, int[] index, int flatIndex, double cellHeight) : base(itemWrapper, offset, cellHeight)
        {
            Index = index;
            FlatIndex = flatIndex;
        }

        public override string ToString()
        {
            return base.ToString() + " Index[{" + string.Join(", ", Index) + "} FlatIndex=[" + FlatIndex + "]";
        }

        public override string Description()
        {
            return base.Description() + ", {" + string.Join(", ", Index) + "}, " + FlatIndex;
        }
    }


}