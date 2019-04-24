using System;

namespace Forms9Patch
{
	/// <summary>
	/// DO NOT USE: Drag event arguments: used internally by Forms9Patch.ListView in the dragging of ListView cells.
	/// </summary>
	internal class CellProximityEventArgs : CellEventArgs
	{

        public static CellProximityEventArgs None = new CellProximityEventArgs(null, null, Proximity.None);

		public new int[] DeepIndex {
			get {
				return _deepIndex;
			}
			set {
				_deepIndex = value;
				if (value == null || value.Length < 1)
					Proximity = Proximity.None;
			}
		}

		internal ItemWrapper Item;

		/// <summary>
		/// The alignment.
		/// </summary>
		public Proximity Proximity;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.CellProximityEventArgs"/> class.
        /// </summary>
        /// <param name="groupWrapper"></param>
        /// <param name="itemWrapper"></param>
        /// <param name="proximity"></param>
        public CellProximityEventArgs(GroupWrapper groupWrapper, ItemWrapper itemWrapper, Proximity proximity=Proximity.None) : base (groupWrapper, itemWrapper)
        {
            Proximity = proximity;
		}

        /// <summary>
        /// Initialize instance of the <see cref="T:Forms9Patch.CellProximityEventArgs"/> class.
        /// </summary>
        /// <param name="itemWrapper"></param>
        /// <param name="deepIndex"></param>
        /// <param name="proximity"></param>
        public CellProximityEventArgs(ItemWrapper itemWrapper, int[] deepIndex, Proximity proximity = Proximity.None) : base(itemWrapper, deepIndex)
        {
            Proximity = proximity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.CellProximityEventArgs"/> class.
        /// </summary>
        /// <param name="e">E.</param>
        public CellProximityEventArgs(CellProximityEventArgs e) : base(e)
        {
			Proximity = e.Proximity;
		}

		/// <summary>
		/// Clear this instance.
		/// </summary>
		public override void Clear()
        {
            Proximity = Proximity.None;
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Forms9Patch.CellProximityEventArgs"/>.
		/// </summary>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Forms9Patch.CellProximityEventArgs"/>.</returns>
		public override string ToString() {
			return base.ToString().Replace("]", ", " + Proximity + "]");
		}

		/// <summary>
		/// Description this instance.
		/// </summary>
		public override string Description() {
			return base.Description().Replace("]", ", Alignment:" + Proximity + "]");
		}

	}
}

