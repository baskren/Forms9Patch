using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.ObjectModel;
using PCL.Utils;
using Xamarin.Forms;
using System.Linq;
using System.Runtime.Serialization;

namespace Forms9Patch
{
	class GroupWrapper : ItemWrapper<object>, IList<ItemWrapper>, ICollection<ItemWrapper>, IEnumerable<ItemWrapper>, IEnumerable, IList, ICollection, IReadOnlyList<ItemWrapper>, IReadOnlyCollection<ItemWrapper>, INotifyCollectionChanged  {


		#region Source / Target (Visibles) coupling
		bool _notifySourceOfChanges;
		public bool NotifySourceOfChanges {
			get {
				return _notifySourceOfChanges;
			}
			set {
				foreach (var item in this) {
					var group = item as GroupWrapper;
					if (group != null)
						group.NotifySourceOfChanges = value;
				}
				_notifySourceOfChanges = value;
			}
		}

		public bool IgnoreSourceChanges {
			get;
			set;
		}

		/// <summary>
		/// The visibility test property backing store.
		/// </summary>
		public static readonly BindableProperty VisibilityTestProperty = BindableProperty.Create("VisibilityTest", typeof(Func<object,bool>), typeof(GroupWrapper), default(Func<object,bool>));
		/// <summary>
		/// Gets or sets the test used to determine if an item or group will be visible.
		/// </summary>
		/// <value>The visibility test.</value>
		public Func<object,bool> VisibilityTest
		{
			get { return (Func<object,bool>)GetValue(VisibilityTestProperty); }
			set { SetValue(VisibilityTestProperty, value); }
		}


		/*
		// note: ValidMateTriggerProperty is an optimization!  If it is null, ValidMateTest will still be used to decide validity, just more often.
		string validMateTriggerProperty;
		public string ValidMateTriggerProperty {
			get { return validMateTriggerProperty; }
			set {
				validMateTriggerProperty = value;
				foreach (var item in items) {
					var group = item as Group;
					if (group!=null) 
						group.ValidMateTriggerProperty = group.ValidMateTriggerProperty ?? ValidMateTriggerProperty;
				}
			}
		}
		*/

		// note: Even though Source's items are not unique (ex: Source= { a, a, "pizza", null, null }) ItemWrappers are because they are each created from a new instance of type Item.

		int SourceCount() {
			int sourceCount = -1;
			var iCollectionIenumerble = SourceChildren as ICollection<IEnumerable>;
			if (iCollectionIenumerble!=null)
				sourceCount = iCollectionIenumerble.Count;
			else {
				var iCollection = SourceChildren as ICollection;
				if (iCollection != null)
					sourceCount = iCollection.Count;
				else {
					sourceCount = 0;
					foreach (var obj in SourceChildren)
						sourceCount++;
				}
			}
			return sourceCount;
		}

		int SourceIndexOf(ItemWrapper item) {
			if (SourceChildren == null)
				throw new MissingMemberException ("Cannot get an index of an item in a null Source");
			if (!_items.Contains (item))
				return -1;

			var iListiEnumerblle = SourceChildren as IList<IEnumerable>;
			if (iListiEnumerblle != null)
				return iListiEnumerblle.IndexOf ((IEnumerable)item.Source);
			var iList = SourceChildren as IList;
			if (iList != null)
				return iList.IndexOf (item.Source);

			int index = 0;
			foreach (var obj in SourceChildren) {
				if (obj == item.Source)
					return index;
				index++;
			}
			return -1;
			/* THIS MAY BE NEEDED ... CAN'T REMEMBER THE ROLE OF VIABILITY HANDLING
			int candidateSourceIndex=0;
			int localIndex=0;
			int itemIndex = _items.IndexOf (item);
			for (int sourceIndex = 0; sourceIndex < SourceCount(); sourceIndex++) {
				if (VisiblityTest==null || VisiblityTest(Source[sourceIndex])) {
					if (localIndex == itemIndex)
						return candidateSourceIndex;
					candidateSourceIndex = sourceIndex+1;
					localIndex++;
				}
			}
			return candidateSourceIndex;
			*/
		}

		void SourceInsert(ItemWrapper item) {
			if (SourceChildren == null)
				throw new MissingMemberException ("Cannot insert an item into a null Source");
			var iListIEnumerable = SourceChildren as IList<IEnumerable>;
			if (iListIEnumerable != null) {
				var sourceIndex = iListIEnumerable.IndexOf ((IEnumerable)item.Source);
				if (sourceIndex == -1)
					throw new MissingMemberException ("Cannot SourceInsert object that is not a member of Group");
				IgnoreSourceChanges = true;
				iListIEnumerable.Insert (sourceIndex, (IEnumerable)item.Source);
				IgnoreSourceChanges = false;
				return;
			} 
			var iList = SourceChildren as IList;
			if (iList != null) {
				var sourceIndex = iList.IndexOf (item.Source);
				if (sourceIndex == -1)
					throw new MissingMemberException ("Cannot SourceInsert object that is not a member of Group");
				IgnoreSourceChanges = true;
				iList.Insert (sourceIndex, item.Source);
				IgnoreSourceChanges = false;
			}
		}

		void SourceAdd(ItemWrapper item) {
			//System.Diagnostics.Debug.WriteLine ("[" + this + "].MateAdd(" + item + ")");
			if (SourceChildren == null)
				throw new MissingMemberException ("Cannot insert an item into a null Source");
			var iCollectionIEnumerable = SourceChildren as ICollection<IEnumerable>;
			if (iCollectionIEnumerable != null) {
				IgnoreSourceChanges = true;
				iCollectionIEnumerable.Add ((IEnumerable)item.Source);
				IgnoreSourceChanges = false;
				return;
			}
			var iList = SourceChildren as IList;
			if (iList != null) {
				IgnoreSourceChanges = true;
				iList.Add (item.Source);
				IgnoreSourceChanges = false;
			}
		}

		void SourceRemove(ItemWrapper item) {
			//System.Diagnostics.Debug.WriteLine ("[" + this + "].MateRemove(" + item + ")");
			if (SourceChildren == null)
				throw new MissingMemberException ("Cannot insert an item into a null Source");
			var iCollectionIEnumerable = SourceChildren as ICollection<IEnumerable>;
			if (iCollectionIEnumerable != null) {
				IgnoreSourceChanges = true;
				iCollectionIEnumerable.Remove ((IEnumerable)item.Source);
				IgnoreSourceChanges = false;
			}
			var iList = SourceChildren as IList;
			if (iList != null) {
				IgnoreSourceChanges = true;
				iList.Remove (item.Source);
				IgnoreSourceChanges = false;
			}
		}
		#endregion


		#region add/remove common actions
		void CommonNewItem(ItemWrapper item)
		{
			if (item == null)
				return;
			SubscribeToItemSourcePropertyChanged(item);
			item.PropertyChanged += OnItemWrapperPropertyChanged;
			item.PropertyChanging += OnItemWrapperPropertyChanging;
			item.Tapped += OnTapped;
			item.LongPressed += OnLongPressed;
			item.LongPressing += OnLongPressing;
			item.SwipeMenuItemTapped += OnSwipeMenuItemTapped;
			item.BindingContext = this;
			item.Parent = this;
			//var group = item as GroupWrapper;
			//if (group != null)
			if (item.GetType()==typeof(GroupWrapper))
			{
				var group = item as GroupWrapper;
				group.VisibilityTest = group.VisibilityTest ?? VisibilityTest;
				group.SubGroupType = group.SubGroupType ?? SubGroupType;
			}
		}

		void CommonAdd(ItemWrapper item) {
			CommonNewItem(item);
			if (NotifySourceOfChanges) 
				SourceAdd (item);
		}

		void CommonInsert(ItemWrapper item) {
			CommonNewItem(item);
			if (NotifySourceOfChanges) 
				SourceInsert (item);
		}

		void CommonRemove(ItemWrapper item) {
			if (item == null)
				return;
			UnsubscribeToItemSourcePropertyChanged(item);
			if (NotifySourceOfChanges) 
				SourceRemove (item);
			item.PropertyChanged -= OnItemWrapperPropertyChanged;
			item.PropertyChanging -= OnItemWrapperPropertyChanging;
			item.Tapped -= OnTapped;
			item.LongPressed -= OnLongPressed;
			item.LongPressing -= OnLongPressing;
			item.SwipeMenuItemTapped -= OnSwipeMenuItemTapped;
			item.BindingContext = null;
			item.Parent = null;
		}


		#endregion


		void Reindex(int index=0)
		{
			for (int i = index; i < _items.Count; i++)
				_items[i].Index = i;
		}




		#region IList<T> implementation
		public int IndexOf (ItemWrapper item) {
			// need to return -1 if the item is not in this group so DeepIndex can be found by searching subgroups
			return _items.IndexOf(item);
		}

		public void Insert (int index, ItemWrapper item) { 
			if (item == null)
				return;
			if (Contains (item))
				throw new NotSupportedException ();
			_items.Insert (index, item);
			Reindex(index);
			CommonInsert (item);
		}

		public void RemoveAt (int index) {
			var item = _items[index];
			CommonRemove (_items[index]);
			item.Index = -1;
			_items.RemoveAt (index);
			Reindex(index);
		}

		public ItemWrapper this [int index] {
			get {
				if (index < 0 || index > _items.Count-1)
					return null;
				return _items[index]; 
			}
			set { 
				if (_items.Contains (value) || index < 0 || index > _items.Count)
					// will this create problems for move/swap operations?  I think so!  
					throw new NotSupportedException ();
				if (index < _items.Count)
				{
					_items[index].Index = -1;
					CommonRemove(_items[index]);
				}
				//System.Diagnostics.Debug.WriteLine ("\t\tsetting _item start");
				_items[index] = value;
				_items[index].Index = index;
				//System.Diagnostics.Debug.WriteLine ("\t\tsetting _item finish");
				CommonAdd (value);
			}
		}
		#endregion


		#region Source object coupling
		public GroupContentType ContentType { get; set; }

		internal enum GroupContentType {
			Unknown,
			Lists,
			Objects
		}

		void VerifyContentType(GroupContentType contentType) {
			if (ContentType == GroupContentType.Unknown)
				ContentType = contentType;
			if (ContentType != contentType)
				throw new InvalidCastException ("Cannot group IEnumerable with non-IEnumerable objects");
		}

		ItemWrapper CreateItem(object sourceObject) {

			string propertyName=null;
			List<string> subgroupPropertyMap;
			if (_subPropertyMap != null && _subPropertyMap.Count > 0) {
				subgroupPropertyMap = new List<string> (_subPropertyMap);
				subgroupPropertyMap [0] = null;
				propertyName = _subPropertyMap [0];
			}

			var children = string.IsNullOrWhiteSpace (propertyName) ? sourceObject : sourceObject.GetPropertyValue (propertyName);
			if (!(children is string))
			{
				var iEnumerable = children as IEnumerable;
				if (iEnumerable != null)
				{
					if (SubGroupType == null || children.GetType() == SubGroupType)
					{
						// groups
						VerifyContentType(GroupContentType.Lists);
						var group = new GroupWrapper(sourceObject, _subPropertyMap, VisibilityTest, SubGroupType);
						return group;
					}
				}
			}
			// items
			VerifyContentType(GroupContentType.Objects);
			ItemWrapper item;
			if (sourceObject == null)
				item = new ItemWrapper<object>();
			else
			{
				var objType = sourceObject.GetType();
				var itemType = typeof(ItemWrapper<>).MakeGenericType(new[] { objType });

				// Approach 1
				item = (ItemWrapper)Activator.CreateInstance(itemType);

				// Approach 2
				//item = (ItemWrapper)BaitAndSwitch.ObjectFactory.Constructor(itemType, new object[] { });

				// Approach 3

				item.Source = sourceObject;
			}
			return item;
		}

		void AddSourceObject(object sourceObject) {
			if (VisibilityTest == null || VisibilityTest(sourceObject))
			{
				NotifySourceOfChanges = false;
				var item = CreateItem(sourceObject);
				Add(item);
				NotifySourceOfChanges = true;
			}
			else
				SubscribeToHiddenSourcePropertyChanged(sourceObject);
		}

		int LocalIndexFromSourceIndex(int requestedSourceIndex) {
			if (SourceChildren==null)
				throw new MissingMemberException ("Cannot get an index of an item in a null Source");
			int localIndex = -1;
			/*
			int sourceCount = SourceCount ();
			for (int sourceIndex = 0; sourceIndex < sourceCount; sourceIndex++) {
				if (VisiblityTest == null || VisiblityTest(Source[sourceIndex])) {
					localIndex++;
					if (sourceIndex == requestedSourceIndex)
						return localIndex;
				}
			}*/
			int sourceIndex = 0;
			foreach (var sourceItem in SourceChildren) {
				if (VisibilityTest == null || VisibilityTest (sourceItem)) {
					localIndex++;
					if (sourceIndex == requestedSourceIndex)
						return localIndex;
				}
				sourceIndex++;
			}
			return localIndex;
		}

		void InsertSourceObject(int sourceIndex, object sourceObject) {
			if (VisibilityTest == null || VisibilityTest (sourceObject)) {
				NotifySourceOfChanges = false;
				var index = LocalIndexFromSourceIndex (sourceIndex);
				var item = CreateItem (sourceObject);
				if (index > -1)
					Insert (index, item);
				else
					Add (item);
				NotifySourceOfChanges = true;
			}
			else
				SubscribeToHiddenSourcePropertyChanged(sourceObject);
		}

		void RemoveItemWithSourceIndex(int sourceIndex) {
			var index = LocalIndexFromSourceIndex (sourceIndex);
			if (index > -1) {
				NotifySourceOfChanges = false;
				RemoveAt (index);
				NotifySourceOfChanges = true;
			}
		}

		void ReplaceItemAtSourceIndex(int sourceIndex, object oldSourceObject, object newSourceObject) {
			var index = LocalIndexFromSourceIndex (sourceIndex);
			if (VisibilityTest == null || VisibilityTest (newSourceObject) && VisibilityTest (oldSourceObject)) {
				// replace object
				NotifySourceOfChanges = false;
				var item = CreateItem (newSourceObject);
				this [index] = item;
				NotifySourceOfChanges = true;
			} else if (VisibilityTest (oldSourceObject)) {
				// remove object
				RemoveItemWithSourceIndex (sourceIndex);
				SubscribeToHiddenSourcePropertyChanged(newSourceObject);
			} else if (VisibilityTest (newSourceObject)) {
				// insert object
				InsertSourceObject(sourceIndex, newSourceObject);
			}
		}

		// used for subscrbing to hidden source objects
		void SubscribeToHiddenSourcePropertyChanged(object source)
		{
			var iNotifiableSource = source as INotifyPropertyChanged;
			if (iNotifiableSource != null)
				iNotifiableSource.PropertyChanged += OnHiddenSourcePropertyChanged;
		}


		void UnsubscribeToHiddenSourcePropertyChanged(object source)
		{
			var iNotifiableSource = source as INotifyPropertyChanged;
			if (iNotifiableSource != null)
				iNotifiableSource.PropertyChanged -= OnHiddenSourcePropertyChanged;
		}


		// used for subscribing to unhidden source objects
		void SubscribeToItemSourcePropertyChanged(ItemWrapper item)
		{
			var iNotifiableSource = item.Source as INotifyPropertyChanged;
			if (iNotifiableSource != null)
				iNotifiableSource.PropertyChanged += OnItemSourcePropertyChanged;
		}


		// used for unsubscribing to unhidden source objects
		void UnsubscribeToItemSourcePropertyChanged(ItemWrapper item)
		{
			var iNotifiableSource = item.Source as INotifyPropertyChanged;
			if (iNotifiableSource != null)
				iNotifiableSource.PropertyChanged -= OnItemSourcePropertyChanged;
		}

		void OnItemSourcePropertyChanged(object source, PropertyChangedEventArgs e)
		{
			// if the change impacts visibiltiy then remove itemwrapper from groupwrapper
			if (!VisibilityTest(source))
				RefreshVisibility();
		}

		void OnHiddenSourcePropertyChanged(object source, PropertyChangedEventArgs e)
		{
			if (VisibilityTest(source))
				RefreshVisibility();
		}

		void RefreshVisibility()
		{
			NotifySourceOfChanges = false;
			int index = 0;
			int sourceIndex = 0;
			foreach (var sourceItem in SourceChildren)
			{
				if (VisibilityTest(sourceItem))
				{
					if (index >= _items.Count || _items[index].Source != sourceItem)
					{
						UnsubscribeToHiddenSourcePropertyChanged(sourceItem);
						InsertSourceObject(sourceIndex, sourceItem);
					}
					index++;
				}
				else
				{
					if (index < _items.Count && _items[index].Source == sourceItem)
					{
						RemoveAt(index);
						SubscribeToHiddenSourcePropertyChanged(sourceItem);
					}
				}
				sourceIndex++;
			}
			NotifySourceOfChanges = true;
			if (index != _items.Count)
				throw new InvalidDataContractException("should have iterated through all visible sourceItems and itemWrappers");
		}

		#endregion


		#region ICollection<T> implementation
		public void Add (ItemWrapper item) { 
			if (item == null)
				return;
			if (Contains (item))
				throw new NotSupportedException ();
			item.Index = _items.Count;
			_items.Add (item); 
			CommonAdd (item);
		}

		public void Clear () { 
			for (int i = _items.Count - 1; i >= 0; i--) {
				var subGroup = _items[i] as GroupWrapper;
				subGroup?.Clear();
				RemoveAt(i);
			}
		}

		public bool Contains (ItemWrapper item) { 
			return item != null && DeepContains (item);
		}

		public void CopyTo (ItemWrapper[] array, int arrayIndex) { 
			if (array == null)
				return;
			int offset = 0;
			foreach (var item in array)
				Insert (arrayIndex + offset++, item);
		}

		public bool Remove (ItemWrapper item) {
			// if item is a group, will garbage collection clean up its items? - It better not because we may want to use it elsewhere
			var index = item.Index;
			CommonRemove(item);
			var result = _items.Remove (item); 
			Reindex(index);
			return result;
		}

		public int Count { 
			get { return _items.Count; }
		}

		public bool IsReadOnly {
			get { return false; }
		}
		#endregion


		#region IEnumerable<T> implementation
		public IEnumerator<ItemWrapper> GetEnumerator () {
			return _items.GetEnumerator ();
		}
		#endregion


		#region IEnumerable implementation
		IEnumerator IEnumerable.GetEnumerator () {
			return _items.GetEnumerator ();
		}
		#endregion


		#region IList implementation
		public int Add (object value)
		{
			Add (value as ItemWrapper);
			return _items.Count - 1;
		}

		public bool Contains (object value)
		{
			return Contains (value as ItemWrapper);
		}

		public int IndexOf (object value)
		{
			return IndexOf (value as ItemWrapper);
		}

		public void Insert (int index, object value)
		{
			var item = value as ItemWrapper;
			if (item != null) {
				Insert (index, item);
			} else
				throw new InvalidCastException ();
		}

		public void Remove (object value)
		{
			var item = value as ItemWrapper;
			if (item != null) 
				Remove (item);
		}

		public bool IsFixedSize {
			get { return false; }
		}

		public void CopyTo (Array array, int index)
		{
			CopyTo ( array as ItemWrapper[], index);
		}

		public bool IsSynchronized {
			get { throw new NotImplementedException (); }
		}

		public object SyncRoot {
			get { throw new NotImplementedException (); }
		}

		object IList.this [int index] {
			get {
				return _items[index];
			}
			set {
				var item = value as ItemWrapper;
				if (item != null && index <= _items.Count && index > -1) {
					if (Contains (item))
						throw new NotSupportedException ();
					if (index < _items.Count)
					{
						_items[index].Index = -1;
						CommonRemove(_items[index]);
					}
					_items[index] = item;
					_items[index].Index = index;
					CommonAdd (item);
				}
				else
					throw new InvalidCastException();
			}
		}
		#endregion


		#region Deep List enhancements

		public bool DeepContains(ItemWrapper item) {
			if (_items.Contains (item))
				return true;
			foreach (var member in _items) {
				var group = member as GroupWrapper;
				if (group != null && group.DeepContains (item))
					return true;
			}
			return false;
		}

		public int[] DeepSourceIndexOf(ItemWrapper item) {
			int pos = SourceIndexOf (item);
			if (pos > -1) {
				return new [] { pos };
			} else {
				for (pos = 0; pos < _items.Count; pos++) {
					var subGroup = _items[pos] as GroupWrapper;
					if (subGroup != null) {
						int[] subGroupDeepIndex = subGroup.DeepSourceIndexOf (item);
						if (subGroupDeepIndex != null) {
							var deepIndex = new int[subGroupDeepIndex.Length+1];
							deepIndex [0] = pos;
							subGroupDeepIndex.CopyTo (deepIndex, 1);
							return deepIndex;
						}
					}
				}
			}
			return null;
		}

		public int[] DeepIndexOf(ItemWrapper item) {
			int pos = IndexOf (item);
			if (pos > -1) {
				return new [] { pos };
			} else {
				for (pos = 0; pos < _items.Count; pos++) {
					var subGroup = _items[pos] as GroupWrapper;
					if (subGroup != null) {
						int[] subGroupDeepIndex = subGroup.DeepIndexOf (item);
						if (subGroupDeepIndex != null) {
							var deepIndex = new int[subGroupDeepIndex.Length+1];
							deepIndex [0] = pos;
							subGroupDeepIndex.CopyTo (deepIndex, 1);
							return deepIndex;
						}
					}
				}
			}
			return null;
		}

		public void DeepInsert (int[] deepIndex, ItemWrapper item) { 
			if (DeepContains (item))
				throw new NotSupportedException ("DeepInsert: Group already contain item");
				//return;
			if (deepIndex==null || deepIndex.Length == 0)
				throw new NotSupportedException ("DeepInsert: deepIndex=null || deepIndex.Length=0");
			if (deepIndex.Length == 1) {
				Insert (deepIndex [0], item);
				return;
			}
			var subGroup = _items[deepIndex [0]] as GroupWrapper;
			if (subGroup == null)
				throw new NotSupportedException ("DeepInsert: deepIndex.Length > 1 but item at index is not Group");
			var subGroupDeepIndex = new int[deepIndex.Length - 1];
			Array.Copy (deepIndex, 1, subGroupDeepIndex, 0, deepIndex.Length - 1);
			subGroup.DeepInsert (subGroupDeepIndex, item);
		}

		public void DeepRemove(ItemWrapper item) {
			if (!DeepContains (item))
				return;
			var deepIndex = DeepIndexOf (item);
			DeepRemoveAt (deepIndex);
		}

		public void DeepRemoveAt (int[] deepIndex) { 
			if (deepIndex==null || deepIndex.Length == 0)
				throw new NotSupportedException ("DeepRemove: deepIndex=null || deepIndex.Length=0");
			if (deepIndex.Length == 1) {
				RemoveAt (deepIndex [0]);
				return;
			}
			var subGroup = _items[deepIndex [0]] as GroupWrapper;
			if (subGroup == null)
				throw new NotSupportedException ("DeepRemove: deepIndex.Length > 1 but item at index is not Group");
			var subGroupDeepIndex = new int[deepIndex.Length - 1];
			Array.Copy (deepIndex, 1, subGroupDeepIndex, 0, deepIndex.Length - 1);
			subGroup.DeepRemoveAt (subGroupDeepIndex);
		}

		public ItemWrapper ItemAtDeepIndex(int[] deepIndex) {
			if (deepIndex==null || deepIndex.Length == 0)
				throw new NotSupportedException ("ItemAtDeepIndex: deepIndex=null || deepIndex.Length=0");
			if (deepIndex.Length == 1) {
				return _items[deepIndex [0]];
			}
			var subGroup = _items[deepIndex [0]] as GroupWrapper;
			if (subGroup == null)
				throw new NotSupportedException ("ItemAtDeepIndex: deepIndex.Length > 1 but item at index is not Group");
			var subGroupDeepIndex = new int[deepIndex.Length - 1];
			Array.Copy (deepIndex, 1, subGroupDeepIndex, 0, deepIndex.Length - 1);
			return subGroup.ItemAtDeepIndex (subGroupDeepIndex);
		}

		public ItemWrapper ItemAtIndexPath(Tuple<int, int> indexPath)
		{
			if (indexPath == null)
				return null;
			var subGroup = _items[indexPath.Item1] as GroupWrapper;
			if (subGroup == null)
			{
				if (indexPath.Item1 > 0)
					throw new NotSupportedException("ItemAtIndexPath: indexPath.Item1 > 0 but item at Item1 is not a Group");
				return _items[indexPath.Item1];
			}
			return subGroup[indexPath.Item2];
		}


		public void DeepSwapItems(ItemWrapper item1, ItemWrapper item2) {
			var deepIndex1 = DeepIndexOf (item1);
			var deepIndex2 = DeepIndexOf (item2);
			if (deepIndex1 == deepIndex2)
				return;
			DeepSet (deepIndex1, item2);
			DeepSet (deepIndex2, item1);
		}

		public void DeepSet(int[] deepIndex, ItemWrapper item) {
			if (deepIndex == null || deepIndex.Length == 0)
				return;
			int index = deepIndex [0];
			if (deepIndex.Length == 1) {// && deepIndex[0] >=0 && deepIndex[0] <= items.Count) {
				if (index < 0 || index > _items.Count)
					return;
				//items [index] = item;
				this[index] = item;
			} else {
				var subGroup = _items[index] as GroupWrapper;
				if (subGroup == null)
					throw new NotSupportedException ("DeepSet: deepIndex.Length > 1 but item at index is not Group");
				// create a new index (subGroupDeepIndex) that contains everything except the [0] index of deepIndex.
				var subGroupDeepIndex = new int[deepIndex.Length - 1];
				Array.Copy (deepIndex, 1, subGroupDeepIndex, 0, deepIndex.Length - 1);
				subGroup.DeepSet (subGroupDeepIndex, item);
			}
		}

		public void DeepRemoveItemsWithSource(object source)
		{
			System.Diagnostics.Debug.WriteLine("");
			var items = this.Where(x => x.Source == source).ToList();
			if (items.Count > 0)
			{
				foreach (var item in items)
					Remove(item);
			}
			foreach (var item in this)
			{
				var group = item as GroupWrapper;
				group?.DeepRemoveItemsWithSource(source);
			}
		}
		#endregion


		#region Flat Position enhancements
		public int FlatPositionOfItem(object item)
		{
			if (item == null)
				return -1;
			int pos = 0;
			foreach (var topItem in _items)
			{
				// count this header/cell
				pos++;
				if (topItem.Source == item)
					return pos;
				var gr = topItem as GroupWrapper;
				if (gr != null)
				{
					foreach (var subItem in gr)
					{
						// count this cell
						pos++;
						if (subItem.Source == item)
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
			int pos = 0;
			foreach (var topItem in _items)
			{
				// count this header/cell
				pos++;
				if (topItem.Source == item)
					return pos - 1;
				var gr = topItem as GroupWrapper;
				if (gr != null)
				{
					foreach (var subItem in gr)
					{
						// count this cell
						pos++;
						if (subItem.Source == item && topItem.Source == group)
							return pos - 1;
					}
				}
			}
			return -1;
		}
		#endregion


		#region Properties
		public static readonly BindableProperty SourceSubPropertyMapProperty = BindableProperty.Create("SourceSubPropertyMap", typeof(List<string>), typeof(GroupWrapper), null);
		public List<string> SourceSubPropertyMap
		{
			get { return (List<string>)GetValue(SourceSubPropertyMapProperty); }
			set { SetValue(SourceSubPropertyMapProperty, value); }
		}

		public static readonly BindableProperty SubGroupTypeProperty = BindableProperty.Create("SubGroupType", typeof(Type), typeof(GroupWrapper), null);
		public Type SubGroupType
		{
			get { return (Type)GetValue(SubGroupTypeProperty); }
			set { SetValue(SubGroupTypeProperty, value); }
		}

		#endregion


		#region Fields
		readonly ObservableCollection<ItemWrapper> _items = new ObservableCollection<ItemWrapper> ();

		List<string> _subPropertyMap;

		IEnumerable SourceChildren;

		string childrenPropertyName;
		#endregion


		#region Constructors
		public GroupWrapper(object source, List<string> sourcePropertyMap, Func<object,bool>visibleItemTest=null, Type subgroupType=null) : this() {
			VisibilityTest = visibleItemTest;
			ContentType = GroupContentType.Unknown;
			SourceSubPropertyMap = sourcePropertyMap;
			SubGroupType = subgroupType;
			Source = source;
		}

		public GroupWrapper()  {
			ContentType = GroupContentType.Unknown;
			NotifySourceOfChanges = true;
			_items.CollectionChanged += OnCollectionChanged; 
		}

		#endregion


		#region Source INotifyCollectionChanged implementation
		void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
			if (IgnoreSourceChanges)
				return;
			NotifySourceOfChanges = false;
			switch (e.Action) {
			case NotifyCollectionChangedAction.Add:
				if (e.NewStartingIndex < 0)
					for (int i = 0; i < e.NewItems.Count; i++) 
						AddSourceObject (e.NewItems [i]);
				else
					for (int i = 0; i < e.NewItems.Count; i++) 
						InsertSourceObject (i + e.NewStartingIndex, e.NewItems [i]);
				break;

			case NotifyCollectionChangedAction.Move:
				for (int i = e.OldItems.Count - 1; i >= 0; i--)
					RemoveItemWithSourceIndex (i + e.OldStartingIndex);
				if (e.NewStartingIndex < 0)
					for (int i = 0; i < e.NewItems.Count; i++) 
						AddSourceObject (e.NewItems [i]);
				else
					for (int i = 0; i < e.NewItems.Count; i++) 
						InsertSourceObject (i + e.NewStartingIndex, e.NewItems [i]);
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
					throw new NotSupportedException ("Cannot remove Source object with unknown index");
				var startingIndex = e.NewStartingIndex;
				if (startingIndex < 0)
					startingIndex = e.OldStartingIndex;
				for (int i = 0; i < e.NewItems.Count; i++)
					ReplaceItemAtSourceIndex (startingIndex + i, e.OldItems [i], e.NewItems [i]);
				break;

			case NotifyCollectionChangedAction.Reset:
				NotifySourceOfChanges = false;
				Clear ();
				Source = sender;
				SourceChildren = string.IsNullOrWhiteSpace (childrenPropertyName) ? sender as IEnumerable : sender.GetPropertyValue (childrenPropertyName) as IEnumerable;
				if (SourceChildren == null || Source==null) {
					throw new ArgumentException ("Group source must be IEnumerable -or- SourcePropertyMap is set to an IEnumerable property of source");
				}
				foreach (var obj in SourceChildren)
					AddSourceObject (obj);
				NotifySourceOfChanges = true;
				break;
			}
			NotifySourceOfChanges = true;
		}
		#endregion


		#region INotifyCollectionChanged implementation
		event NotifyCollectionChangedEventHandler _collectionChanged;
		public event NotifyCollectionChangedEventHandler CollectionChanged {
			add { _collectionChanged += value; }
			remove { _collectionChanged -= value; }
		}

		void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
			//System.Diagnostics.Debug.WriteLine (GetType ().Name + ".OnCollectionChanged( " + sender + ", " + e);
			_collectionChanged?.Invoke(sender,e);
		}
		#endregion


		#region Property Change Management

		protected override void OnPropertyChanging(string propertyName = null)
		{
			base.OnPropertyChanging(propertyName);
			if (propertyName == SourceProperty.PropertyName && Source != null)
			{
				SourceChildren = string.IsNullOrWhiteSpace(childrenPropertyName) ? Source as IEnumerable : Source.GetPropertyValue(childrenPropertyName) as IEnumerable;
				var observableCollection = SourceChildren as INotifyCollectionChanged;
				if (observableCollection != null)
					observableCollection.CollectionChanged -= OnSourceCollectionChanged;
			}
		}


		protected override void OnPropertyChanged (string propertyName = null)
		{
			base.OnPropertyChanged (propertyName);

			if (propertyName == SourceProperty.PropertyName)
			{
				_items.Clear();
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
				var observableCollection = SourceChildren as INotifyCollectionChanged;
				if (observableCollection != null)
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
			else if (propertyName == RowHeightProperty.PropertyName)
				foreach (var child in this)
					child.RowHeight = RowHeight;
			else if (propertyName == SeparatorColorProperty.PropertyName)
				foreach (var child in this)
					child.SeparatorColor = SeparatorColor;
			else if (propertyName == SeparatorHeightProperty.PropertyName)
				foreach (var child in this)
					child.SeparatorHeight = SeparatorHeight;
			else if (propertyName == SeparatorIsVisibleProperty.PropertyName)
				foreach (var child in this)
					child.SeparatorIsVisible = SeparatorIsVisible;
			else if (propertyName == SeparatorLeftIndentProperty.PropertyName)
				foreach (var child in this)
					child.SeparatorLeftIndent = SeparatorLeftIndent;
			else if (propertyName == SeparatorRightIndentProperty.PropertyName)
				foreach (var child in this)
					child.SeparatorRightIndent = SeparatorRightIndent;
				
		}
		#endregion


		#region Member Item Property Change Notificaiton
		public event PropertyChangingEventHandler ItemWrapperPropertyChanging;
		void OnItemWrapperPropertyChanging (object sender, PropertyChangingEventArgs e)
		{
			ItemWrapperPropertyChanging?.Invoke(sender, e);
		}

		public event PropertyChangedEventHandler ItemWrapperPropertyChanged;
		void OnItemWrapperPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			//if (BcGlobal.timerTrippedCount>1)
			//	System.Diagnostics.Debug.WriteLine (GetType ().Name + ".OnItemPropertyChanged( " + sender + ", " + e);
			/* not sure why I wanted to do this.  If the item has been removed from ListItems Group, then this would add it back!
			var item = sender as Item;
			var mateGroup = Source;
			if (Coupled && item != null && mateGroup != null && validMateTest!=null) {
				// TODO: determine if getting ride of ValidMateTriggerProperty has no percievable impact upon performance
				if (ValidMateTriggerProperty == null || e.PropertyName == ValidMateTriggerProperty) {
					if (validMateTest (item))
						MateInsert (item);
					else
						MateRemove (item);
				}
			}
			*/
			ItemWrapperPropertyChanged?.Invoke(sender, e);
		}

		#endregion


		#region Source finding
		public ItemWrapper WrapperForSource(object source)
		{
			if (Source == source)
				return this;
			foreach (var item in _items)
			{
				if (item.Source == source)
					return item;
				var group = item as GroupWrapper;
				var subItem = group?.WrapperForSource(source);
				if (subItem != null)
					return subItem;
			}
			return null;
		}

		public GroupWrapper GroupWrapperForItemWrapper(ItemWrapper item)
		{
			if (item == null)
				return null;
			if (Contains(item))
				return this;
		    foreach (var member in _items)
			{
				var group = member as GroupWrapper;
				var result = group?.GroupWrapperForItemWrapper(item);
				if (result != null)
					return result;
			}
			return null;
		}

		public Tuple<GroupWrapper, ItemWrapper> GroupAndItemWrappersForSource(object source)
		{
			if (Source == source)
				return new Tuple<GroupWrapper,ItemWrapper>(null,this);
			foreach (var item in _items)
			{
				if (item.Source == source)
					return new Tuple<GroupWrapper, ItemWrapper>(this, item);
				var group = item as GroupWrapper;
				var subItem = group?.GroupAndItemWrappersForSource(source);
				if (subItem != null)
					return subItem;
			}
			return null;
		}
		#endregion
	}
}

