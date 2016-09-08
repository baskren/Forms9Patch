using System;
using Xamarin.Forms;
using System.ComponentModel;
using FormsGestures;
using System.Linq;
using System.Collections.Generic;

namespace Forms9Patch
{
	/// <summary>
	/// FormsDragNDropListView List view.
	/// </summary>
	public class ListView : Xamarin.Forms.ListView
	{

		#region ListView proxy
		/// <summary>
		/// The separator visibility property.
		/// </summary>
		public static new readonly BindableProperty SeparatorVisibilityProperty = BindableProperty.Create ("SeparatorVisibility", typeof(SeparatorVisibility), typeof(ListView), default(SeparatorVisibility));
		/// <summary>
		/// Gets or sets the separator visibility.
		/// </summary>
		/// <value>The separator visibility.</value>
		public new SeparatorVisibility SeparatorVisibility {
			get { return (SeparatorVisibility)GetValue (SeparatorVisibilityProperty); }
			set { SetValue (SeparatorVisibilityProperty, value); }
		}

		/// <summary>
		/// The cell background color property.
		/// </summary>
		public static readonly BindableProperty CellBackgroundColorProperty = BindableProperty.Create ("CellBackgroundColor", typeof(Color), typeof(ListView), Color.Transparent);
		/// <summary>
		/// Gets or sets the color of the cell background.
		/// </summary>
		/// <value>The color of the cell background.</value>
		public Color CellBackgroundColor {
			get { return (Color)GetValue (CellBackgroundColorProperty); }
			set { 
				if (DisplayedItems != null)
					DisplayedItems.BackgroundColor = value;
				SetValue (CellBackgroundColorProperty, value); 
			}
		}

		/// <summary>
		/// The separator color property.
		/// </summary>
		public static new readonly BindableProperty SeparatorColorProperty = Xamarin.Forms.ListView.SeparatorColorProperty;
		/// <summary>
		/// Gets or sets the color of the separator.
		/// </summary>
		/// <value>The color of the separator.</value>
		public new Color SeparatorColor {
			get { return (Color)GetValue(Xamarin.Forms.ListView.SeparatorColorProperty); }
			set { 
				if (DisplayedItems != null)
					DisplayedItems.SeparatorColor = value;
				SetValue(Xamarin.Forms.ListView.SeparatorColorProperty, value); 
			}
		}

		/// <summary>
		/// Gets or sets the item template.
		/// </summary>
		/// <value>The item template.</value>
		public new DataTemplateSelector ItemTemplate {
			get { return (DataTemplateSelector)GetValue (ItemTemplateProperty); }
			set { 
				SetValue (ItemTemplateProperty, value); 
			}
		}

		/// <summary>
		/// Occurs when item is selected.
		/// </summary>
		public new event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

		/// <summary>
		/// Occurs when item is tapped.
		/// </summary>
		public new event EventHandler<ItemTappedEventArgs> ItemTapped;

		/// <summary>
		/// Occurs when item is appearing.
		/// </summary>
		public new event EventHandler<ItemVisibilityEventArgs> ItemAppearing;

		/// <summary>
		/// Occurs when item is disappearing.
		/// </summary>
		public new event EventHandler<ItemVisibilityEventArgs> ItemDisappearing;
		#endregion


		#region Constructor
		static int Count;
		int id;
		Listener _listener;

		readonly ModalPopup _popup = new ModalPopup {
			Padding = 3,
			HasShadow = true,
			OutlineRadius = 4
		};

		void init() {
			id = Count++;
			HasUnevenRows = false;
			BackgroundColor = Color.Transparent;
			base.SeparatorColor = Color.Transparent;
			base.SeparatorVisibility = SeparatorVisibility.None;

			base.ItemSelected += (sender, e) => {
				System.Diagnostics.Debug.WriteLine("base.ItemSelected");
				//OnItemSelected(sender, e);
				var source = ((Item)e?.SelectedItem)?.Source;
				if (source !=null) 
					ItemSelected?.Invoke(this,new SelectedItemChangedEventArgs(source));
			};

			base.ItemTapped += (sender, e) => {
				System.Diagnostics.Debug.WriteLine("base.ItemTapped");
				var source = ((Item)e?.Item)?.Source;
				var group = (Group)((BindableObject)e.Group).GetValue(Xamarin.Forms.ListView.ItemsSourceProperty);
				if (source !=null) 
					ItemTapped?.Invoke(this,new ItemTappedEventArgs(group,source));
			};

			base.ItemAppearing += (sender, e) => {
				var item = ((Item)e?.Item);
				var source = item?.Source;
				if (source != null)
					ItemAppearing?.Invoke(this, new ItemVisibilityEventArgs(source));
			};

			base.ItemDisappearing += (sender, e) => {
				var source = ((Item)e?.Item)?.Source;
				if (source != null)
					ItemDisappearing?.Invoke(this, new ItemVisibilityEventArgs(source));
			};

			IsEnabled = true;
			_listener = new Listener (this);
			_listener.LongPressed += OnLongPressed;
			_listener.LongPressing += OnLongPressing;
			_listener.Panning += OnPanning;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.ListView"/> class.
		/// </summary>
		/// <param name="strategy">Strategy.</param>
		public ListView(ListViewCachingStrategy strategy) : base (strategy){
			init ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.ListView"/> class.
		/// </summary>
		public ListView() {
			init ();
		}

		/// <summary>
		/// Description this instance.
		/// </summary>
		public string Description() {
			return "ListView[" + id + "]";
		}
		#endregion


		#region proxy's enhanced functionality
		/// <summary>
		/// The editable property.
		/// </summary>
		public static readonly BindableProperty EditableProperty = BindableProperty.Create ("Editable", typeof(bool), typeof(ListView), false);
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Forms9Patch.ListView"/> is editable.
		/// </summary>
		/// <value><c>true</c> if editable; otherwise, <c>false</c>.</value>
		public bool Editable {
			get { return (bool)GetValue (EditableProperty); }
			set { SetValue (EditableProperty, value); }
		}

		/// <summary>
		/// The source property map property.
		/// </summary>
		public static readonly BindableProperty SourcePropertyMapProperty = BindableProperty.Create ("SourcePropertyMap", typeof(List<string>), typeof(ListView), default(List<string>));
		/// <summary>
		/// Gets or sets the source property map.  Used to map the properties in a heirachial ItemsSource used to make the heirachy and bind (as items) to the CellViews
		/// </summary>
		/// <value>The source property map.</value>
		public List<string> SourcePropertyMap {
			get { return (List<string>)GetValue (SourcePropertyMapProperty); }
			set { 
				SetValue (SourcePropertyMapProperty, value); 
				UpdateItemsSource ();
			}
		}

		Group _displayedItemsSource;
		/// <summary>
		/// The items source property.
		/// </summary>
		public static new readonly BindableProperty ItemsSourceProperty = BindableProperty.Create( "ItemsSource", typeof(object), typeof(ListView), null);
		/// <summary>
		/// Gets or sets the items source.
		/// </summary>
		/// <value>The items source.</value>
		public new object ItemsSource {
			get { return GetValue(ItemsSourceProperty); }
			set { 
				SetValue (ItemsSourceProperty, value);
				UpdateItemsSource ();
			}
		}

		void UpdateItemsSource() {
			// be ready to set / reset IsGroupingEnabled as items are added or removed
			/* need to use Group to determine if grouping is enabled.
			var observableCollection = value as INotifyCollectionChanged;
			if (observableCollection != null) {
				observableCollection.CollectionChanged += (sender, e) => {
					switch (e.Action) {
					case NotifyCollectionChangedAction.Add:
						if (e.NewItems.Count==ItemsSource.Count) 
							IsGroupingEnabled = value [0] is IList;
						break;
					case NotifyCollectionChangedAction.Move:
						break;
					case NotifyCollectionChangedAction.Remove:
						IsGroupingEnabled &= ItemsSource.Count > 0;
						break;
					case NotifyCollectionChangedAction.Replace:
						if (ItemsSource.Count==1)
							IsGroupingEnabled = value [0] is IList;
						break;
					case NotifyCollectionChangedAction.Reset:
						if (ItemsSource.Count > 0)
							IsGroupingEnabled = value [0] is IList;
						break;
					}
				};
			}
			*/
			// This listView.ItemsSource is a target, so validity testing is to facility drag/drop of items
			if (ItemsSource == null)
				return;
			_displayedItemsSource = new Group(ItemsSource, SourcePropertyMap);
			base.ItemsSource = _displayedItemsSource;
			IsGroupingEnabled = _displayedItemsSource.ContentType == Group.GroupContentType.Lists;

			if (_displayedItemsSource != null) {
				_displayedItemsSource.SeparatorIsVisible = SeparatorVisibility!=SeparatorVisibility.None;
				_displayedItemsSource.BackgroundColor = CellBackgroundColor;
				_displayedItemsSource.SeparatorColor = SeparatorColor;
			}
		}

		internal Group DisplayedItems {
			get { return _displayedItemsSource; }
		}


		/// <summary>
		/// The selected cell background color property.
		/// </summary>
		public static readonly BindableProperty SelectedCellBackgroundColorProperty = BindableProperty.Create( "SelectedCellBackgroundColor", typeof(Color), typeof(ListView), Color.FromRgba(200,200,200,255));
		/// <summary>
		/// Gets or sets the color of the selected cell background.
		/// </summary>
		/// <value>The color of the selected cell background.</value>
		public Color SelectedCellBackgroundColor {
			get { return (Color) GetValue (SelectedCellBackgroundColorProperty); }
			set { 
				SetValue (SelectedCellBackgroundColorProperty, value); 
			}
		}


		Item _currentlySelectedItem;
		void OnItemSelected(object sender, SelectedItemChangedEventArgs e) {
			System.Diagnostics.Debug.WriteLine("OnItemSelected");
			if (_currentlySelectedItem != null)
				_currentlySelectedItem.BackgroundColor = Color.Transparent;
			_currentlySelectedItem = SelectedItem as Item;
			_currentlySelectedItem.BackgroundColor = SelectedCellBackgroundColor;
		}

		/// <summary>
		/// Triggered when a property is about to change
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected override void OnPropertyChanging (string propertyName = null)
		{
			base.OnPropertyChanging (propertyName);
			base.OnPropertyChanged (propertyName);
			if (propertyName == SelectedItemProperty.PropertyName && SelectedItem!=null) {
				((Item)SelectedItem).BackgroundColor = CellBackgroundColor;
			}
		}

		/// <summary>
		/// Trigged with a property has changed
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected override void OnPropertyChanged (string propertyName = null)
		{
			//System.Diagnostics.Debug.WriteLine ("\t\tListView.OnPropertyChanged enter");
			base.OnPropertyChanged (propertyName);
			if (propertyName == SelectedItemProperty.PropertyName && SelectedItem!=null) {
				((Item)SelectedItem).BackgroundColor = SelectedCellBackgroundColor;
			}
			//System.Diagnostics.Debug.WriteLine ("\t\tListView.OnPropertyChanged exit");
		}

		#endregion


		#region Delgation support
		/*
		void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
			//System.Diagnostics.Debug.WriteLine ("CollectionChanged:(" + sender + ", " + e + ")");
			//System.Diagnostics.Debug.WriteLine ("  causedBy: " + e.Action);
		}
		*/

		/// <summary>
		/// Occurs when a property of a ListViewItem is about to change.
		/// </summary>
		public event PropertyChangingEventHandler ItemPropertyChanging;
		void OnItemPropertyChanging(object sender, PropertyChangingEventArgs e) {
			//var element = sender as ListViewItem;
			//System.Diagnostics.Debug.WriteLine ("PropertyChanging(" + sender + ", " + e + ")");
			//System.Diagnostics.Debug.WriteLine ("  property: " + e.PropertyName);
			//System.Diagnostics.Debug.WriteLine ("  Title:["+element.Title+"] Help["+element.Help+"] ");
			PropertyChangingEventHandler propertyChangingEventHandler = ItemPropertyChanging;
			if (propertyChangingEventHandler != null)
				propertyChangingEventHandler (sender, e);
		}

		/// <summary>
		/// Occurs when property of a ListViewItem has changed.
		/// </summary>
		public event PropertyChangedEventHandler ItemPropertyChanged;
		void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e) {
			//var element = sender as ListViewItem;
			//System.Diagnostics.Debug.WriteLine ("PropertyChanged(" + sender + ", " + e + ")");
			//System.Diagnostics.Debug.WriteLine ("  property: " + e.PropertyName);
			//System.Diagnostics.Debug.WriteLine ("  Title:["+element.Title+"] Help["+element.Help+"] ");
			PropertyChangedEventHandler propertyChangedEventHandler = ItemPropertyChanged;
			if (propertyChangedEventHandler != null)
				propertyChangedEventHandler (sender, e);
		}
		#endregion


		#region DragDrop support
		internal static readonly BindableProperty ScrollEnabledProperty = BindableProperty.Create("ScrollEnabled",typeof(bool),typeof(ListView),true);
		internal bool ScrollEnabled {
			get { return (bool)GetValue (ScrollEnabledProperty); }
			set { SetValue (ScrollEnabledProperty, value); }
		}


		internal Func<Rectangle, DragEventArgs> RendererFindItemDataUnderRectangle;
		internal DragEventArgs FindItemDataUnderRectangle(Rectangle rect) {
			return RendererFindItemDataUnderRectangle != null ? RendererFindItemDataUnderRectangle (rect) : null;
		}


		internal Func<double,bool> RendererScrollBy;
		double _scrollSpeed;
		bool _scrolling;
		void ScrollSpeed(double speed) {
			if (!_scrolling &&  Math.Abs(_scrollSpeed) > 0) {
				_scrolling = true;
				Device.StartTimer (TimeSpan.FromMilliseconds (25), () => {
					_scrolling = RendererScrollBy(_scrollSpeed);
					_scrolling &= Math.Abs(_scrollSpeed) > 0;
					return _scrolling;
				});
			}
			_scrollSpeed = speed;
		}

		/// <summary>
		/// Scroll the ListView by so many DIPs
		/// </summary>
		/// <returns><c>true</c>, if by was scrolled, <c>false</c> otherwise.</returns>
		/// <param name="delta">Delta.</param>
		public bool ScrollBy(double delta) {
			return RendererScrollBy (delta);
		}
		#endregion


		#region Drag/Drop
		/// <summary>
		/// Delegate function that will be called to query if a item (at a deep index location) can be dragged
		/// </summary>
		public Func<ListView, object, int[],bool> CanDrag;
		/// <summary>
		/// Delegate function that will be called to query if a item being dragged can be dropped at location specified by a deep index.
		/// </summary>
		public Func<ListView, object, int[],bool> CanDrop;

		DragEventArgs _longPress;

		readonly NullItem _nullItem = new NullItem ();
		readonly BlankItem _blankItem = new BlankItem();

		Rectangle _nativeFrame;

		void OnLongPressing(object sender, LongPressEventArgs e) {
			if (!Editable)
				return;
			// will be called when the _listener (attached to this) detects a long press
			if (_longPress != null)
				return;

			// we need to know what item is being pressed and it's corresponding view
			//System.Diagnostics.Debug.WriteLine ("LONGPRESSING ["+e.Listener.Element+"]");

			_longPress = DependencyService.Get<IListItemLocation>().DragEventArgsForItemAtPoint(this,e.Center);
			if (_longPress != null && _longPress.Item.BaseCellView != null) {
				// can we drag this Item?
				bool canDrag = true;
				if (CanDrag != null)
					canDrag = CanDrag (this,_longPress.Item.Source, _longPress.DeepIndex);
				if (!canDrag) {
					_longPress = null;
					return;
				}
				SelectedItem = null;
				_nativeFrame = _longPress.Item.BaseCellView.BoundsToWinCoord();

				// need a null item to fill the void 
				_nullItem.RequestedHeight = _nativeFrame.Height;
				_nullItem.BackgroundColor = BackgroundColor;

				_displayedItemsSource.NotifySourceOfChanges = false;
				_displayedItemsSource.DeepSwapItems (_longPress.Item, _nullItem);

				_longPress.Item.SeparatorIsVisible = false;
				var contentView = ItemTemplate.MakeContentView (_longPress.Item);
				contentView.WidthRequest = _nativeFrame.Width;
				contentView.HeightRequest = _nativeFrame.Height;
				contentView.BackgroundColor = Color.Transparent;
				//_popup.BackgroundColor = contentView.BackgroundColor == Color.Transparent ? Color.White : contentView.BackgroundColor;\
				_popup.BackgroundColor = CellBackgroundColor;
				_popup.Content = contentView;
				_popup.Location = _nativeFrame.Location;
				_popup.IsVisible = true;

				ScrollEnabled = false;
			} else {
				_longPress = null;
			}
		}


		void OnPanning(object sender, PanEventArgs e) {
			if (_longPress != null) {
				//_longPressPan = true;
				//System.Diagnostics.Debug.WriteLine("PAN ["+e.Listener.Element+"]");
				System.Diagnostics.Debug.WriteLine("LONGPRESS PANNING");
				_popup.TranslationX = e.TotalDistance.X;
				_popup.TranslationY = e.TotalDistance.Y;

				Point currentOrigin = _nativeFrame.Location + (Size)e.TotalDistance;
				var cellRect = new Rectangle (currentOrigin, _nativeFrame.Size);
				var currentDragOver = FindItemDataUnderRectangle(cellRect);
				if (currentDragOver != null && currentDragOver.Item != _nullItem && currentDragOver.Item!=null) {
					//System.Diagnostics.Debug.WriteLine ("current=[{0}]", currentDragOver.Item.Title);
					// can we drop here?
					bool canDrop = true;
					if (CanDrop != null)
						canDrop = CanDrop(this, currentDragOver.Item.Source, currentDragOver.DeepIndex);
					if (canDrop) {
						// yes: put the NullItem here
						_displayedItemsSource.DeepRemove(_nullItem);
						_displayedItemsSource.DeepInsert (currentDragOver.DeepIndex, _nullItem);
					} else if (_displayedItemsSource.DeepContains(_nullItem) && !_displayedItemsSource.DeepIndexOf (_nullItem).SequenceEqual (_longPress.DeepIndex)) {
						// no: put the NullItem at the location where we started 
						_displayedItemsSource.DeepRemove(_nullItem);
						_displayedItemsSource.DeepInsert (_longPress.DeepIndex, _nullItem);
					}
				}

				if (cellRect.Top < 40 && e.DeltaDistance.Y <= 2)
					ScrollSpeed (cellRect.Top - 40);
				else if (cellRect.Bottom + 40 > Height && e.DeltaDistance.Y >= -2)
					ScrollSpeed (cellRect.Bottom + 40 - Height);
				else
					ScrollSpeed (0);
			}
		}


		void OnLongPressed(object sender, LongPressEventArgs e) {
			//System.Diagnostics.Debug.WriteLine ("LONGPRESSED ["+e.Listener.Element+"]");

			if (_longPress != null) {
				ScrollSpeed(0);
				ScrollEnabled = true;

				// next two lines are for Android - without them, if you try to drag the same cell twice, the ModalPopup content is blank.
				var blankView = ItemTemplate.MakeContentView (_blankItem);
				_popup.Content = blankView;

				_popup.IsVisible = false;

				// return to our pre-drag state
				var nullIndex = _displayedItemsSource.DeepIndexOf (_nullItem);
				_displayedItemsSource.DeepRemove (_nullItem);
				_displayedItemsSource.DeepInsert (_longPress.DeepIndex, _longPress.Item);
				_displayedItemsSource.NotifySourceOfChanges = true;
				if (!nullIndex.SequenceEqual (_longPress.DeepIndex)) {
					// we made a allowed move, so make that move
					_displayedItemsSource.DeepRemove(_longPress.Item);
					_displayedItemsSource.DeepInsert (nullIndex, _longPress.Item);
				}
				_longPress = null;
				_popup.Content = null;
			}

		}
		#endregion
	}
}

