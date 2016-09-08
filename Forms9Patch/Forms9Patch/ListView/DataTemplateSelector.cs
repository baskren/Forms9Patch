using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;

namespace Forms9Patch
{
	/// <summary>
	/// Data template selector: Used to match types of objects with the types of views that will be used to display them in a ListView.
	/// </summary>
	public class DataTemplateSelector : Xamarin.Forms.DataTemplateSelector
	{
		readonly Dictionary <Type, DataTemplate> _cellTemplates = new Dictionary<Type, DataTemplate>();
		readonly Dictionary <Type, Type> _contentTypes = new Dictionary<Type, Type> ();

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Forms9Patch.DataTemplateSelector"/> class.
		/// </summary>
		public DataTemplateSelector() {
			//Add (typeof(NullItem), typeof (NullCellView));
			var viewType = typeof(NullCellView);
			var itemType = typeof(NullItem);
			Type cellType = typeof(Cell<>).MakeGenericType (new[] { viewType });
			var template = new DataTemplate (cellType);
			_cellTemplates [itemType] = template;
			_contentTypes [itemType] = viewType;

			viewType = typeof(BlankCellView);
			itemType = typeof(BlankItem);
			cellType = typeof(Cell<>).MakeGenericType (new[] { viewType });
			template = new DataTemplate (cellType);
			_cellTemplates [itemType] = template;
			_contentTypes [itemType] = viewType;
		}

		/// <summary>
		/// Add to the DetaTemplateSelector a viewType that will be used to display any objects of itemBaseType.
		/// </summary>
		/// <param name="itemBaseType">Item base type.</param>
		/// <param name="viewType">View type.</param>
		public void Add(Type itemBaseType, Type viewType) {
			if (_cellTemplates.Count > 19)
				throw new IndexOutOfRangeException("Xamarin.Forms.Platforms.Android does not permit more than 20 DataTemplates per ListView");
			Type itemType;
			//var iList = itemBaseType as IList;
			//if (typeof(IList).GetTypeInfo().IsAssignableFrom(itemBaseType.GetTypeInfo()))
				itemType = itemBaseType;
			//else
			//	itemType = typeof(Item<>).MakeGenericType (new [] { itemBaseType });
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
			Type itemType = ((item as Item)?.Source ?? item).GetType();
			//var result = _cellTemplates.ContainsKey (itemType) ? _cellTemplates [itemType] : _cellTemplates[typeof(NullItem)];
			return TemplateForType (itemType);
		}

		DataTemplate TemplateForType(Type type) {
			if (_cellTemplates.ContainsKey (type))
				return _cellTemplates [type];
			var baseType = type.GetTypeInfo ().BaseType;
			if (baseType == null || baseType == typeof(System.Object))
				return _cellTemplates[typeof(NullItem)];
			return TemplateForType (baseType);
		}

		internal BaseCellView MakeContentView(Item item) {
			Type itemType = item.GetType ();
			if (_contentTypes.ContainsKey(itemType)) {
				Type contentType = _contentTypes [itemType];
				//var cellType = typeof(Cell<>).MakeGenericType (new[] { contentType });
				//var cellView = (BaseCellView)Activator.CreateInstance (cellType);
				//cellView.BindingContext = item;
				//return cellView;
				//System.Diagnostics.Debug.WriteLine("\t\tMakeContentView({0}) enter",item);
				var baseCellView = new BaseCellView();
				baseCellView.Content = (View)Activator.CreateInstance (contentType);
				baseCellView.BindingContext = item;
				//System.Diagnostics.Debug.WriteLine("\t\tMakeContentView({0}) exit",item);
				return baseCellView;
			}
			return null;
		}


		//class Cell<TContent> : ViewCell where TContent : ContentView, new() {
		class Cell<TContent> : ViewCell where TContent : Xamarin.Forms.View, new() {
			
			internal BaseCellView BaseCellView = new BaseCellView ();

			/// <summary>
			/// Initializes a new instance of the <see cref="T:Forms9Patch.DataTemplateSelector.Cell`1"/> class.
			/// </summary>
			public Cell ()
			{
				//System.Diagnostics.Debug.WriteLine("\t\t\t{0} start", this.GetType());
				View = BaseCellView;
				BaseCellView.Content = new TContent();	
				//System.Diagnostics.Debug.WriteLine("\t\t\t{0} exit", this.GetType());
			}

			/// <summary>
			/// Triggered before a property is changed.
			/// </summary>
			/// <param name="propertyName">Property name.</param>
			protected override void OnPropertyChanging (string propertyName = null)
			{
				base.OnPropertyChanging (propertyName);
				if (propertyName == BindableObject.BindingContextProperty.PropertyName && View != null)
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
			}
		}

	}
}

