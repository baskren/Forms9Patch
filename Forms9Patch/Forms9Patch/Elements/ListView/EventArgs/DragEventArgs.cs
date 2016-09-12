using System;

namespace Forms9Patch
{
	/// <summary>
	/// DO NOT USE: Drag event arguments: used internally by Forms9Patch.ListView in the dragging of ListView cells.
	/// </summary>
	public class DragEventArgs : EventArgs
	{
		int[] _deepIndex;
		/// <summary>
		/// Gets or sets the deep index of the item being dragged.
		/// </summary>
		/// <value>The index of the deep.</value>
		public int[] DeepIndex {
			get {
				return _deepIndex;
			}
			set {
				_deepIndex = value;
				if (value == null || value.Length < 1)
					Alignment = HoverOverAlignment.None;
			}
		}

		internal Item Item;

		/// <summary>
		/// The alignment.
		/// </summary>
		public HoverOverAlignment Alignment;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.DragEventArgs"/> class.
		/// </summary>
		public DragEventArgs() {
			Clear();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.DragEventArgs"/> class.
		/// </summary>
		/// <param name="e">E.</param>
		public DragEventArgs(DragEventArgs e) {
			_deepIndex = new int[e._deepIndex.Length];
			Array.Copy (e._deepIndex, _deepIndex, e._deepIndex.Length);
			Item = e.Item;
			Alignment = e.Alignment;
		}

		/// <summary>
		/// Clear this instance.
		/// </summary>
		public void Clear() {
			Item = null;
			DeepIndex = null;
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Forms9Patch.DragEventArgs"/>.
		/// </summary>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Forms9Patch.DragEventArgs"/>.</returns>
		public override string ToString() {
			string indexPath = "";
			foreach (int index in DeepIndex)
				indexPath += index + ".";
			indexPath = indexPath.Substring (0, indexPath.Length - 1);
			return "[HoverOver:" + Item + ", " + indexPath + ", " + Alignment + "]";
		}

		/// <summary>
		/// Description this instance.
		/// </summary>
		public string Description() {
			string indexPath = "";
			foreach (int index in DeepIndex)
				indexPath += index + ".";
			indexPath = indexPath.Substring (0, indexPath.Length - 1);
			return "[HoverOver Item:" + Item + ", DeepIndex:" + indexPath + ", Alignment:" + Alignment + "]";
		}

	}
}

