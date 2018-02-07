using System;

namespace Forms9Patch
{
    /// <summary>
    /// DO NOT USE: Drag event arguments: used internally by Forms9Patch.ListView in the dragging of ListView cells.
    /// </summary>
    internal class CellEventArgs : EventArgs
    {
        protected internal int[] _deepIndex;
        /// <summary>
        /// Gets or sets the deep index of the item being dragged.
        /// </summary>
        /// <value>The index of the deep.</value>
        public int[] DeepIndex
        {
            get => _deepIndex;
            set => _deepIndex = value;
        }

        public ItemWrapper ItemWrapper
        {
            get; private set;
        }

        public CellEventArgs(GroupWrapper gr, ItemWrapper itemWrapper)
        {
            Clear();
            ItemWrapper = itemWrapper;
            DeepIndex = gr.DeepSourceIndexOf(itemWrapper);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.CellProximityEventArgs"/> class.
        /// </summary>
        public CellEventArgs(ItemWrapper itemWrapper, int[] deepIndex)
        {
            Clear();
            ItemWrapper = itemWrapper;
            DeepIndex = deepIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.CellProximityEventArgs"/> class.
        /// </summary>
        /// <param name="e">E.</param>
        public CellEventArgs(CellEventArgs e)
        {
            _deepIndex = new int[e._deepIndex.Length];
            Array.Copy(e._deepIndex, _deepIndex, e._deepIndex.Length);
            ItemWrapper = e.ItemWrapper;
        }

        /// <summary>
        /// Clear this instance.
        /// </summary>
        public virtual void Clear()
        {
            ItemWrapper = null;
            DeepIndex = null;
        }



        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Forms9Patch.CellProximityEventArgs"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Forms9Patch.CellProximityEventArgs"/>.</returns>
        public override string ToString()
        {
            string indexPath = "";
            foreach (int index in DeepIndex)
                indexPath += index + ".";
            indexPath = indexPath.Substring(0, indexPath.Length - 1);
            return "["+GetType()+":" + ItemWrapper + "," + indexPath + "]";
        }

        /// <summary>
        /// Description this instance.
        /// </summary>
        public virtual string Description()
        {
            string indexPath = "";
            foreach (int index in DeepIndex)
                indexPath += index + ".";
            indexPath = indexPath.Substring(0, indexPath.Length - 1);
            return "["+GetType()+" ItemWrapper:" + ItemWrapper + ", DeepIndex:" + indexPath + "]";
        }

    }
}

