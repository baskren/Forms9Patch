using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.ObjectModel;
using P42.Utils;
using Xamarin.Forms;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Forms9Patch
{
    [DesignTimeVisible(true)]
    class GroupWrapper : ItemWrapper<object>, IList<ItemWrapper>, ICollection<ItemWrapper>, IEnumerable<ItemWrapper>, IEnumerable, IList, ICollection, IReadOnlyList<ItemWrapper>, IReadOnlyCollection<ItemWrapper>, INotifyCollectionChanged
    {

        #region Properties

        #region SourceSubPropertyMap property
        /// <summary>
        /// backing store for SourceSubPropertyMap property
        /// </summary>
        public static readonly BindableProperty SourceSubPropertyMapProperty = BindableProperty.Create(nameof(SourceSubPropertyMap), typeof(List<string>), typeof(GroupWrapper), default(List<string>));
        /// <summary>
        /// Gets/Sets the SourceSubPropertyMap property
        /// </summary>
        public List<string> SourceSubPropertyMap
        {
            get { return (List<string>)GetValue(SourceSubPropertyMapProperty); }
            set { SetValue(SourceSubPropertyMapProperty, value); }
        }
        #endregion SourceSubPropertyMap property

        #region SubGroupType property
        /// <summary>
        /// backing store for SubGroupType property
        /// </summary>
        public static readonly BindableProperty SubGroupTypeProperty = BindableProperty.Create(nameof(SubGroupType), typeof(Type), typeof(GroupWrapper), default(Type));
        /// <summary>
        /// Gets/Sets the SubGroupType property
        /// </summary>
        public Type SubGroupType
        {
            get { return (Type)GetValue(SubGroupTypeProperty); }
            set { SetValue(SubGroupTypeProperty, value); }
        }
        #endregion SubGroupType property

        #region GroupHeader properties
        /// <summary>
        /// backing store for GroupHeaderCellHeight property
        /// </summary>
        public static readonly BindableProperty RequestedGroupHeaderRowHeightProperty = BindableProperty.Create(nameof(RequestedGroupHeaderRowHeight), typeof(double), typeof(GroupWrapper), 40.0);
        /// <summary>
        /// Gets/Sets the GroupHeaderCellHeight property
        /// </summary>
        public double RequestedGroupHeaderRowHeight
        {
            get { return (double)GetValue(RequestedGroupHeaderRowHeightProperty); }
            set { SetValue(RequestedGroupHeaderRowHeightProperty, value); }
        }

        /// <summary>
        /// The group header background color property.
        /// </summary>
        public static readonly BindableProperty GroupHeaderBackgroundColorProperty = BindableProperty.Create(nameof(GroupHeaderBackgroundColor), typeof(Color), typeof(GroupWrapper), Color.DarkGray);
        public Color GroupHeaderBackgroundColor
        {
            get { return (Color)GetValue(GroupHeaderBackgroundColorProperty); }
            set { SetValue(GroupHeaderBackgroundColorProperty, value); }
        }
        #endregion GroupHeader properties

        #endregion


        #region Fields
        readonly ObservableCollection<ItemWrapper> _itemWrappers = new ObservableCollection<ItemWrapper>();

        List<string> _subPropertyMap;

        IEnumerable SourceChildren;

        string childrenPropertyName;
        #endregion


        #region Constructors
        public GroupWrapper(object source, List<string> sourcePropertyMap, Func<object, bool> visibleItemTest = null, Type subgroupType = null) : this()
        {
            VisibilityTest = visibleItemTest;
            ContentType = GroupContentType.Unknown;
            SourceSubPropertyMap = sourcePropertyMap;
            SubGroupType = subgroupType;
            Source = source;
        }

        public GroupWrapper()
        {
            ContentType = GroupContentType.Unknown;
            NotifySourceOfChanges = true;
            _itemWrappers.CollectionChanged += OnCollectionChanged;
        }

        #endregion


        #region Convenience
        internal double BestGuessGroupHeaderHeight()
        {
            if (RenderedRowHeight >= 0)
                return RenderedRowHeight;
            if (RequestedGroupHeaderRowHeight >= 0)
                return RequestedGroupHeaderRowHeight;
            if (Parent is GroupWrapper parent && parent.RequestedGroupHeaderRowHeight >= 0)
                return parent.RequestedRowHeight;
            return BestGuessItemRowHeight();
        }
        #endregion


        #region Property Change Management

        protected override void OnPropertyChanging(string propertyName = null)
        {
            base.OnPropertyChanging(propertyName);
            if (propertyName == SourceProperty.PropertyName && Source != null)
            {
                SourceChildren = string.IsNullOrWhiteSpace(childrenPropertyName) ? Source as IEnumerable : Source.GetPropertyValue(childrenPropertyName) as IEnumerable;
                if (SourceChildren is INotifyCollectionChanged observableCollection)
                    observableCollection.CollectionChanged -= OnSourceCollectionChanged;
            }
        }


        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            #region Data Mapping properties
            if (propertyName == SourceProperty.PropertyName)
            {
                _itemWrappers.Clear();
                if (Source == null)
                {
                    SourceChildren = null;
                    return;
                }
                SourceChildren = string.IsNullOrWhiteSpace(childrenPropertyName) ? Source as IEnumerable : Source.GetPropertyValue(childrenPropertyName) as IEnumerable;
                if (SourceChildren == null)
                    return;
                foreach (var obj in SourceChildren)
                    AddSourceObject(obj);
                if (SourceChildren is INotifyCollectionChanged observableCollection)
                    observableCollection.CollectionChanged += OnSourceCollectionChanged;
            }
            else if (propertyName == SourceSubPropertyMapProperty.PropertyName)
            {
                if (SourceSubPropertyMap != null && SourceSubPropertyMap.Count > 0)
                {
                    _subPropertyMap = SourceSubPropertyMap.GetRange(1, SourceSubPropertyMap.Count - 1);
                    childrenPropertyName = SourceSubPropertyMap[0];
                }
                var source = Source;
                Source = null;
                Source = source;
            }
            else if (propertyName == VisibilityTestProperty.PropertyName || propertyName == SubGroupTypeProperty.PropertyName)
            {
                // delete and reset Source so the VisibilityTest can be applied as the new Source is converted to items
                var source = Source;
                Source = null;
                Source = source;
            }
            #endregion

            #region RowHeight properties
            else if (propertyName == RequestedGroupHeaderRowHeightProperty.PropertyName)
                foreach (var child in this)
                {
                    if (child is GroupWrapper subGroup)
                        subGroup.RequestedGroupHeaderRowHeight = RequestedGroupHeaderRowHeight;
                }
            else if (propertyName == RequestedRowHeightProperty.PropertyName)
                foreach (var child in this)
                    child.RequestedRowHeight = RequestedRowHeight;
            #endregion

            #region Separator properties
            else if (propertyName == SeparatorVisibilityProperty.PropertyName)
                foreach (var child in this)
                    child.SeparatorVisibility = SeparatorVisibility;
            else if (propertyName == RequestedSeparatorHeightProperty.PropertyName)
                foreach (var child in this)
                    child.RequestedSeparatorHeight = RequestedSeparatorHeight;
            else if (propertyName == SeparatorLeftIndentProperty.PropertyName)
                foreach (var child in this)
                    child.SeparatorLeftIndent = SeparatorLeftIndent;
            else if (propertyName == SeparatorRightIndentProperty.PropertyName)
                foreach (var child in this)
                    child.SeparatorRightIndent = SeparatorRightIndent;
            else if (propertyName == SeparatorColorProperty.PropertyName)
                foreach (var child in this)
                    child.SeparatorColor = SeparatorColor;
            #endregion

            #region Background Color properties
            else if (propertyName == GroupHeaderBackgroundColorProperty.PropertyName)
            {
                foreach (var child in this)
                    if (child is GroupWrapper childGroup)
                        childGroup.GroupHeaderBackgroundColor = GroupHeaderBackgroundColor;
            }
            else if (propertyName == CellBackgroundColorProperty.PropertyName)
                foreach (var child in this)
                    child.CellBackgroundColor = CellBackgroundColor;
            else if (propertyName == SelectedCellBackgroundColorProperty.PropertyName)
                foreach (var child in this)
                    child.SelectedCellBackgroundColor = SelectedCellBackgroundColor;
            #endregion

        }
        #endregion


        #region Member (ItemWrapper) Property Change Notification
        public event Xamarin.Forms.PropertyChangingEventHandler ItemWrapperPropertyChanging;
        void OnItemWrapperPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
        {
            ItemWrapperPropertyChanging?.Invoke(sender, e);
        }

        public event PropertyChangedEventHandler ItemWrapperPropertyChanged;
        void OnItemWrapperPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ItemWrapperPropertyChanged?.Invoke(sender, e);
        }

        #endregion


        #region Source to ItemWrapper coupling
        bool _notifySourceOfChanges;
        public bool NotifySourceOfChanges
        {
            get => _notifySourceOfChanges;
            set
            {
                foreach (var itemWrapper in this)
                {
                    if (itemWrapper is GroupWrapper groupWrapper)
                        groupWrapper.NotifySourceOfChanges = value;
                }
                _notifySourceOfChanges = value;
            }
        }

        public bool IgnoreSourceChanges
        {
            get;
            set;
        }

        /// <summary>
        /// The visibility test property backing store.
        /// </summary>
        public static readonly BindableProperty VisibilityTestProperty = BindableProperty.Create(nameof(VisibilityTest), typeof(Func<object, bool>), typeof(GroupWrapper), default(Func<object, bool>));
        /// <summary>
        /// Gets or sets the test used to determine if a source item or group will be visible.
        /// </summary>
        /// <value>The visibility test.</value>
        public Func<object, bool> VisibilityTest
        {
            get { return (Func<object, bool>)GetValue(VisibilityTestProperty); }
            set { SetValue(VisibilityTestProperty, value); }
        }


        // note: Even though a items in a group source may not be unique to  (ex: Source= { a, a, "pizza", null, null }), a GroupWrapper's ItemWrappers are because they are each created from a new instance of ItemWrapper.
        /*
        int SourceCount()
        {
            int sourceCount;
            if (SourceChildren is ICollection<IEnumerable> iCollectionIenumerble)
                sourceCount = iCollectionIenumerble.Count;
            else
            {
                if (SourceChildren is ICollection iCollection)
                    sourceCount = iCollection.Count;
                else
                {
                    sourceCount = 0;
                    foreach (var obj in SourceChildren)
                        sourceCount++;
                }
            }
            return sourceCount;
        }
        */

        int SourceIndexOf(ItemWrapper itemWrapper)
        {
            if (SourceChildren == null)
                return -1;
            if (!Contains(itemWrapper))
                return -1;

            if (SourceChildren is IList<IEnumerable> iListiEnumerblle)
                return iListiEnumerblle.IndexOf((IEnumerable)itemWrapper.Source);
            if (SourceChildren is IList iList)
                return iList.IndexOf(itemWrapper.Source);

            var index = 0;
            foreach (var obj in SourceChildren)
            {
                if (obj == itemWrapper.Source)
                    return index;
                index++;
            }
            return -1;
        }

        void SourceInsert(ItemWrapper itemWrapper)
        {
            if (SourceChildren == null)
                throw new MissingMemberException("Cannot SourceInsert a source item into a null SourceChildren");
            if (SourceChildren is IList<IEnumerable> iListIEnumerable)
            {
                var sourceIndex = iListIEnumerable.IndexOf((IEnumerable)itemWrapper.Source);
                if (sourceIndex == -1)
                    throw new MissingMemberException("Cannot SourceInsert a source item that is not a member of SourceChildren");
                IgnoreSourceChanges = true;
                iListIEnumerable.Insert(sourceIndex, (IEnumerable)itemWrapper.Source);
                IgnoreSourceChanges = false;
                return;
            }
            if (SourceChildren is IList iList)
            {
                var sourceIndex = iList.IndexOf(itemWrapper.Source);
                if (sourceIndex == -1)
                    throw new MissingMemberException("Cannot SourceInsert a source item that is not a member of SourceChildren");
                IgnoreSourceChanges = true;
                iList.Insert(sourceIndex, itemWrapper.Source);
                IgnoreSourceChanges = false;
            }
        }

        void SourceAdd(ItemWrapper itemWrapper)
        {
            //System.Diagnostics.Debug.WriteLine ("[" + this + "].MateAdd(" + itemWrapper + ")");
            if (SourceChildren == null)
                throw new MissingMemberException("Cannot SourceAdd a source item into a null SourceChildren");
            if (SourceChildren is ICollection<IEnumerable> iCollectionIEnumerable)
            {
                IgnoreSourceChanges = true;
                iCollectionIEnumerable.Add((IEnumerable)itemWrapper.Source);
                IgnoreSourceChanges = false;
                return;
            }
            if (SourceChildren is IList iList)
            {
                IgnoreSourceChanges = true;
                iList.Add(itemWrapper.Source);
                IgnoreSourceChanges = false;
            }
        }

        void SourceRemove(ItemWrapper itemWrapper)
        {
            //System.Diagnostics.Debug.WriteLine ("[" + this + "].MateRemove(" + itemWrapper + ")");
            if (SourceChildren == null)
                throw new MissingMemberException("Cannot SourceRemove a source item from a null SourceChildren");
            if (SourceChildren is ICollection<IEnumerable> iCollectionIEnumerable)
            {
                IgnoreSourceChanges = true;
                iCollectionIEnumerable.Remove((IEnumerable)itemWrapper.Source);
                IgnoreSourceChanges = false;
            }
            if (SourceChildren is IList iList)
            {
                IgnoreSourceChanges = true;
                iList.Remove(itemWrapper.Source);
                IgnoreSourceChanges = false;
            }
        }
        #endregion


        #region add/remove common actions
        void CommonNew(ItemWrapper itemWrapper)
        {
            if (itemWrapper == null)
                return;
            SubscribeToItemWrapperSourcePropertyChanged(itemWrapper);
            itemWrapper.PropertyChanged += OnItemWrapperPropertyChanged;
            itemWrapper.PropertyChanging += OnItemWrapperPropertyChanging;
            itemWrapper.Tapped += OnTapped;
            itemWrapper.LongPressed += OnLongPressed;
            itemWrapper.LongPressing += OnLongPressing;
            itemWrapper.SwipeMenuItemTapped += OnSwipeMenuItemTapped;
            itemWrapper.Panned += OnPanned;
            itemWrapper.Panning += OnPanning;
            itemWrapper.BindingContext = this;
            itemWrapper.Parent = this;
            //if (itemWrapper.GetType()==typeof(GroupWrapper))
            if (itemWrapper is GroupWrapper groupWrapper)
            {
                //var groupWrapper = itemWrapper as GroupWrapper;
                groupWrapper.VisibilityTest = groupWrapper.VisibilityTest ?? VisibilityTest;
                groupWrapper.SubGroupType = groupWrapper.SubGroupType ?? SubGroupType;
                groupWrapper.SourceSubPropertyMap = _subPropertyMap;
                groupWrapper.RequestedGroupHeaderRowHeight = RequestedGroupHeaderRowHeight;
                groupWrapper.GroupHeaderBackgroundColor = GroupHeaderBackgroundColor;
            }

            #region RowHeight properties
            // RequestedGroupHeight is above
            itemWrapper.RequestedRowHeight = RequestedRowHeight;
            #endregion

            #region Separator properties
            itemWrapper.SeparatorVisibility = SeparatorVisibility;
            itemWrapper.RequestedSeparatorHeight = RequestedSeparatorHeight;
            itemWrapper.SeparatorLeftIndent = SeparatorLeftIndent;
            itemWrapper.SeparatorRightIndent = SeparatorRightIndent;
            itemWrapper.SeparatorColor = SeparatorColor;
            #endregion

            #region BackgroundColor properties
            // GroupHeaderBackgroundColor is above
            itemWrapper.CellBackgroundColor = CellBackgroundColor;
            itemWrapper.SelectedCellBackgroundColor = SelectedCellBackgroundColor;
            #endregion
        }

        void CommonAdd(ItemWrapper itemWrapper)
        {
            CommonNew(itemWrapper);
            if (NotifySourceOfChanges)
                SourceAdd(itemWrapper);
        }

        void CommonInsert(ItemWrapper itemWrapper)
        {
            CommonNew(itemWrapper);
            if (NotifySourceOfChanges)
                SourceInsert(itemWrapper);
        }

        void CommonRemove(ItemWrapper itemWrapper)
        {
            if (itemWrapper == null)
                return;
            UnsubscribeToItemWrapperSourcePropertyChanged(itemWrapper);
            if (NotifySourceOfChanges)
                SourceRemove(itemWrapper);
            itemWrapper.PropertyChanged -= OnItemWrapperPropertyChanged;
            itemWrapper.PropertyChanging -= OnItemWrapperPropertyChanging;
            itemWrapper.Tapped -= OnTapped;
            itemWrapper.LongPressed -= OnLongPressed;
            itemWrapper.LongPressing -= OnLongPressing;
            itemWrapper.SwipeMenuItemTapped -= OnSwipeMenuItemTapped;
            itemWrapper.Panned -= OnPanned;
            itemWrapper.Panning -= OnPanning;
            itemWrapper.BindingContext = null;
            itemWrapper.Parent = null;
        }


        #endregion



        void Reindex(int index = 0)
        {
            for (int i = index; i < _itemWrappers.Count; i++)
            {
                var itemWrapper = _itemWrappers[i];
                itemWrapper.Index = i;
                var wasLastItem = itemWrapper.IsLastItem;
                //_itemWrappers[i].SignalPropertyChanged(ItemWrapper.SeparatorVisibilityProperty.PropertyName);
                //_itemWrappers[i].SetValueCore(ItemWrapper.SeparatorVisibilityProperty, i < _itemWrappers.Count - 1, (Xamarin.Forms.Internals.SetValueFlags)15);
                var isLastItem = (i >= _itemWrappers.Count - 1);
                var isLastGroup = Parent != null && this.IsLastItem;
                itemWrapper.IsLastItem = isLastItem && !isLastGroup;

                if (itemWrapper is GroupWrapper groupWrapper && wasLastItem != itemWrapper.IsLastItem)
                    groupWrapper.Reindex();
            }
        }




        #region IList<T> implementation
        public int IndexOf(ItemWrapper itemWrapper)
        {
            // need to return -1 if itemWrapper is not in this group to communicate to DeepIndexOf to begin searching subgroups
            return _itemWrappers.IndexOf(itemWrapper);
        }

        public void Insert(int index, ItemWrapper itemWrapper)
        {
            if (itemWrapper == null)
                return;
            if (Contains(itemWrapper))
                throw new NotSupportedException();
            CommonInsert(itemWrapper);
            _itemWrappers.Insert(index, itemWrapper);
            //Reindex(index);
        }

        public void RemoveAt(int index)
        {
            if (_itemWrappers[index] is ItemWrapper itemWrapper)
            {
                CommonRemove(itemWrapper);
                itemWrapper.Index = -1;
                _itemWrappers.RemoveAt(index);
            }
        }

        public ItemWrapper this[int index]
        {
            get
            {
                if (index < 0 || index > _itemWrappers.Count - 1)
                    return null;
                return _itemWrappers[index];
            }
            set
            {
                if (DeepContains(value) || index < 0 || index > Count)
                    // will this create problems for move/swap operations?  I think so!  
                    throw new NotSupportedException();
                if (index < Count)
                {
                    _itemWrappers[index].Index = -1;
                    CommonRemove(_itemWrappers[index]);
                }
                //System.Diagnostics.Debug.WriteLine ("\t\tsetting _item start");
                CommonAdd(value);
                value.Index = index;
                _itemWrappers[index] = value;
                //_itemWrappers[index].Index = index;
                //System.Diagnostics.Debug.WriteLine ("\t\tsetting _item finish");
            }
        }
        #endregion


        #region Source object coupling
        public GroupContentType ContentType { get; set; }

        internal enum GroupContentType
        {
            Unknown,
            Lists,
            Objects
        }

        void VerifyContentType(GroupContentType contentType)
        {
            if (ContentType == GroupContentType.Unknown)
                ContentType = contentType;

            //if (ContentType != contentType)
            //    throw new InvalidCastException("Cannot group IEnumerable with non-IEnumerable objects");
        }

        ItemWrapper CreateItemWrapper(object sourceObject)
        {

            string propertyName = null;
            List<string> subgroupPropertyMap;
            if (_subPropertyMap != null && _subPropertyMap.Count > 0)
            {
                subgroupPropertyMap = new List<string>(_subPropertyMap) { [0] = null };
                propertyName = _subPropertyMap[0];
            }

            var children = string.IsNullOrWhiteSpace(propertyName) ? sourceObject : sourceObject.GetPropertyValue(propertyName);
            if (!(children is string))
            {
                if (children is IEnumerable iEnumerable)
                {
                    if (SubGroupType == null || children.GetType() == SubGroupType)
                    {
                        // groups
                        VerifyContentType(GroupContentType.Lists);
                        return new GroupWrapper(sourceObject, _subPropertyMap, VisibilityTest, SubGroupType);
                    }
                }
            }
            // items
            VerifyContentType(GroupContentType.Objects);
            ItemWrapper itemWrapper;
            if (sourceObject == null)
                itemWrapper = new ItemWrapper<object>();
            else
            {
                var objType = sourceObject.GetType();
                var itemType = typeof(ItemWrapper<>).MakeGenericType(new[] { objType });

                // Approach 1
                itemWrapper = (ItemWrapper)Activator.CreateInstance(itemType);

                // Approach 2
                //itemWrapper = (ItemWrapper)BaitAndSwitch.ObjectFactory.Constructor(itemType, new object[] { });

                // Approach 3

                itemWrapper.Source = sourceObject;
            }
            return itemWrapper;
        }

        void AddSourceObject(object sourceObject)
        {
            if (VisibilityTest == null || VisibilityTest(sourceObject))
            {
                var before = NotifySourceOfChanges;
                NotifySourceOfChanges = false;
                var itemWrapper = CreateItemWrapper(sourceObject);
                Add(itemWrapper);
                NotifySourceOfChanges = before;
            }
            else
                SubscribeToHiddenSourcePropertyChanged(sourceObject);
        }

        int IndexFromSourceIndex(int requestedSourceIndex)
        {
            if (SourceChildren == null)
                throw new MissingMemberException("Cannot get an index of an ItemWrapper in a null SourceChildren");
            var localIndex = -1;
            /*
			int sourceCount = SourceCount ();
			for (int sourceIndex = 0; sourceIndex < sourceCount; sourceIndex++) {
				if (VisiblityTest == null || VisiblityTest(Source[sourceIndex])) {
					localIndex++;
					if (sourceIndex == requestedSourceIndex)
						return localIndex;
				}
			}*/
            var sourceIndex = 0;
            foreach (var sourceItem in SourceChildren)
            {
                if (VisibilityTest == null || VisibilityTest(sourceItem))
                {
                    localIndex++;
                    if (sourceIndex == requestedSourceIndex)
                        return localIndex;
                }
                sourceIndex++;
            }
            return localIndex;
        }

        void InsertSourceObject(int sourceIndex, object sourceObject)
        {
            if (VisibilityTest == null || VisibilityTest(sourceObject))
            {
                var before = NotifySourceOfChanges;
                NotifySourceOfChanges = false;
                var index = IndexFromSourceIndex(sourceIndex);
                var itemWrapper = CreateItemWrapper(sourceObject);
                if (index > -1)
                    Insert(index, itemWrapper);
                else
                    Add(itemWrapper);
                NotifySourceOfChanges = before;
            }
            else
                SubscribeToHiddenSourcePropertyChanged(sourceObject);
        }

        void RemoveItemWithSourceIndex(int sourceIndex)
        {
            var index = IndexFromSourceIndex(sourceIndex);
            if (index > -1)
            {
                var before = NotifySourceOfChanges;
                NotifySourceOfChanges = false;
                RemoveAt(index);
                NotifySourceOfChanges = before;
            }
        }

        void ReplaceItemAtSourceIndex(int sourceIndex, object oldSourceObject, object newSourceObject)
        {
            var index = IndexFromSourceIndex(sourceIndex);
            if (VisibilityTest == null || (VisibilityTest(newSourceObject) && VisibilityTest(oldSourceObject)))
            {
                // replace object
                var before = NotifySourceOfChanges;
                NotifySourceOfChanges = false;
                var itemWrapper = CreateItemWrapper(newSourceObject);
                this[index] = itemWrapper;
                NotifySourceOfChanges = before;
            }
            else if (VisibilityTest?.Invoke(oldSourceObject) ?? default)
            {
                // remove object
                RemoveItemWithSourceIndex(sourceIndex);
                SubscribeToHiddenSourcePropertyChanged(newSourceObject);
            }
            else if (VisibilityTest?.Invoke(newSourceObject) ?? default)
            {
                // insert object
                InsertSourceObject(sourceIndex, newSourceObject);
            }
        }

        // used for subscribing to hidden source objects
        void SubscribeToHiddenSourcePropertyChanged(object source)
        {
            if (source is INotifyPropertyChanged iNotifiableSource)
                iNotifiableSource.PropertyChanged += OnHiddenSourcePropertyChanged;
        }


        void UnsubscribeToHiddenSourcePropertyChanged(object source)
        {
            if (source is INotifyPropertyChanged iNotifiableSource)
                iNotifiableSource.PropertyChanged -= OnHiddenSourcePropertyChanged;
        }


        // used for subscribing to unhidden source objects
        void SubscribeToItemWrapperSourcePropertyChanged(ItemWrapper itemWrapper)
        {
            if (itemWrapper.Source is INotifyPropertyChanged iNotifiableSource)
                iNotifiableSource.PropertyChanged += OnItemWrapperSourcePropertyChanged;
        }


        // used for unsubscribing to unhidden source objects
        void UnsubscribeToItemWrapperSourcePropertyChanged(ItemWrapper itemWrapper)
        {
            if (itemWrapper.Source is INotifyPropertyChanged iNotifiableSource)
                iNotifiableSource.PropertyChanged -= OnItemWrapperSourcePropertyChanged;
        }

        void OnItemWrapperSourcePropertyChanged(object source, PropertyChangedEventArgs e)
        {
            // if the change impacts visibility then remove itemwrapper from groupwrapper
            if (VisibilityTest != null && !VisibilityTest(source))
                RefreshVisibility();
        }

        void OnHiddenSourcePropertyChanged(object source, PropertyChangedEventArgs e)
        {
            if (VisibilityTest != null && VisibilityTest(source))
                RefreshVisibility();
        }

        void RefreshVisibility()
        {
            if (VisibilityTest != null)
            {
                var before = NotifySourceOfChanges;
                NotifySourceOfChanges = false;
                var index = 0;
                var sourceIndex = 0;
                foreach (var sourceItem in SourceChildren)
                {
                    if (VisibilityTest(sourceItem))
                    {
                        if (index >= _itemWrappers.Count || _itemWrappers[index].Source != sourceItem)
                        {
                            UnsubscribeToHiddenSourcePropertyChanged(sourceItem);
                            InsertSourceObject(sourceIndex, sourceItem);
                        }
                        index++;
                    }
                    else
                    {
                        if (index < _itemWrappers.Count && _itemWrappers[index].Source == sourceItem)
                        {
                            RemoveAt(index);
                            SubscribeToHiddenSourcePropertyChanged(sourceItem);
                        }
                    }
                    sourceIndex++;
                }
                if (index != _itemWrappers.Count)
                    throw new InvalidDataContractException("should have iterated through all visible sourceItems and itemWrappers");
                NotifySourceOfChanges = before;
            }
        }

        #endregion


        #region ICollection<T> implementation
        public void Add(ItemWrapper itemWrapper)
        {
            if (itemWrapper == null)
                return;
            if (Contains(itemWrapper))
                throw new NotSupportedException();
            itemWrapper.Index = _itemWrappers.Count;
            CommonAdd(itemWrapper);
            _itemWrappers.Add(itemWrapper);
        }

        bool _clearing;
        public void Clear()
        {
            _clearing = true;
            for (int i = _itemWrappers.Count - 1; i >= 0; i--)
            {
                var subGroup = _itemWrappers[i] as GroupWrapper;
                subGroup?.Clear();
                RemoveAt(i);
            }
            _clearing = false;
        }

        public bool Contains(ItemWrapper itemWrapper)
        {
            //return itemWrapper != null && DeepContains (itemWrapper);
            return itemWrapper != null && _itemWrappers.Contains(itemWrapper);
        }

        public void CopyTo(ItemWrapper[] array, int arrayIndex)
        {
            if (array == null)
                return;
            var offset = 0;
            foreach (var itemWrapper in array)
                Insert(arrayIndex + offset++, itemWrapper);
        }

        public bool Remove(ItemWrapper itemWrapper)
        {
            //var index = itemWrapper.Index;
            CommonRemove(itemWrapper);
            var result = _itemWrappers.Remove(itemWrapper);
            //Reindex(index);
            return result;
        }

        public int Count
        {
            get { return _itemWrappers.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
        #endregion


        #region IEnumerable<T> implementation
        public IEnumerator<ItemWrapper> GetEnumerator()
        {
            return _itemWrappers.GetEnumerator();
        }
        #endregion


        #region IEnumerable implementation
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _itemWrappers.GetEnumerator();
        }
        #endregion


        #region IList implementation
        public int Add(object value)
        {
            Add(value as ItemWrapper);
            return _itemWrappers.Count - 1;
        }

        public bool Contains(object value)
        {
            return Contains(value as ItemWrapper);
        }

        public int IndexOf(object value)
        {
            return IndexOf(value as ItemWrapper);
        }

        public void Insert(int index, object value)
        {
            if (value is ItemWrapper itemWrapper)
                Insert(index, itemWrapper);
            else
                throw new InvalidCastException();
        }

        public void Remove(object value)
        {
            if (value is ItemWrapper itemWrapper)
                Remove(itemWrapper);
        }

        public bool IsFixedSize => false;


        public void CopyTo(Array array, int index)
        {
            if (array is ItemWrapper[] itemWrapperArray)
                CopyTo(itemWrapperArray, index);
        }

        public bool IsSynchronized => false;

        public object SyncRoot => false;

        object IList.this[int index]
        {
            get
            {
                if (index < 0 || index > _itemWrappers.Count)
                    return null;
                return _itemWrappers[index];
            }
            set
            {
                if (value is ItemWrapper itemWrapper && index <= _itemWrappers.Count && index > -1)
                {
                    if (Contains(itemWrapper))
                        throw new NotSupportedException();
                    if (index < _itemWrappers.Count)
                    {
                        _itemWrappers[index].Index = -1;
                        CommonRemove(_itemWrappers[index]);
                    }
                    CommonAdd(itemWrapper);
                    itemWrapper.Index = index;
                    _itemWrappers[index] = itemWrapper;
                    //_itemWrappers[index].Index = index;
                }
                else
                    throw new InvalidCastException();
            }
        }
        #endregion


        #region IndexPath enhancements
        public ItemWrapper ItemWrapperAtIndexPath(Tuple<int, int> indexPath)
        {
            if (indexPath == null)
                return null;
            if (!(this[indexPath.Item1] is GroupWrapper subGroup))
            {
                if (indexPath.Item1 > 0)
                    throw new NotSupportedException("ItemAtIndexPath: indexPath.Item1 > 0 but ItemWrapper at Item1 is not a Group");
                return this[indexPath.Item1];
            }
            return subGroup[indexPath.Item2];
        }
        #endregion


        #region TwoDeepDataSet query

        public DeepDataSet TwoDeepDataSetForOffset(double offset)
        {
            if (offset < 0)
                return null;

            bool groupSelector(double calcOffset, int flatIndex, int i, GroupWrapper groupWrapper) => (offset < calcOffset);
            bool itemSelector(double calcOffset, int flatIndex, int i, ItemWrapper itemWrapper) => (offset < calcOffset);
            bool subItemSelector(double calcOffset, int flatIndex, int i, int j, GroupWrapper groupWrapper, ItemWrapper subItemWrapper) => (offset < calcOffset);

            return TwoDeepDataSet(groupSelector, itemSelector, subItemSelector);
        }

        public DeepDataSet TwoDeepDataSetForIndex(int[] index)
        {
            bool groupSelector(double calcOffset, int flatIndex, int i, GroupWrapper groupWrapper) => (index.Length == 1 && index[0] == i);
            bool itemSelector(double calcOffset, int flatIndex, int i, ItemWrapper itemWrapper) => (index.Length == 1 && index[0] == i);
            bool subItemSelector(double calcOffset, int flatIndex, int i, int j, GroupWrapper groupWrapper, ItemWrapper subItemWrapper)
            {
                return (index.Length >= 2 && index[0] == i && index[1] == j);
            }

            return TwoDeepDataSet(groupSelector, itemSelector, subItemSelector);
        }

        public DeepDataSet TwoDeepDataSetForFlatIndex(int reqFlatIndex)
        {
            bool groupSelector(double calcOffset, int flatIndex, int i, GroupWrapper groupWrapper) => (reqFlatIndex == flatIndex);
            bool itemSelector(double calcOffset, int flatIndex, int i, ItemWrapper itemWrapper) => (reqFlatIndex == flatIndex);
            bool subItemSelector(double calcOffset, int flatIndex, int i, int j, GroupWrapper groupWrapper, ItemWrapper subItemWrapper) => (reqFlatIndex == flatIndex);

            return TwoDeepDataSet(groupSelector, itemSelector, subItemSelector);
        }

        public DeepDataSet TwoDeepDataSet(object item)
        {
            if (item is int fIndex)
                return TwoDeepDataSetForFlatIndex(fIndex);
            if (item is double number)
                return TwoDeepDataSetForOffset(number);
            if (item is int[] index)
                return TwoDeepDataSetForIndex(index);

            bool groupSelector(double calcOffset, int flatIndex, int i, GroupWrapper groupWrapper)
                //=> (groupWrapper == item || groupWrapper.Source == item);
                => (groupWrapper.Equals(item) || groupWrapper.Source.Equals(item));
            bool itemSelector(double calcOffset, int flatIndex, int i, ItemWrapper itemWrapper)
                //=> (itemWrapper == item || itemWrapper.Source == item);
                => (itemWrapper.Equals(item) || itemWrapper.Source.Equals(item));
            bool subItemSelector(double calcOffset, int flatIndex, int i, int j, GroupWrapper groupWrapper, ItemWrapper subItemWrapper)
            {
                //if (subItemWrapper == item || subItemWrapper.Source == item)
                if (subItemWrapper.Equals(item) || subItemWrapper.Source.Equals(item))
                    return true;
                if (subItemWrapper is GroupWrapper subGroupWrapper && subGroupWrapper.DeepContains(item))
                    return true;
                return false;
            }

            return TwoDeepDataSet(groupSelector, itemSelector, subItemSelector);
        }

        public DeepDataSet TwoDeepDataSet(object group, object item)
        {

            bool groupSelector(double calcOffset, int flatIndex, int i, GroupWrapper groupWrapper)
                //=> ((group == groupWrapper || group == groupWrapper.Source) && item == null);
                => ((group.Equals(groupWrapper) || group.Equals(groupWrapper.Source)) && item == null);
            bool itemSelector(double calcOffset, int flatIndex, int i, ItemWrapper itemWrapper)
                //=> (group == null && (item == itemWrapper || item == itemWrapper.Source));
                => (group == null && (item.Equals(itemWrapper) || item.Equals(itemWrapper.Source)));
            bool subItemSelector(double calcOffset, int flatIndex, int i, int j, GroupWrapper groupWrapper, ItemWrapper subItemWrapper)
            {
                //if ((group == groupWrapper || group == groupWrapper.Source) && (item == subItemWrapper || item == subItemWrapper.Source))
                if ((group.Equals(groupWrapper) || group.Equals(groupWrapper.Source)) && (item.Equals(subItemWrapper) || item.Equals(subItemWrapper.Source)))
                    return true;
                if (subItemWrapper is GroupWrapper subGroupWrapper && subGroupWrapper.DeepContains(item))
                {
                    var itemWrapper = DeepItemWrapperForItem(item);
                    return (itemWrapper.Parent is GroupWrapper gr && (gr.Equals(group) || gr.Source.Equals(group)));
                }
                return false;
            }

            return TwoDeepDataSet(groupSelector, itemSelector, subItemSelector);
        }

        DeepDataSet TwoDeepDataSet(Func<double, int, int, GroupWrapper, bool> groupSelector, Func<double, int, int, ItemWrapper, bool> itemSelector, Func<double, int, int, int, GroupWrapper, ItemWrapper, bool> subItemSelector)
        {
            var calcOffset = 0.0;
            var flatIndex = 0;
            for (int i = 0; i < Count; i++)
            {
                var itemWrapper = this[i];
                if (itemWrapper is GroupWrapper groupWrapper)
                {
                    var groupCellHeight = groupWrapper.BestGuessGroupHeaderHeight();
                    if (groupSelector?.Invoke(calcOffset + groupCellHeight, flatIndex, i, groupWrapper) ?? default)
                        return new DeepDataSet(itemWrapper, calcOffset, new int[] { i }, flatIndex, groupCellHeight);
                    calcOffset += groupCellHeight;
                    flatIndex++;
                    for (int j = 0; j < groupWrapper.Count; j++)
                    {
                        var subItemWrapper = groupWrapper[j];
                        var cellHeight = subItemWrapper.BestGuessItemRowHeight();
                        if (subItemSelector?.Invoke(calcOffset + cellHeight, flatIndex, i, j, groupWrapper, subItemWrapper) ?? default)
                            return new DeepDataSet(subItemWrapper, calcOffset, new int[] { i, j }, flatIndex, cellHeight);
                        calcOffset += cellHeight;
                        flatIndex++;
                        if (j < groupWrapper.Count - 1)
                            //calcOffset += subItemWrapper.RenderedSeparatorHeight + SeparatorThicknessError();
                            calcOffset += subItemWrapper.RequestedSeparatorHeight + SeparatorThicknessError();
                    }
                }
                else
                {
                    var cellHeight = itemWrapper.BestGuessItemRowHeight();
                    if (itemSelector?.Invoke(calcOffset + cellHeight, flatIndex, i, itemWrapper) ?? default)
                        return new DeepDataSet(itemWrapper, calcOffset, new int[] { i }, flatIndex, cellHeight);
                    calcOffset += cellHeight;
                    flatIndex++;
                    if (i < Count - 1)
                        //calcOffset += itemWrapper.RenderedSeparatorHeight + SeparatorThicknessError();
                        calcOffset += itemWrapper.RequestedSeparatorHeight + SeparatorThicknessError();
                }
            }
            return null;
        }

        static double SeparatorThicknessError()
        {
            return Device.RuntimePlatform == Device.UWP
                    ? 0 // 0.4 / Display.Scale
                    : Device.RuntimePlatform == Device.Android
                            ? 0.07
                            : 0;
        }
        #endregion


        #region DeepIndex query

        public ItemWrapper ItemWrapperAtDeepIndex(int[] deepIndex)
        {
            if (deepIndex == null || deepIndex.Length == 0)
                throw new NotSupportedException("ItemAtDeepIndex: deepIndex=null || deepIndex.Length=0");
            var itemWrapper = this[deepIndex[0]];
            if (deepIndex.Length == 1)
                return itemWrapper;
            if (!(itemWrapper is GroupWrapper subGroup))
                throw new NotSupportedException("ItemAtDeepIndex: deepIndex.Length > 1 but item at index is not Group");
            var subGroupDeepIndex = new int[deepIndex.Length - 1];
            Array.Copy(deepIndex, 1, subGroupDeepIndex, 0, deepIndex.Length - 1);
            return subGroup.ItemWrapperAtDeepIndex(subGroupDeepIndex);
        }

        /*
        public bool DeepContains(ItemWrapper itemWrapper)
        {
            if (itemWrapper == null)
                return false;
            if (Contains(itemWrapper))
                return true;
            foreach (var member in this)
            {
                if (member is GroupWrapper group && group.DeepContains(itemWrapper))
                    return true;
            }
            return false;
        }
        */

        public bool DeepContains(object item)
        {
            if (item is ItemWrapper itemWrapper)
            {
                if (itemWrapper == null)
                    return false;
                if (Contains(itemWrapper))
                    return true;
                foreach (var member in this)
                {
                    if (member is GroupWrapper groupWrapper && groupWrapper.DeepContains(itemWrapper))
                        return true;
                }
                return false;
            }
            foreach (var member in this)
            {
                if (member.Source == item)
                    return true;
                if (member is GroupWrapper groupWrapper && groupWrapper.DeepContains(item))
                    return true;
            }
            return false;
        }


        #region IndexOf
        public int[] DeepSourceIndexOf(ItemWrapper itemWrapper)
        {
            var pos = SourceIndexOf(itemWrapper);
            if (pos > -1)
            {
                return new[] { pos };
            }
            else
            {
                for (pos = 0; pos < Count; pos++)
                {
                    if (this[pos] is GroupWrapper subGroup)
                    {
                        var subGroupDeepIndex = subGroup.DeepSourceIndexOf(itemWrapper);
                        if (subGroupDeepIndex != null)
                        {
                            var deepIndex = new int[subGroupDeepIndex.Length + 1];
                            deepIndex[0] = pos;
                            subGroupDeepIndex.CopyTo(deepIndex, 1);
                            return deepIndex;
                        }
                    }
                }
            }
            return null;
        }

        public int[] DeepIndexOf(ItemWrapper itemWrapper)
        {
            var pos = IndexOf(itemWrapper);
            if (pos > -1)
            {
                return new[] { pos };
            }
            else
            {
                for (pos = 0; pos < Count; pos++)
                {
                    if (this[pos] is GroupWrapper subGroup)
                    {
                        var subGroupDeepIndex = subGroup.DeepIndexOf(itemWrapper);
                        if (subGroupDeepIndex != null)
                        {
                            var deepIndex = new int[subGroupDeepIndex.Length + 1];
                            deepIndex[0] = pos;
                            subGroupDeepIndex.CopyTo(deepIndex, 1);
                            return deepIndex;
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        public void DeepInsert(int[] deepIndex, ItemWrapper itemWrapper)
        {
            if (DeepContains(itemWrapper))
                throw new NotSupportedException("DeepInsert: Group already contain itemWrapper");
            //return;
            if (deepIndex == null || deepIndex.Length == 0)
                throw new NotSupportedException("DeepInsert: deepIndex=null || deepIndex.Length=0");
            if (deepIndex.Length == 1)
            {
                Insert(deepIndex[0], itemWrapper);
                return;
            }
            if (!(this[deepIndex[0]] is GroupWrapper subGroup))
                throw new NotSupportedException("DeepInsert: deepIndex.Length > 1 but itemWrapper at index is not a GroupWrapper");
            var subGroupDeepIndex = new int[deepIndex.Length - 1];
            Array.Copy(deepIndex, 1, subGroupDeepIndex, 0, deepIndex.Length - 1);
            subGroup.DeepInsert(subGroupDeepIndex, itemWrapper);
        }

        public void DeepRemove(ItemWrapper itemWrapper)
        {
            if (!DeepContains(itemWrapper))
                return;
            var deepIndex = DeepIndexOf(itemWrapper);
            DeepRemoveAt(deepIndex);
        }

        public void DeepRemoveAt(int[] deepIndex)
        {
            if (deepIndex == null || deepIndex.Length == 0)
                throw new NotSupportedException("DeepRemove: deepIndex=null || deepIndex.Length=0");
            if (deepIndex.Length == 1)
            {
                RemoveAt(deepIndex[0]);
                return;
            }
            if (!(this[deepIndex[0]] is GroupWrapper subGroup))
                throw new NotSupportedException("DeepRemove: deepIndex.Length > 1 but itemWrapper at index is not a GroupWrapper");
            var subGroupDeepIndex = new int[deepIndex.Length - 1];
            Array.Copy(deepIndex, 1, subGroupDeepIndex, 0, deepIndex.Length - 1);
            subGroup.DeepRemoveAt(subGroupDeepIndex);
        }

        public void DeepSwap(ItemWrapper itemWrapper1, ItemWrapper itemWrapper2)
        {
            var deepIndex1 = DeepIndexOf(itemWrapper1);
            var deepIndex2 = DeepIndexOf(itemWrapper2);
            if (deepIndex1 == deepIndex2)
                return;
            DeepSet(deepIndex1, itemWrapper2);
            DeepSet(deepIndex2, itemWrapper1);
        }

        public void DeepSet(int[] deepIndex, ItemWrapper itemWrapper)
        {
            if (deepIndex == null || deepIndex.Length == 0)
                return;
            var index = deepIndex[0];
            if (deepIndex.Length == 1)
            {// && deepIndex[0] >=0 && deepIndex[0] <= items.Count) {
                if (index < 0 || index > Count)
                    return;
                //items [index] = item;
                this[index] = itemWrapper;
            }
            else
            {
                if (!(this[index] is GroupWrapper subGroup))
                    throw new NotSupportedException("DeepSet: deepIndex.Length > 1 but itemWrapper at index is not a GroupWrapper");
                // create a new index (subGroupDeepIndex) that contains everything except the [0] index of deepIndex.
                var subGroupDeepIndex = new int[deepIndex.Length - 1];
                Array.Copy(deepIndex, 1, subGroupDeepIndex, 0, deepIndex.Length - 1);
                subGroup.DeepSet(subGroupDeepIndex, itemWrapper);
            }
        }

        public void DeepRemoveItemsWithSource(object source)
        {
            var itemWrappers = new List<ItemWrapper>();
            foreach (var itemWrapper in this)
                if (itemWrapper.Source == source)
                    itemWrappers.Add(itemWrapper);
            if (itemWrappers.Count > 0)
                foreach (var itemWrapper in itemWrappers)
                    Remove(itemWrapper);
            foreach (var itemWrapper in this)
            {
                var groupWrapper = itemWrapper as GroupWrapper;
                groupWrapper?.DeepRemoveItemsWithSource(source);
            }
        }
        #endregion


        #region Flat Position enhancements
        public int FlatPositionOf(object item)
        {
            if (item == null)
                return -1;
            var pos = 0;
            foreach (var itemWrapper in this)
            {
                // count this header/cell
                pos++;
                if (itemWrapper.Source == item || itemWrapper == item)
                    return pos;
                if (itemWrapper is GroupWrapper groupWrapper)
                {
                    foreach (var subItem in groupWrapper)
                    {
                        // count this cell
                        pos++;
                        if (subItem.Source == item || subItem == item)
                            return pos;
                    }
                }
            }
            return -1;
        }

        public int FlatPositionOfItemInGroup(object item, object group)
        {
            if (item == null || group == null)
                return -1;
            var pos = 0;
            foreach (var itemWrapper in this)
            {
                // count this header/cell
                pos++;
                if (itemWrapper.Source == item || itemWrapper == item)
                    return pos - 1;
                if (itemWrapper is GroupWrapper groupWrapper)
                {
                    foreach (var subItemWrapper in groupWrapper)
                    {
                        // count this cell
                        pos++;
                        if ((subItemWrapper.Source == item || subItemWrapper == item) && (groupWrapper.Source == group || groupWrapper == group))
                            return pos - 1;
                    }
                }
            }
            return -1;
        }

        public ItemWrapper DeepItemWrapperForItem(object item)
        {
            if (item is ItemWrapper result)
                return result;
            result = null;
            foreach (var itemWrapper in this)
            {
                if (itemWrapper == item || itemWrapper.Source == item)
                    return itemWrapper;
                if (itemWrapper is GroupWrapper groupWrapper)
                    result = groupWrapper.DeepItemWrapperForItem(item);
                if (result != null)
                    return result;
            }
            return null;
        }
        #endregion


        #region Source INotifyCollectionChanged implementation
        void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IgnoreSourceChanges)
                return;
            var before = NotifySourceOfChanges;
            NotifySourceOfChanges = false;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 0)
                        for (int i = 0; i < e.NewItems.Count; i++)
                            AddSourceObject(e.NewItems[i]);
                    else
                        for (int i = 0; i < e.NewItems.Count; i++)
                            InsertSourceObject(i + e.NewStartingIndex, e.NewItems[i]);
                    break;

                case NotifyCollectionChangedAction.Move:
                    for (int i = e.OldItems.Count - 1; i >= 0; i--)
                        RemoveItemWithSourceIndex(i + e.OldStartingIndex);
                    if (e.NewStartingIndex < 0)
                        for (int i = 0; i < e.NewItems.Count; i++)
                            AddSourceObject(e.NewItems[i]);
                    else
                        for (int i = 0; i < e.NewItems.Count; i++)
                            InsertSourceObject(i + e.NewStartingIndex, e.NewItems[i]);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 0)
                    {
                        // we don't have an index but we can figure it out IF the objects are unique!
                        //throw new NotSupportedException("Cannot remove Source object with unknown index");
                        foreach (var sourceItem in e.OldItems)
                            DeepRemoveItemsWithSource(sourceItem);
                    }
                    else
                    {
                        for (int i = e.OldItems.Count - 1; i >= 0; i--)
                            RemoveItemWithSourceIndex(i + e.OldStartingIndex);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (e.NewStartingIndex < 0 && e.OldStartingIndex < 0)
                        throw new NotSupportedException("Cannot remove Source object with unknown index");
                    var startingIndex = e.NewStartingIndex;
                    if (startingIndex < 0)
                        startingIndex = e.OldStartingIndex;
                    for (int i = 0; i < e.NewItems.Count; i++)
                        ReplaceItemAtSourceIndex(startingIndex + i, e.OldItems[i], e.NewItems[i]);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    Clear();
                    Source = sender;
                    SourceChildren = string.IsNullOrWhiteSpace(childrenPropertyName) ? sender as IEnumerable : sender.GetPropertyValue(childrenPropertyName) as IEnumerable;
                    if (SourceChildren == null || Source == null)
                    {
                        throw new ArgumentException("Group source must be IEnumerable -or- SourcePropertyMap is set to an IEnumerable property of source");
                    }
                    foreach (var obj in SourceChildren)
                        AddSourceObject(obj);
                    break;
            }
            NotifySourceOfChanges = before;
        }
        #endregion


        #region INotifyCollectionChanged implementation
        event NotifyCollectionChangedEventHandler _collectionChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { _collectionChanged += value; }
            remove { _collectionChanged -= value; }
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine (GetType ().Name + ".OnCollectionChanged( " + sender + ", " + e);
            /*
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Reindex(e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Reindex(e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Move:
                    Reindex(Math.Min(e.OldStartingIndex, e.NewStartingIndex));
                    break;
                case NotifyCollectionChangedAction.Replace:
                    Reindex();
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Reindex();
                    break;
            }
            */
            if (!_clearing || Count == 0)
                Reindex();
            _collectionChanged?.Invoke(sender, e);
        }
        #endregion


        #region Source finding
        public ItemWrapper ItemWrapperForSource(object source)
        {
            if (Source == source)
                return this;
            foreach (var item in this)
            {
                //if (item.Source == source)
                if (item.Source.Equals(source))
                    return item;
                var groupWrapper = item as GroupWrapper;
                var subItem = groupWrapper?.ItemWrapperForSource(source);
                if (subItem != null)
                    return subItem;
            }
            return null;
        }

        public GroupWrapper GroupWrapperForItemWrapper(ItemWrapper itemWrapper)
        {
            if (itemWrapper == null)
                return null;
            if (Contains(itemWrapper))
                return this;
            foreach (var member in this)
            {
                var groupWrapper = member as GroupWrapper;
                var result = groupWrapper?.GroupWrapperForItemWrapper(itemWrapper);
                if (result != null)
                    return result;
            }
            return null;
        }

        public Tuple<GroupWrapper, ItemWrapper> GroupAndItemWrappersForSource(object source)
        {
            if (Source == source)
                return new Tuple<GroupWrapper, ItemWrapper>(Parent, this);
            foreach (var itemWrapper in this)
            {
                if (itemWrapper.Source == source)
                    return new Tuple<GroupWrapper, ItemWrapper>(this, itemWrapper);
                var groupWrapper = itemWrapper as GroupWrapper;
                var subItem = groupWrapper?.GroupAndItemWrappersForSource(source);
                if (subItem != null)
                    return subItem;
            }
            return null;
        }
        #endregion
    }
}

