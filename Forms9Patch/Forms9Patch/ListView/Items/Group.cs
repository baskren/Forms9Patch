using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.ObjectModel;
using PCL.Utils;

namespace Forms9Patch
{
	class Group : Item<object>, IList<Item>, ICollection<Item>, IEnumerable<Item>, IEnumerable, IList, ICollection, IReadOnlyList<Item>, IReadOnlyCollection<Item>, INotifyCollectionChanged  {


		#region Source / Target (Visibles) coupling
		bool _notifySourceOfChanges;
		public bool NotifySourceOfChanges {
			get {
				return _notifySourceOfChanges;
			}
			set {
				foreach (var item in this) {
					var group = item as Group;
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


		/*
		IList Source {
			get { return Value; }
		}
		*/

		/*
	 	WeakReference weakSource;
		Group Source {
			get { return weakSource?.Target as Group; }
			set { weakSource = new WeakReference (value); }
		}

		*/
		Func<object,bool> _visibleItemTest;
		public Func<object,bool> VisiblityTest {
			get { return _visibleItemTest; }
			set {
				_visibleItemTest = value;
				foreach (var item in _items) {
					var group = item as Group;
					if (group!=null) 
						group.VisiblityTest = group.VisiblityTest ?? VisiblityTest;
				}
			}
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

		// note: Even though Source's items are not unique (ex: Source= { a, a, "pizza", null, null }) Group's items are because they are each created from a new instance of type Item.

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

		int SourceIndexOf(Item item) {
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


		void SourceInsert(Item item) {
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

		void SourceAdd(Item item) {
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

		void SourceRemove(Item item) {
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
		void CommonAdd(Item item) {
			if (item == null)
				return;
			item.PropertyChanged += OnItemPropertyChanged;
			item.PropertyChanging += OnItemPropertyChanging;
			//item.LongPressed += OnLongPressed;
			//item.LongPressing += OnLongPressing;
			if (NotifySourceOfChanges) SourceAdd (item);
			var group = item as Group;
			if (group!=null) {
				group.VisiblityTest = group.VisiblityTest ?? VisiblityTest;
				//group.ValidMateTriggerProperty = group.ValidMateTriggerProperty ?? ValidMateTriggerProperty;
			}
			ResetSeparators ();
		}

		void CommonInsert(Item item) {
			if (item == null)
				return;
			item.PropertyChanged += OnItemPropertyChanged;
			item.PropertyChanging += OnItemPropertyChanging;
			//item.LongPressed += OnLongPressed;
			//item.LongPressing += OnLongPressing;
			if (NotifySourceOfChanges) SourceInsert (item);
			var group = item as Group;
			if (group!=null) {
				group.VisiblityTest = group.VisiblityTest ?? VisiblityTest;
				//group.ValidMateTriggerProperty = group.ValidMateTriggerProperty ?? ValidMateTriggerProperty;
			}
			ResetSeparators ();
		}

		void CommonRemove(Item item) {
			if (item == null)
				return;
			if (NotifySourceOfChanges) SourceRemove (item);
			item.PropertyChanged -= OnItemPropertyChanged;
			item.PropertyChanging -= OnItemPropertyChanging;
			//item.LongPressed -= OnLongPressed;
			//item.LongPressing -= OnLongPressing;
			ResetSeparators ();
		}

		void ResetSeparators() {
			for (int i = 0; i < _items.Count; i++)
				_items[i].SeparatorIsVisible = (i != 0);
		}
		#endregion


		#region IList<T> implementation
		public int IndexOf (Item item) { 
			return _items.IndexOf (item); 
		}

		public void Insert (int index, Item item) { 
			if (item == null)
				return;
			if (Contains (item))
				throw new NotSupportedException ();
			_items.Insert (index, item); 
			CommonInsert (item);
		}

		public void RemoveAt (int index) { 
			CommonRemove (_items [index]);
			_items.RemoveAt (index); 
		}

		public Item this [int index] {
			get { return _items [index]; }
			set { 
				if (_items.Contains (value) || index < 0 || index > _items.Count)
					// will this create problems for move/swap operations?  I think so!  
					throw new NotSupportedException ();
				if (index < _items.Count) 
					CommonRemove (_items [index]);
				System.Diagnostics.Debug.WriteLine ("\t\tsetting _item start");
				_items [index] = value; 
				System.Diagnostics.Debug.WriteLine ("\t\tsetting _item finish");
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

		Item CreateItem(object sourceObject) {

			string propertyName=null;
			List<string> subgroupPropertyMap;
			if (_subPropertyMap != null && _subPropertyMap.Count > 0) {
				subgroupPropertyMap = new List<string> (_subPropertyMap);
				subgroupPropertyMap [0] = null;
				propertyName = _subPropertyMap [0];
			}

			var children = string.IsNullOrWhiteSpace (propertyName) ? sourceObject : sourceObject.GetPropertyValue (propertyName);
			var iEnumerable = children as IEnumerable;
			if (iEnumerable != null) {
				// groups
				VerifyContentType (GroupContentType.Lists);
				return new Group (sourceObject, _subPropertyMap, VisiblityTest);
			}
			// items
			VerifyContentType(GroupContentType.Objects);
			var objType = sourceObject.GetType ();
			var itemType = typeof(Item<>).MakeGenericType (new[] { objType });
			var item = (Item)Activator.CreateInstance (itemType);
			item.Source = sourceObject;
			return item;
		}

		void AddSourceObject(object sourceObject) {
			if (VisiblityTest == null || VisiblityTest (sourceObject)) {
				NotifySourceOfChanges = false;
				var item = CreateItem (sourceObject);
				Add (item);
				NotifySourceOfChanges = true;
			}
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
				if (VisiblityTest == null || VisiblityTest (sourceItem)) {
					localIndex++;
					if (sourceIndex == requestedSourceIndex)
						return localIndex;
				}
				sourceIndex++;
			}
			return localIndex;
		}

		void InsertSourceObject(int sourceIndex, object sourceObject) {
			if (VisiblityTest == null || VisiblityTest (sourceObject)) {
				NotifySourceOfChanges = false;
				var index = LocalIndexFromSourceIndex (sourceIndex);
				var item = CreateItem (sourceObject);
				if (index > -1)
					Insert (index, item);
				else
					Add (item);
				NotifySourceOfChanges = true;
			}
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
			if (VisiblityTest == null || VisiblityTest (newSourceObject) && VisiblityTest (oldSourceObject)) {
				// replace object
				NotifySourceOfChanges = false;
				var item = CreateItem (newSourceObject);
				this [index] = item;
				NotifySourceOfChanges = true;
			} else if (VisiblityTest (oldSourceObject)) {
				// remove object
				RemoveItemWithSourceIndex (sourceIndex);
			} else if (VisiblityTest (newSourceObject)) {
				// insert object
				InsertSourceObject(sourceIndex, newSourceObject);
			}
		}
		#endregion


		#region ICollection<T> implementation
		public void Add (Item item) { 
			if (item == null)
				return;
			if (Contains (item))
				throw new NotSupportedException ();
			_items.Add (item); 
			CommonAdd (item);
		}

		public void Clear () { 
			for (int i = _items.Count - 1; i >= 0; i--) {
				var subGroup = _items [i] as Group;
				if (subGroup == null)
					RemoveAt (i);
				else
					subGroup.Clear ();
			}
		}

		public bool Contains (Item item) { 
			return item != null && DeepContains (item);
		}

		public void CopyTo (Item[] array, int arrayIndex) { 
			if (array == null)
				return;
			int offset = 0;
			foreach (var item in array)
				Insert (arrayIndex + offset++, item);
		}

		public bool Remove (Item item) { 
			// if item is a group, will garbage collection clean up its items? - It better not because we may want to use it elsewhere
			CommonRemove(item);
			return _items.Remove (item); 
		}

		public int Count { 
			get { return _items.Count; }
		}

		public bool IsReadOnly {
			get { return false; }
		}
		#endregion


		#region IEnumerable<T> implementation
		public IEnumerator<Item> GetEnumerator () {
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
			Add (value as Item);
			return _items.Count - 1;
		}

		public bool Contains (object value)
		{
			return Contains (value as Item);
		}

		public int IndexOf (object value)
		{
			return IndexOf (value as Item);
		}

		public void Insert (int index, object value)
		{
			var item = value as Item;
			if (item != null) {
				Insert (index, item);
			} else
				throw new InvalidCastException ();
		}

		public void Remove (object value)
		{
			var item = value as Item;
			if (item != null) 
				Remove (item);
		}

		public bool IsFixedSize {
			get {
				return false;
			}
		}

		public void CopyTo (Array array, int index)
		{
			CopyTo ( array as Item[], index);
		}

		public bool IsSynchronized {
			get { throw new NotImplementedException (); }
		}

		public object SyncRoot {
			get { throw new NotImplementedException (); }
		}

		object IList.this [int index] {
			get {
				return _items [index];
			}
			set {
				var item = value as Item;
				if (item != null) {
					if (Contains (item))
						throw new NotSupportedException ();
					if (index <= _items.Count)
						CommonRemove (_items [index]);
					_items [index] = item;
					CommonAdd (item);
				}
				else
					throw new InvalidCastException();
			}
		}
		#endregion


		#region Deep List enhancements

		public bool DeepContains(Item item) {
			if (_items.Contains (item))
				return true;
			foreach (var member in _items) {
				var group = member as Group;
				if (group != null && group.DeepContains (item))
					return true;
			}
			return false;
		}

		public int[] DeepSourceIndexOf(Item item) {
			int pos = SourceIndexOf (item);
			if (pos > -1) {
				return new [] { pos };
			} else {
				for (pos = 0; pos < _items.Count; pos++) {
					var subGroup = _items [pos] as Group;
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

		public int[] DeepIndexOf(Item item) {
			int pos = IndexOf (item);
			if (pos > -1) {
				return new [] { pos };
			} else {
				for (pos = 0; pos < _items.Count; pos++) {
					var subGroup = _items [pos] as Group;
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

		public void DeepInsert (int[] deepIndex, Item item) { 
			if (DeepContains (item))
				throw new NotSupportedException ("DeepInsert: Group already contain item");
				//return;
			if (deepIndex==null || deepIndex.Length == 0)
				throw new NotSupportedException ("DeepInsert: deepIndex=null || deepIndex.Length=0");
			if (deepIndex.Length == 1) {
				Insert (deepIndex [0], item);
				return;
			}
			var subGroup = _items [deepIndex [0]] as Group;
			if (subGroup == null)
				throw new NotSupportedException ("DeepInsert: deepIndex.Length > 1 but item at index is not Group");
			var subGroupDeepIndex = new int[deepIndex.Length - 1];
			Array.Copy (deepIndex, 1, subGroupDeepIndex, 0, deepIndex.Length - 1);
			subGroup.DeepInsert (subGroupDeepIndex, item);
		}

		public void DeepRemove(Item item) {
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
			var subGroup = _items [deepIndex [0]] as Group;
			if (subGroup == null)
				throw new NotSupportedException ("DeepRemove: deepIndex.Length > 1 but item at index is not Group");
			var subGroupDeepIndex = new int[deepIndex.Length - 1];
			Array.Copy (deepIndex, 1, subGroupDeepIndex, 0, deepIndex.Length - 1);
			subGroup.DeepRemoveAt (subGroupDeepIndex);
		}

		public Item ItemAtDeepIndex(int[] deepIndex) {
			if (deepIndex==null || deepIndex.Length == 0)
				throw new NotSupportedException ("DeepRemove: deepIndex=null || deepIndex.Length=0");
			if (deepIndex.Length == 1) {
				return _items [deepIndex [0]];
			}
			var subGroup = _items [deepIndex [0]] as Group;
			if (subGroup == null)
				throw new NotSupportedException ("DeepRemove: deepIndex.Length > 1 but item at index is not Group");
			var subGroupDeepIndex = new int[deepIndex.Length - 1];
			Array.Copy (deepIndex, 1, subGroupDeepIndex, 0, deepIndex.Length - 1);
			return subGroup.ItemAtDeepIndex (subGroupDeepIndex);
		}

		public void DeepSwapItems(Item item1, Item item2) {
			var deepIndex1 = DeepIndexOf (item1);
			var deepIndex2 = DeepIndexOf (item2);
			if (deepIndex1 == deepIndex2)
				return;
			DeepSet (deepIndex1, item2);
			DeepSet (deepIndex2, item1);
		}

		public void DeepSet(int[] deepIndex, Item item) {
			if (deepIndex == null || deepIndex.Length == 0)
				return;
			int index = deepIndex [0];
			if (deepIndex.Length == 1) {// && deepIndex[0] >=0 && deepIndex[0] <= items.Count) {
				if (index < 0 || index > _items.Count)
					return;
				//items [index] = item;
				this[index] = item;
			} else {
				var subGroup = _items [index] as Group;
				if (subGroup == null)
					throw new NotSupportedException ("DeepSet: deepIndex.Length > 1 but item at index is not Group");
				// create a new index (subGroupDeepIndex) that contains everything except the [0] index of deepIndex.
				var subGroupDeepIndex = new int[deepIndex.Length - 1];
				Array.Copy (deepIndex, 1, subGroupDeepIndex, 0, deepIndex.Length - 1);
				subGroup.DeepSet (subGroupDeepIndex, item);
			}
		}
		#endregion


		#region Fields

		ObservableCollection<Item> _items = new ObservableCollection<Item> ();

		List<string> _subPropertyMap;

		IEnumerable SourceChildren;

		string childrenPropertyName;
		#endregion



		#region Constructors
		public Group(object source, List<string> sourcePropertyMap, Func<object,bool>visibleItemTest=null) : this() {
			VisiblityTest = visibleItemTest;
			ContentType = GroupContentType.Unknown;

			if (sourcePropertyMap != null && sourcePropertyMap.Count > 0) {
				_subPropertyMap = sourcePropertyMap.GetRange (1, sourcePropertyMap.Count - 1);
				childrenPropertyName = sourcePropertyMap [0];
			}

			Source = source;
			SourceChildren = string.IsNullOrWhiteSpace (childrenPropertyName) ? source as IEnumerable : source.GetPropertyValue (childrenPropertyName) as IEnumerable;
			if (SourceChildren == null || Source==null) {
				throw new ArgumentException ("Group source must be IEnumerable -or- SourcePropertyMap is set to an IEnumerable property of source");
			}
			foreach (var obj in SourceChildren)
				AddSourceObject (obj);

			var observableCollection = SourceChildren as INotifyCollectionChanged;
			if (observableCollection != null) {
				observableCollection.CollectionChanged += OnSourceCollectionChanged;
			}
		}

		public Group()  {
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
					// we don't have an index but we can figure it out IF the objects are unique!
					throw new NotSupportedException ("Cannot remove Source object with unknown index");
				for (int i = e.OldItems.Count - 1; i >= 0; i--)
					RemoveItemWithSourceIndex (i + e.OldStartingIndex);
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
		protected override void OnPropertyChanged (string propertyName = null)
		{
			if (propertyName == "SeparatorIsVisible") {
				for (int i = 0; i < _items.Count; i++) {
					var item = _items [i];
					var group = item as Group;
					if (group != null) {
						group.SeparatorIsVisible = SeparatorIsVisible;
					} else if (item != null) {
						item.SeparatorIsVisible = SeparatorIsVisible && i > 0;
					}
				}
			} else if (propertyName == "SeparatorColor") {
				foreach (var item in _items) {
					var listViewitem = item;
					if (listViewitem != null) {
						listViewitem.SeparatorColor = SeparatorColor;
					}
				}
			} else if (propertyName == "BackgroundColor") {
				foreach (var item in _items) {
					var listViewitem = item;
					if (listViewitem != null) {
						listViewitem.BackgroundColor = BackgroundColor;
					}
				}
			} else {
				base.OnPropertyChanged (propertyName);
			}
		}
		#endregion


		#region Member Item Property Change Notificaiton
		public event Xamarin.Forms.PropertyChangingEventHandler ItemPropertyChanging;
		void OnItemPropertyChanging (object sender, Xamarin.Forms.PropertyChangingEventArgs e)
		{
			Xamarin.Forms.PropertyChangingEventHandler handler = ItemPropertyChanging;
			if (sender != null && handler!=null) 
				handler (sender, e);
		}

		public event PropertyChangedEventHandler ItemPropertyChanged;
		void OnItemPropertyChanged (object sender, PropertyChangedEventArgs e)
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
			PropertyChangedEventHandler handler = ItemPropertyChanged;
			if (sender != null && handler != null)
				handler (sender, e);
		}
		#endregion


	}
}

