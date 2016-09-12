using Xamarin.Forms;
using System;

namespace Forms9Patch
{
	/// <summary>
	/// FormsDragNDropListView Item.
	/// </summary>
	abstract class Item : BindableObject {


		#region Fields
		protected static bool debugProperties = false;
		public readonly int ID;
		#endregion


		#region Properties

		/*
		string _key;
		public string Key {
			get { return _key; }
			set { 
				if (debugProperties) 
					System.Diagnostics.Debug.WriteLine ("Key: update from ["+_key+"] to [" + value + "]");
				_key = value;
			}
		}

		public static readonly BindableProperty TitleProperty = BindableProperty.Create("Title", typeof(string), typeof(Item), default(string));
		public string Title {
			get { return (string)GetValue(TitleProperty); }
			set { 
				if (debugProperties) 
					System.Diagnostics.Debug.WriteLine ("Title: update from ["+(string)GetValue(TitleProperty)+"] to [" + value + "]");
				if (Title != value)
					SetValue (TitleProperty, value); 
			}
		}

		public static readonly BindableProperty HelpProperty  = BindableProperty.Create("Help",  typeof(string), typeof(Item), default(string));
		public string Help {
			get { return (string)GetValue(HelpProperty); }
			set { 
				if (debugProperties) System.Diagnostics.Debug.WriteLine ("Help: update from ["+(string)GetValue(HelpProperty)+"] to [" + value + "]");
				if (Help != value)
					SetValue (HelpProperty, value);
			}
		}
		*/

		/*
		public static readonly BindableProperty IsVisibleProperty  = BindableProperty.Create("IsVisible",  typeof(bool), typeof(Item), true);
		public bool IsVisible {
			get { return (bool)GetValue(IsVisibleProperty); }
			internal set { SetValue (IsVisibleProperty, value); }
		}
		*/

		public static readonly BindableProperty SeparatorIsVisibleProperty  = BindableProperty.Create("SeparatorIsVisible",  typeof(bool), typeof(Item), false);
		public bool SeparatorIsVisible {
			get { 
				return (bool)GetValue(SeparatorIsVisibleProperty); 
			}
			internal set { SetValue (SeparatorIsVisibleProperty, value); }
		}		

		public static readonly BindableProperty SeparatorColorProperty  = BindableProperty.Create("SeparatorColor",  typeof(Color), typeof(Item), Color.FromRgba(0,0,0,0.12));
		public Color SeparatorColor {
			get { return (Color)GetValue(SeparatorColorProperty); }
			internal set { 
				SetValue (SeparatorColorProperty, value); 
			}
		}

		/// <summary>
		/// The separator height property.
		/// </summary>
		public static readonly BindableProperty SeparatorHeightProperty = BindableProperty.Create("SeparatorHeight", typeof(double), typeof(Item), -1.0);
		/// <summary>
		/// Gets or sets the height of the separator.
		/// </summary>
		/// <value>The height of the separator.</value>
		public double SeparatorHeight
		{
			get { return (double)GetValue(SeparatorHeightProperty); }
			set { SetValue(SeparatorHeightProperty, value); }
		}

		/// <summary>
		/// The separator left indent property.
		/// </summary>
		public static readonly BindableProperty SeparatorLeftIndentProperty = BindableProperty.Create("SeparatorLeftIndent", typeof(double), typeof(Item), 20.0);
		/// <summary>
		/// Gets or sets the separator left indent.
		/// </summary>
		/// <value>The separator left indent.</value>
		public double SeparatorLeftIndent
		{
			get { return (double)GetValue(SeparatorLeftIndentProperty); }
			set { SetValue(SeparatorLeftIndentProperty, value); }
		}

		/// <summary>
		/// The separator right indent property.
		/// </summary>
		public static readonly BindableProperty SeparatorRightIndentProperty = BindableProperty.Create("SeparatorRightIndent", typeof(double), typeof(Item), 0.0);
		/// <summary>
		/// Gets or sets the separator right indent.
		/// </summary>
		/// <value>The separator right indent.</value>
		public double SeparatorRightIndent
		{
			get { return (double)GetValue(SeparatorRightIndentProperty); }
			set { SetValue(SeparatorRightIndentProperty, value); }
		}


		public static readonly BindableProperty BackgroundColorProperty  = BindableProperty.Create("BackgroundColor",  typeof(Color), typeof(Item), Color.Transparent);
		public Color BackgroundColor {
			get { return (Color)GetValue(BackgroundColorProperty); }
			internal set { SetValue (BackgroundColorProperty, value); }
		}	

		public static readonly BindableProperty SourceProperty = BindableProperty.Create("Value", typeof(object),  typeof(Item), null);
		public object Source {
			get { return GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}

		public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.Create("SelectedBackgroundColor", typeof(Color), typeof(Item), Color.Gray);
		public Color SelectedBackgroundColor
		{
			get { return (Color)GetValue(SelectedBackgroundColorProperty); }
			set { SetValue(SelectedBackgroundColorProperty, value); }
		}

		public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create("IsSelected", typeof(bool), typeof(Item), false);
		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set { 
				SetValue(IsSelectedProperty, value); 
			}
		}

		internal static readonly BindableProperty IndexProperty = BindableProperty.Create("Index", typeof(int), typeof(Item), -1);
		internal int Index
		{
			get { return (int)GetValue(IndexProperty); }
			set { SetValue(IndexProperty, value); }
		}

		#endregion


		#region Constructors
		static int instantiations;

		protected Item() {
			ID = instantiations++;
		}
		#endregion


		#region Convenience
		internal void ShallowCopy(Item other) {
			SeparatorColor = other.SeparatorColor;
			SeparatorIsVisible = other.SeparatorIsVisible;
			BackgroundColor = other.BackgroundColor;
			SelectedBackgroundColor = other.SelectedBackgroundColor;
			Source = other.Source;
		}

		public string Description ()
		{
			//return string.Format ("{0} ({1})", ID ,Title);
			return string.Format("{0}[{1}]",GetType().Name,ID);
			//return string.Format ("[BcElementVMGroup: Item={0}, Count={1}, IsReadOnly={2}, IsFixedSize={3}, IsSynchronized={4}, SyncRoot={5}, VisibilityTargetGroup={6}]", Item, Count, IsReadOnly, IsFixedSize, IsSynchronized, SyncRoot, VisibilityTargetGroup);
		}
		#endregion


		/*
		#region Operations
		public override int GetHashCode() {
			return ID;
		}
			
		/// <summary>
		/// Value equality test
		/// </summary>
		/// <param name="other">The <see cref="FormsDragNDropListView.Item"/> to compare with the current <see cref="FormsDragNDropListView.Item"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="FormsDragNDropListView.Item"/> is equal to the current
		/// <see cref="FormsDragNDropListView.Item"/>; otherwise, <c>false</c>.</returns>
		public bool Equals(Item other) {
			// value equality
			if (other == null)
				return false;
			if (Value == null)
				return other.Value == null;
			return Value.Equals (other.Value);
		}


		public override bool Equals(object obj) {
			return Equals( obj as Item );
		}

		#endregion
		
		#region Editing
		//internal bool IsDragging = false;
		#endregion
		*/

		#region Reference to CellView bound to Item
		internal WeakReference _weakBaseCellView;
		internal BaseCellView BaseCellView {
			get { return (BaseCellView)(_weakBaseCellView != null && _weakBaseCellView.IsAlive ? _weakBaseCellView.Target : null); }
			set { _weakBaseCellView = (value==null ? null : new WeakReference (value)); }
		}
		#endregion


		#region Property change management
		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			//if (propertyName==SeparatorColorProperty.PropertyName || propertyName==SeparatorIsVisibleProperty.PropertyName)
			//	System.Diagnostics.Debug.WriteLine("["+ID+"] SeparatorColor=["+SeparatorColor+"] SeparatorVisibility=["+SeparatorIsVisible+"]");
			if (propertyName == IsSelectedProperty.PropertyName)
			{
				var isSelectedSource = Source as IIsSelectedAble;
				if (isSelectedSource != null)
					isSelectedSource.IsSelected = IsSelected;
			}
		}
		#endregion
	}
}

