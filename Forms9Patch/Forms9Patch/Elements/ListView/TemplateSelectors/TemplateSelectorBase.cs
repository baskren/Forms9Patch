using System;
using System.ComponentModel;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Reflection;

namespace Forms9Patch
{
    /// <summary>
    /// Data template selector: Used to match types of objects with the types of views that will be used to display them in a ListView.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class TemplateSelectorBase : Xamarin.Forms.DataTemplateSelector
    {

        // this is here in preparation for the day Xamarin fixes ListView to support template selectors for ListView.GroupHeaderTempate on all platforms

        /*
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
		*/

        /// <summary>
        /// The cell templates.
        /// </summary>
        protected readonly Dictionary<Type, DataTemplate> _cellTemplates = new Dictionary<Type, DataTemplate>();
        readonly Dictionary<Type, Type> _contentTypes = new Dictionary<Type, Type>();
        readonly DataTemplate _unknownTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.DataTemplateSelector"/> class.
        /// </summary>
        protected TemplateSelectorBase()
        {
            //Add (typeof(NullItem), typeof (NullCellView));
            var viewType = typeof(NullItemCellView);
            var cellType = typeof(ItemCell<>).MakeGenericType(new[] { viewType });
            var template = new DataTemplate(cellType);
            var itemType = typeof(NullItemWrapper);
            _cellTemplates[itemType] = template;
            _contentTypes[itemType] = viewType;

            viewType = typeof(BlankCellView);
            cellType = typeof(ItemCell<>).MakeGenericType(new[] { viewType });
            template = new DataTemplate(cellType);
            itemType = typeof(BlankItemWrapper);
            _cellTemplates[itemType] = template;
            _contentTypes[itemType] = viewType;

            viewType = typeof(TextCellViewContent);
            cellType = typeof(ItemCell<>).MakeGenericType(new[] { viewType });
            template = new DataTemplate(cellType);
            itemType = typeof(ItemWrapper<string>);
            _cellTemplates[itemType] = template;
            _contentTypes[itemType] = viewType;
            _unknownTemplate = template;
        }

        /// <summary>
        /// Removes the factory defaults, to make more space for Andriod implementation.
        /// </summary>
        public void RemoveFactoryDefaults()
        {
            _cellTemplates.Remove(typeof(NullItemWrapper));
            _contentTypes.Remove(typeof(NullItemWrapper));
            _cellTemplates.Remove(typeof(BlankItemWrapper));
            _contentTypes.Remove(typeof(BlankItemWrapper));
            _cellTemplates.Remove(typeof(ItemWrapper<string>));
            _contentTypes.Remove(typeof(ItemWrapper<string>));
        }

        /// <summary>
        /// Clear all cell templates
        /// </summary>
        public void Clear()
        {
            _cellTemplates.Clear();
            _contentTypes.Clear();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.GroupTemplate"/> class.
        /// </summary>
        /// <param name="groupContentViewType">Group content view type.</param>
        public TemplateSelectorBase(Type groupContentViewType) : this()
        {
            var itemType = typeof(GroupWrapper);
            var cellType = typeof(ItemCell<>).MakeGenericType(new[] { groupContentViewType });
            var template = new DataTemplate(cellType);
            _cellTemplates[itemType] = template;
            _contentTypes[itemType] = groupContentViewType;
        }

        /// <summary>
        /// Add to the DetaTemplateSelector a viewType that will be used to display any objects of itemBaseType.
        /// </summary>
        /// <param name="dataType">Item base type.</param>
        /// <param name="viewType">View type.</param>

        protected void Add(Type dataType, Type viewType)
        {
            // Type itemType = dataType;
            //itemType = itemBaseType;
            //itemType = typeof(ItemWrapper<>).MakeGenericType(new Type[] { itemBaseType });
            if (_cellTemplates.Count > 20 && !_contentTypes.ContainsKey(dataType))
                throw new IndexOutOfRangeException("Xamarin.Forms.Platforms.Android does not permit more than 20 DataTemplates per ListView");
            var cellType = typeof(ItemCell<>).MakeGenericType(new[] { viewType });
            var template = new DataTemplate(cellType);
            _cellTemplates[dataType] = template;
            _contentTypes[dataType] = viewType;
        }



        /// <summary>
        /// Triggered when a Forms9Patch.ListView needs to create a view template for an item.
        /// </summary>
        /// <returns>The select template.</returns>
        /// <param name="item">Item.</param>
        /// <param name="container">Container.</param>
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            //var group = item as Group;
            //Type itemType = group?.Source.GetType () ?? item.GetType ();
            //Type itemType = ((item as Item)?.Source ?? item).GetType();
            if (item is ItemWrapper itemWrapper)
            {
                var itemType = itemWrapper.GetType();
                if (_cellTemplates.ContainsKey(itemType))
                    return _cellTemplates[itemType];
                var source = itemWrapper.Source;
                if (source != null)
                {
                    var sourceType = source.GetType();
                    if (_cellTemplates.ContainsKey(sourceType))
                        return _cellTemplates[sourceType];
                    if (sourceType.IsConstructedGenericType)
                    {
                        var genericSourceType = sourceType.GetGenericTypeDefinition();
                        if (_cellTemplates.ContainsKey(genericSourceType))
                            return _cellTemplates[genericSourceType];
                        //var genericItemType = typeof(ItemWrapper<>).MakeGenericType(new[] { genericSourceType });
                        //if (_cellTemplates.ContainsKey(genericItemType))
                        //	return _cellTemplates[genericItemType];
                    }
                    var template = TemplateForType(source.GetType());
                    return template;
                }
                return _cellTemplates[typeof(BlankItemWrapper)];
            }
            //throw new KeyNotFoundException("No data template found.  item=["+itemWrapper+"]  item.source=["+(itemWrapper?.Source)+"]");
            return _cellTemplates[typeof(ItemWrapper<string>)];
        }

        DataTemplate TemplateForType(Type type)
        {
            if (_cellTemplates.ContainsKey(type))
                return _cellTemplates[type];
            var baseType = type.GetTypeInfo().BaseType;
            //if (baseType == null || baseType == typeof(System.Object))
            //return _cellTemplates[typeof(NullItem)];
            if (baseType == null)
                return _cellTemplates[typeof(BlankItemWrapper)];
            else
                return _unknownTemplate;
            //return TemplateForType (baseType);
        }

        internal BaseCellView MakeContentView(ItemWrapper item)
            => (item.GetType() is Type itemType && _contentTypes.ContainsKey(itemType))
                ? new BaseCellView
                {
                    ContentView = (View)Activator.CreateInstance(_contentTypes[itemType]),
                    BindingContext = item
                }
                : null;

    }
}

