using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Reflection;

namespace Forms9Patch
{
	/// <summary>
	/// Data template selector: Used to match types of objects with the types of views that will be used to display them in a ListView.
	/// </summary>
	public class GroupTemplate : Xamarin.Forms.DataTemplateSelector
	{

		Type _baseCellViewType = typeof(BaseCellView);
		internal Type BaseCellViewType
		{
			get { return _baseCellViewType; }
			set
			{
				var baseCellView = (BaseCellView)Activator.CreateInstance(value);
				if (baseCellView == null)
					throw new InvalidCastException("BaseCellViewType must be derived from Forms9Patch.BaseCellView");
				_baseCellViewType = value;
			}
		}

		protected readonly Dictionary <Type, DataTemplate> _cellTemplates = new Dictionary<Type, DataTemplate>();
		readonly Dictionary <Type, Type> _contentTypes = new Dictionary<Type, Type> ();
		readonly DataTemplate _unknownTemplate;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.DataTemplateSelector"/> class.
		/// </summary>
		protected GroupTemplate() {
			//Add (typeof(NullItem), typeof (NullCellView));
			var viewType = typeof(NullItemCellView);
			Type cellType = typeof(Cell<>).MakeGenericType (new[] { viewType });
			var template = new DataTemplate (cellType);
			var itemType = typeof(NullItemWrapper);
			_cellTemplates [itemType] = template;
			_contentTypes [itemType] = viewType;

			viewType = typeof(BlankCellView);
			cellType = typeof(Cell<>).MakeGenericType (new[] { viewType });
			template = new DataTemplate (cellType);
			itemType = typeof(BlankItemWrapper);
			_cellTemplates [itemType] = template;
			_contentTypes [itemType] = viewType;

			viewType = typeof(TextCellViewContent);
			cellType = typeof(Cell<>).MakeGenericType(new[] { viewType });
			template = new DataTemplate(cellType);
			itemType = typeof(ItemWrapper<string>);
			_cellTemplates[itemType] = template;
			_contentTypes[itemType] = viewType;
			_unknownTemplate = template;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.GroupTemplate"/> class.
		/// </summary>
		/// <param name="groupContentViewType">Group content view type.</param>
		public GroupTemplate(Type groupContentViewType) : this()
		{
			var itemType = typeof(GroupWrapper);
			Type cellType = typeof(Cell<>).MakeGenericType(new[] { groupContentViewType });
			var template = new DataTemplate(cellType);
			_cellTemplates[itemType] = template;
			_contentTypes[itemType] = groupContentViewType;
		}

		/// <summary>
		/// Add to the DetaTemplateSelector a viewType that will be used to display any objects of itemBaseType.
		/// </summary>
		/// <param name="itemBaseType">Item base type.</param>
		/// <param name="viewType">View type.</param>

		protected void Add(Type itemBaseType, Type viewType) {
			if (_cellTemplates.Count > 20)
				throw new IndexOutOfRangeException("Xamarin.Forms.Platforms.Android does not permit more than 20 DataTemplates per ListView");
			Type itemType;
			//itemType = itemBaseType;
			itemType = typeof(ItemWrapper<>).MakeGenericType(new Type[] { itemBaseType });
			Type cellType = typeof(Cell<>).MakeGenericType (new [] { viewType });
			var template = new DataTemplate (cellType);
			_cellTemplates [itemType] = template;
			_contentTypes [itemType] = viewType;
		}


		/// <summary>
		/// Triggered when a Forms9Patch.ListView needs to create a view template for an item.
		/// </summary>
		/// <returns>The select template.</returns>
		/// <param name="item">Item.</param>
		/// <param name="container">Container.</param>
		protected override DataTemplate OnSelectTemplate (object item, BindableObject container)
		{
			//var group = item as Group;
			//Type itemType = group?.Source.GetType () ?? item.GetType ();
			//Type itemType = ((item as Item)?.Source ?? item).GetType();
			var itemWrapper = item as ItemWrapper;
			if (itemWrapper != null)
			{
				Type itemType = itemWrapper.GetType();
				if (_cellTemplates.ContainsKey(itemType))
					return _cellTemplates[itemType];
				var source = itemWrapper.Source;
				if (source != null)
				{
					var template = TemplateForType(source.GetType());
					return template;
				}
				return _cellTemplates[typeof(BlankItemWrapper)];
			}
			throw new KeyNotFoundException("No data template found.  item=["+itemWrapper+"]  item.source=["+(itemWrapper?.Source)+"]");
		}

		DataTemplate TemplateForType(Type type) {
			if (_cellTemplates.ContainsKey (type))
				return _cellTemplates [type];
			var baseType = type.GetTypeInfo ().BaseType;
			//if (baseType == null || baseType == typeof(System.Object))
			//return _cellTemplates[typeof(NullItem)];
			if (baseType == null)
				return _cellTemplates[typeof(BlankItemWrapper)];
			else
				return _unknownTemplate;
			//return TemplateForType (baseType);
		}

		internal BaseCellView MakeContentView(ItemWrapper item) {
			Type itemType = item.GetType ();
			if (_contentTypes.ContainsKey(itemType)) {
				Type contentType = _contentTypes [itemType];
				//var cellType = typeof(Cell<>).MakeGenericType (new[] { contentType });
				//var cellView = (BaseCellView)Activator.CreateInstance (cellType);
				//cellView.BindingContext = item;
				//return cellView;
				//System.Diagnostics.Debug.WriteLine("\t\tMakeContentView({0}) enter",item);
				//var baseCellView = new BaseCellView();
				var baseCellView = (BaseCellView)Activator.CreateInstance(BaseCellViewType);
				baseCellView.Content = (View)Activator.CreateInstance (contentType);
				baseCellView.BindingContext = item;
				//System.Diagnostics.Debug.WriteLine("\t\tMakeContentView({0}) exit",item);
				return baseCellView;
			}
			return null;
		}


		//class Cell<TContent> : ViewCell where TContent : ContentView, new() {
		class Cell<TContent> : ViewCell where TContent : View, new() {
			
			internal BaseCellView BaseCellView = new BaseCellView ();
			ICellContentView _iCellContent;

			/// <summary>
			/// Initializes a new instance of the <see cref="T:Forms9Patch.DataTemplateSelector.Cell`1"/> class.
			/// </summary>
			public Cell ()
			{
				//System.Diagnostics.Debug.WriteLine("\t\t\t{0} start", this.GetType());
				View = BaseCellView;
				BaseCellView.Content = new TContent();
				_iCellContent = BaseCellView.Content as ICellContentView;
				if (_iCellContent != null && _iCellContent.CellHeight >= 0)
					Height = _iCellContent.CellHeight;
				BaseCellView.Content.PropertyChanged += (sender, e) =>
				{
					if (e.PropertyName == VisualElement.HeightRequestProperty.PropertyName)
						SetHeight();
				};
				//BaseCellView.BackgroundColor = Color.Pink;
				//System.Diagnostics.Debug.WriteLine("\t\t\t{0} exit", this.GetType());
			}

			/// <summary>
			/// Triggered before a property is changed.
			/// </summary>
			/// <param name="propertyName">Property name.</param>
			protected override void OnPropertyChanging (string propertyName = null)
			{
				base.OnPropertyChanging (propertyName);
				if (propertyName == BindingContextProperty.PropertyName && View != null)
					View.BindingContext = null;
			}

			/// <summary>
			/// Triggered by a change in the binding context.
			/// </summary>
			protected override void OnBindingContextChanged ()
			{
				base.OnBindingContextChanged ();
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
						Height = _iCellContent.CellHeight;
					else
						Height = iItem.RowHeight;
				}
				View.HeightRequest = Height;
			}
		}

	}
}

