using MvvmCross.WeakSubscription;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace VaultLib.Core.External.CollectionViews
{
    public class CollectionView : ICollectionView
    {
        //-----------------------------------------------------

        #region Construction

        public CollectionView(IList sourceCollection)
        {
            _sourceCollection = sourceCollection ?? throw new ArgumentNullException(nameof(sourceCollection));
            _groups = new FastObservableCollection<IGroupData>();
            Groups = new ReadOnlyObservableCollection<IGroupData>(_groups);
            GroupDescriptions = new FastObservableCollection<GroupDescription>();
            GroupDescriptions.CollectionChanged += _groupDescriptions_CollectionChanged;
            SortDescriptions = new FastObservableCollection<SortDescription>();
            SortDescriptions.CollectionChanged += _sortDescriptions_CollectionChanged;
            var newObservable = _sourceCollection as INotifyCollectionChanged;
            if (newObservable != null)
                _subscription = newObservable.WeakSubscribe(OnSourceCollection_CollectionChanged);
            Init();
        }

        #endregion

        //-----------------------------------------------------

        #region Fields

        private MvxNotifyCollectionChangedEventSubscription _subscription;
        private static readonly string HeaderNameForNullItems = "Nulls";
        private readonly FastObservableCollection<IGroupData> _groups;
        private readonly IList _sourceCollection;
        private List<object> _internalSource;
        private Predicate<object> _filter;

        #endregion

        //-----------------------------------------------------

        #region Properties & Methods

        public object this[int index]
        {
            get => GetObjectValueAt(index);
            set => throw new NotSupportedException("The collection is read only.");
        }

        public ReadOnlyObservableCollection<IGroupData> Groups { get; }

        public Predicate<object> Filter
        {
            get => _filter;
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    OnFilterChanged();
                }
            }
        }

        public bool IsFixedSize => false;

        public bool IsReadOnly => true;

        public int Count { get; private set; }

        public bool IsSynchronized => false;

        public object SyncRoot => null;

        public FastObservableCollection<GroupDescription> GroupDescriptions { get; }

        public FastObservableCollection<SortDescription> SortDescriptions { get; }

        public bool CanGroup => true;

        public bool CanSort => true;

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public int Add(object value)
        {
            throw new NotSupportedException("The collection is read only.");
        }

        public void Clear()
        {
            throw new NotSupportedException("The collection is read only.");
        }

        public bool Contains(object value)
        {
            foreach (GroupData group in _groups)
                foreach (var item in group)
                    if (value.Equals(item))
                        return true;
            return false;
        }

        public void CopyTo(Array array, int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), "'index' can not be a negative value.");
            if (Count > array.Length - index - 1)
                throw new ArgumentOutOfRangeException(nameof(index),
                    "The number of elements in the source collection is greater than the available space from index to the end of the destination array.");
            for (var i = 0; i < Count; i++) array.SetValue(GetObjectValueAt(i), index++);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (GroupData group in _groups)
                foreach (var item in group)
                    yield return item;
            //int currentIndex = 0;
            //Tuple<int, int> groupPairIndex = ConvertToGroupIndex(currentIndex);
            //while (groupPairIndex.Item1 >= 0)
            //{
            //    yield return (_groups[groupPairIndex.Item1] as GroupData).GetObjectAt(groupPairIndex.Item2);
            //    currentIndex++;
            //    groupPairIndex = ConvertToGroupIndex(currentIndex);
            //}
        }

        public int IndexOf(object value)
        {
            return GetIndexInCollectionView(value);
        }

        public void Insert(int index, object value)
        {
            throw new NotSupportedException("The collection is read only.");
        }

        public void Refresh()
        {
            Init();
            NotifyCollectionReset();
        }

        public void Remove(object value)
        {
            throw new NotSupportedException("The collection is read only.");
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException("The collection is read only.");
        }

        #endregion

        //-----------------------------------------------------

        #region Private Methods

        private void Init()
        {
            CreateInternalSource();
            SortCollection(_internalSource);
            var groupedData = GroupCollectionItems(_internalSource);
            _groups.ReplaceRange(groupedData);
            UpdateCount();
        }

        private void _sortDescriptions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (SortDescriptions.Count == 0)
            {
                Init(); // We need to recreate this collectionView in order to revert back to sorting of the source collection.
            }
            else
            {
                SortCollection(_internalSource); //Updates internal source collection
                foreach (var group in _groups) ((GroupData)group).SortItems(SourceItemsComparison);
            }

            NotifyCollectionReset();
        }

        private void _groupDescriptions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var groupedData = GroupCollectionItems(_internalSource);
            _groups.ReplaceRange(groupedData);
            UpdateCount();
            NotifyCollectionReset();
        }

        private void OnSourceCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SyncViewBySourceChanges(e);
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        protected virtual void SortCollection(IEnumerable<object> collection)
        {
            if (SortDescriptions.Count > 0)
            {
                if (collection is List<object>)
                    ((List<object>)collection).Sort(SourceItemsComparison);
                else if (collection is object[])
                    Array.Sort((object[])collection, SourceItemsComparison);
                else
                    collection.OrderBy(i => i, new SourceItemsComparer(SortDescriptions));
            }
        }

        protected virtual int SourceItemsComparison(object object1, object object2)
        {
            var result = 0;
            var lvl = 0;
            if (SortDescriptions.Count == 0)
                return result;
            while (lvl < SortDescriptions.Count)
            {
                var currentSort = SortDescriptions[lvl];
                var smaller = currentSort.Direction == ListSortDirection.Ascending ? -1 : 1;
                var greater = -smaller;
                if (object1 == null && object2 == null)
                {
                    lvl++;
                    continue; //this sort is match, continue to the next sort criteria
                }

                if (object1 == null)
                    return smaller;
                if (object2 == null) return greater;

                if (currentSort.PropertyValueProvider != null)
                {
                    var x = currentSort.PropertyValueProvider(object1);
                    var y = currentSort.PropertyValueProvider(object2);
                    if (x == null && y == null)
                    {
                        lvl++;
                        continue; //this sort is match, continue to the next sort criteria
                    }

                    if (x == null)
                        return smaller;
                    if (y == null) return greater;

                    result = x.CompareTo(y) * greater;
                    if (result != 0)
                        return result;
                    lvl++;
                }
                else if (!string.IsNullOrEmpty(currentSort.PropertyName))
                {
                    var val1 = object1.GetType().GetRuntimeProperty(currentSort.PropertyName)?.GetValue(object1);
                    var val2 = object2.GetType().GetRuntimeProperty(currentSort.PropertyName)?.GetValue(object2);
                    if (val1 == null && val2 == null)
                    {
                        lvl++;
                        continue; //this sort is match, continue to the next sort criteria
                    }

                    if (val1 == null)
                        return smaller;
                    if (val2 == null) return greater;

                    var x = (IComparable)val1; //items must be IComparable
                    result = x.CompareTo(val2) * greater;
                    if (result != 0)
                        return result;
                    lvl++;
                }
                else
                {
                    var x = (IComparable)object1; //items must be IComparable
                    result = x.CompareTo(object2) * greater;
                    if (result != 0)
                        return result;
                    lvl++;
                }
            }

            return result;
        }

        protected virtual IEnumerable<IGroupData> GroupCollectionItems(IEnumerable<object> collection)
        {
            var groups = new List<GroupData>();

            foreach (var item in collection)
            {
                var groupNames = GetItemGroupNames(item);
                var alreadyCreated = FindMatchingGroup(groups, groupNames) as GroupData;
                if (alreadyCreated != null)
                {
                    alreadyCreated.AddItemToGroup(item, 1, groupNames.Skip(1));
                }
                else
                {
                    var header = groupNames.Length > 0 ? groupNames[0] ?? HeaderNameForNullItems : null;
                    var group = new GroupData(header);
                    group.AddItemToGroup(item, 1, groupNames.Skip(1));
                    groups.Add(group);
                }
            }

            if (groups.Count == 0) //if no item was in the collection, we add an empty group
            {
                var group = new GroupData(null);
                groups.Add(group);
            }

            var sorted = SortGroupHeaders(groups);

            return sorted;
        }

        internal IEnumerable<IGroupData> SortGroupHeaders(List<GroupData> groups)
        {
            if (GroupDescriptions.Count == 0)
                return groups;
            var currentGroup = GroupDescriptions[0];
            var comparer = currentGroup.GroupHeaderComparer;
            if (comparer == null)
                return groups;

            var sorted = groups.OrderBy(g => g.Header, comparer);

            //_groups.ReplaceRange(sorted);

            foreach (var group in sorted) group.SortHeaders(GroupDescriptions);
            return sorted;
        }

        protected virtual object[] GetItemGroupNames(object item)
        {
            var names = new object[GroupDescriptions.Count];
            for (var i = 0; i < GroupDescriptions.Count; i++)
                names[i] = GroupDescriptions[i].GroupNameFromItem(item, i, CultureInfo.CurrentCulture);
            return names;
        }

        protected virtual IGroupData FindMatchingGroup(IEnumerable<IGroupData> groups, object[] groupNames)
        {
            var alreadyCreatedGroup = groups.FirstOrDefault(g =>
                {
                    if (groupNames.Length == 0)
                        return true;
                    return g.Header.Equals(groupNames[0]);
                }
            );
            return alreadyCreatedGroup;
        }

        protected virtual void OnFilterChanged()
        {
            Init();
            NotifyOfPropertyChange(nameof(Filter));
            NotifyCollectionReset();
        }

        protected virtual void CreateInternalSource()
        {
            _internalSource = GetFilteredItems(_sourceCollection);
        }

        #region Index mapping

        /// <summary>
        ///     Returns the group which contains an item corresponding to the index of this collectionView or null if the index in
        ///     out of bound.
        /// </summary>
        private GroupData GetGroupFromIndex(int index)
        {
            foreach (GroupData group in _groups)
            {
                if (index <= group.LastIndex)
                    return group;
                index -= group.Count;
            }

            return null;
        }

        /// <summary>
        ///     Converts an index of this collectionView to the position in the groups
        /// </summary>
        private Tuple<int, int> ConvertToGroupIndex(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), "'index' can not be a negative value.");
            var groupIndex = 0;
            foreach (GroupData group in _groups)
            {
                if (index <= group.LastIndex)
                    return new Tuple<int, int>(groupIndex, index);
                index -= group.Count;
                groupIndex++;
                if (index < 0 || groupIndex >= _groups.Count)
                    break;
            }

            return new Tuple<int, int>(-1, -1);
        }

        private int GetIndexInCollectionView(object value)
        {
            var index = -1;
            foreach (GroupData group in _groups)
                foreach (var item in group)
                {
                    index++;
                    if (value.Equals(item))
                        return index;
                }

            return -1;
        }

        private int GetIndexInCollectionView(IGroupData existingGroup, int indexInGroup)
        {
            if (existingGroup == null)
                throw new ArgumentNullException(nameof(existingGroup));
            if (indexInGroup < 0)
                throw new ArgumentException("indexInGroup can not be a negative value.");
            var indexInCollectionView = 0;
            var groupIndex = _groups.IndexOf(existingGroup);
            if (groupIndex < 0)
                throw new ArgumentException(
                    $"the given IGroupData does not exists in the {nameof(Groups)} collection.");
            for (var i = 0; i < groupIndex; i++) indexInCollectionView += _groups[i].Count;
            indexInCollectionView += indexInGroup;
            return indexInCollectionView;
        }

        private int GetIndexInCollectionView(IGroupData existingGroup, object itemInGroup)
        {
            if (existingGroup == null)
                throw new ArgumentNullException(nameof(existingGroup));
            if (itemInGroup == null)
                throw new ArgumentNullException(nameof(itemInGroup));
            var indexInGroup = ((GroupData)existingGroup).IndexOf(itemInGroup);
            if (indexInGroup < 0)
                throw new InvalidOperationException("The given item does not exists in the given IGroupData");
            return GetIndexInCollectionView(existingGroup, indexInGroup);
        }

        #endregion

        #region Source Changes Synchronization

        protected virtual void SyncViewBySourceChanges(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove: //can affect grouping of items if groups become empty
                    RemoveItems(e.OldStartingIndex, e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Replace
                    : //can affect both sorting and grouping of items based on the new values
                    ReplaceItems(e.OldItems, e.NewItems, e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Add
                    : //can affect both sorting and grouping of items based on the item values
                    AddItems(e.NewItems, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Move
                    : //only affects position of the items in the collectionView if currently no sorting is applied
                    MoveItems(e.OldItems ?? e.NewItems, e.OldStartingIndex, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                default:
                    Init();
                    NotifyCollectionReset();
                    break;
            }
        }

        private void RemoveItems(int oldStartingIndex, IList oldItems)
        {
            if (SortDescriptions.Count > 0 || GroupDescriptions.Count > 0 || Filter != null)
                //Most likely ordering of items in this collectionView changed according to the source collection. Hence we remove items individually
                foreach (var item in oldItems)
                    RemoveObject(item);
            else
                //In this case, this collectionView mimics the source collection
                RemoveObjects(oldStartingIndex, oldItems);
        }

        private void RemoveObject(object toBeRemoved)
        {
            if (Filter != null && !Filter(toBeRemoved)) //if this item is not in the collectionview
                return;
            var index = -1;
            List<GroupData> emptiedGroups = null;
            foreach (GroupData group in _groups)
            {
                index = group.IndexOf(toBeRemoved);
                if (index >= 0)
                {
                    //the item is part of this group
                    emptiedGroups = group.RemoveObjectAt(index);
                    _internalSource.Remove(toBeRemoved); //updating the internal source
                    index = GetIndexInCollectionView(group,
                        index); //updating this index so it represents index of removed object in this collectionView
                    break;
                }
            }

            if (emptiedGroups != null && emptiedGroups.Count > 0 && GroupDescriptions.Count > 0)
            {
                //we remove the emptied group from the collectionView unless it is the only group in the case of _groupDescriptions.Count == 0
                _groups.Remove(emptiedGroups
                    .Last()); //the emptiedGroup is not necessarily direct child of the _groups collection
                UpdateCount();
                NotifyOfPropertyChange("Item[]");
                try
                {
                    //We have to report removed indexes in the descending order, so the views can adjust their items properly.
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,
                        toBeRemoved, index));
                    foreach (var emptiedGroup in emptiedGroups)
                        if (emptiedGroup.Header != null) //if the removed group had header
                            //header was the item before the removed object
                            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Remove, emptiedGroup.Header, --index));
                }
                catch (InvalidOperationException)
                {
                    //https://stackoverflow.com/questions/37698854/system-invalidoperationexception-n-index-in-collection-change-event-is-not-val
                    //It seems that in WPF ListCollectionView checks that the indexes provided in the eventArgs belong to the collection; if not, the exception in the subject is thrown.
                    //Since we might have already removed this indexes from the collectionView and collectionChanged event is calling asynchronously in the WPF, this in inevitable.
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }

                return;
            }

            if (index >= 0) // since item was in the collection viwe this alwasy must be true
            {
                UpdateCount();
                NotifyOfPropertyChange("Item[]");
                try
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,
                        toBeRemoved, index));
                }
                catch (InvalidOperationException)
                {
                    //same as above
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }
        }

        //this method only will be called when this collectionView is completely filled like the source collection
        private void RemoveObjects(int oldStartingIndex, IList removedItems)
        {
            if (oldStartingIndex >= 0
            ) //check the source collection to ensure that it raised the 'CollectionChanged' event properly
            {
                ((GroupData)_groups[0]).RemoveRangeAt(oldStartingIndex, removedItems.Count);
                _internalSource.RemoveRange(oldStartingIndex, removedItems.Count);
                NotifyOfPropertyChange("Item[]");
                UpdateCount();
                try
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,
                        removedItems, oldStartingIndex));
                }
                catch (NotSupportedException)
                {
                    //WPF listView does not support range actions :
                    //for (int i = removedItems.Count - 1; i >= 0; i--)
                    //{
                    //    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems[i], oldStartingIndex + i));
                    //}
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }
            else
            {
                foreach (var item in removedItems)
                {
                    var index = _internalSource.IndexOf(item);
                    ((GroupData)_groups[0]).RemoveObjectAt(index);
                    _internalSource.Remove(item);
                    OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
                }

                NotifyOfPropertyChange("Item[]");
                UpdateCount();
            }
        }

        private void ReplaceItems(IList oldItems, IList newItems, int startIndex)
        {
            if (oldItems.Count == newItems.Count)
            {
                if (SortDescriptions.Count > 0 || GroupDescriptions.Count > 0)
                {
                    Init();
                    NotifyCollectionReset();
                }
                else if (Filter != null || startIndex < 0) //if 
                {
                    var replaced = new List<object>();
                    var newValues = new List<object>();
                    for (var i = 0; i < oldItems.Count; i++)
                    {
                        var index = _internalSource.IndexOf(oldItems[i]);
                        if (index >= 0) // if items is not filtered and exists in the internal source
                        {
                            replaced.Add(oldItems[i]);
                            newValues.Add(newItems[i]);
                            _internalSource[index] = newItems[i];
                            ((GroupData)_groups[0]).ReplaceAt(index, newItems[i]);
                        }
                    }

                    if (replaced.Count > 0)
                    {
                        NotifyOfPropertyChange("Item[]");
                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,
                            newValues, replaced));
                    }
                }
                else
                {
                    //this collection should match the source collection
                    var index = startIndex;
                    for (var i = 0; i < oldItems.Count; i++)
                    {
                        _internalSource[index] = newItems[i];
                        ((GroupData)_groups[0]).ReplaceAt(index, newItems[i]);
                        index++;
                    }

                    NotifyOfPropertyChange("Item[]");
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,
                        newItems, oldItems, startIndex));
                }
            }
            else
            {
                //It can not be a valid replace action for collectionchanged event; We need to recreate the collectionview
                Init();
                NotifyCollectionReset();
            }
        }

        private void AddItems(IList newItems, int newStartingIndex)
        {
            if (newItems == null)
                throw new ArgumentNullException(nameof(newItems));
            if (newItems.Count == 0)
                return;

            if (SortDescriptions.Count > 0 || GroupDescriptions.Count > 0)
            {
                if (GroupDescriptions.Count == 0 && newItems.Count < 10 && _internalSource.Count > 30)
                {
                    //only sorting is applied and few items are being added to the relatively large collection 
                    var filtered = GetFilteredItems(newItems);
                    foreach (var item in filtered)
                    {
                        var insertAt = GetInsertIndex(_internalSource, item, 0, _internalSource.Count - 1);
                        _internalSource.Insert(insertAt, item);
                        ((GroupData)_groups[0]).Insert(insertAt, item);
                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                            item, insertAt));
                    }

                    UpdateCount();
                    NotifyOfPropertyChange("Item[]");
                }
                else
                {
                    Init();
                    NotifyCollectionReset();
                }
            }
            else
            {
                newStartingIndex = newStartingIndex < 0 ? _internalSource.Count : newStartingIndex;
                var filtered = GetFilteredItems(newItems);
                _internalSource.InsertRange(newStartingIndex, filtered);
                ((GroupData)_groups[0]).InsertRange(newStartingIndex, filtered);
                UpdateCount();
                NotifyOfPropertyChange("Item[]");
                try
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                        filtered, newStartingIndex));
                }
                catch (NotSupportedException)
                {
                    //WPF listView does not support range actions:
                    for (var i = 0; i < newItems.Count; i++)
                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                            newItems[i], newStartingIndex + i));
                    //OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }
        }

        private void MoveItems(IList items, int oldIndex, int newIndex)
        {
            if (SortDescriptions.Count == 0)
            {
                var filtered = GetFilteredItems(items);
                var startIndex = oldIndex;
                var endIndex = newIndex;
                foreach (var item in filtered)
                {
                    _internalSource.RemoveAt(startIndex);
                    _internalSource.Insert(endIndex, item);
                    var group = GetGroupFromItem(item);
                    if (endIndex > startIndex)
                    {
                        for (var i = endIndex - 1;
                            i >= startIndex;
                            i--) //check all affected items in the internal source
                            if (group.Items.Contains(_internalSource[i])
                            ) //if corresponding group contains the affected item
                            {
                                var collectionViewOldIndex = GetIndexInCollectionView(item);
                                var groupOldIndex = group.Items.IndexOf(item);
                                var groupNewIndex = group.Items.IndexOf(_internalSource[i]);
                                var collectionViewNewIndex = collectionViewOldIndex + groupNewIndex - groupOldIndex;
                                ((GroupData)group).Move(groupOldIndex, groupNewIndex);
                                NotifyOfPropertyChange("Item[]");
                                OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                                    NotifyCollectionChangedAction.Move, item, collectionViewNewIndex,
                                    collectionViewOldIndex));
                                break;
                            }
                    }
                    else if (endIndex < startIndex)
                    {
                        for (var i = endIndex + 1; i <= startIndex; i++)
                            if (group.Items.Contains(_internalSource[i]))
                            {
                                var collectionViewOldIndex = GetIndexInCollectionView(item);
                                var groupOldIndex = group.Items.IndexOf(item);
                                var groupNewIndex = group.Items.IndexOf(_internalSource[i]);
                                var collectionViewNewIndex = collectionViewOldIndex + groupNewIndex - groupOldIndex;
                                ((GroupData)group).Move(groupOldIndex, groupNewIndex);
                                NotifyOfPropertyChange("Item[]");
                                OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                                    NotifyCollectionChangedAction.Move, item, collectionViewNewIndex,
                                    collectionViewOldIndex));
                                break;
                            }
                    }

                    startIndex++;
                    endIndex++;
                }
            }
        }

        #endregion

        /// <summary>
        ///     Returns the object at the given index in the collectionView
        /// </summary>
        /// <param name="index">index of item in the collectionCiew</param>
        /// <returns></returns>
        private object GetObjectValueAt(int index)
        {
            var groupPairIndex = ConvertToGroupIndex(index);
            if (groupPairIndex.Item1 >= 0)
                return (_groups[groupPairIndex.Item1] as GroupData).GetObjectAt(groupPairIndex.Item2);
            throw new IndexOutOfRangeException(
                $"The given {nameof(index)} value is out of range of valid indexes for this collection.");
        }

        /// <summary>
        ///     Returns the index that item should be placed in the previously sorted collection based on the sorting criterias
        /// </summary>
        private int GetInsertIndex(IList<object> items, object newItem, int startIndex, int endIndex)
        {
            if (startIndex < 0 || endIndex >= items.Count)
                throw new ArgumentOutOfRangeException(
                    "Given index was out of range of the valid indexs for the given collection.");
            if (startIndex > endIndex)
                throw new ArgumentException("startIndex should be less than or equal to the endIndex");
            if (SourceItemsComparison(newItem, items[startIndex]) <= 0)
                return startIndex;
            if (SourceItemsComparison(newItem, items[endIndex]) >= 0)
                return endIndex + 1;

            var middle = startIndex + (endIndex - startIndex) / 2;
            var comparison = SourceItemsComparison(newItem, items[middle]);
            if (comparison == 0)
                return middle;
            if (comparison < 0)
                return GetInsertIndex(items, newItem, startIndex, middle);
            comparison = SourceItemsComparison(newItem, items[middle + 1]);
            if (comparison <= 0)
                return middle + 1;
            return GetInsertIndex(items, newItem, middle + 1, endIndex);
        }

        /// <summary>
        ///     Returns count of the items in the collectionview
        /// </summary>
        /// <returns></returns>
        private int GetCount()
        {
            return _groups.Sum(g => g.Count);
        }

        private void UpdateCount()
        {
            Count = GetCount();
            NotifyOfPropertyChange(nameof(Count));
        }

        private List<object> GetFilteredItems(IEnumerable items)
        {
            var filtered = new List<object>();
            if (Filter != null)
                foreach (var item in items)
                {
                    if (Filter(item))
                        filtered.Add(item);
                }
            else
                foreach (var item in items)
                    filtered.Add(item);

            return filtered;
        }

        /// <summary>
        ///     Returns the group which its Items collection contains the given item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private IGroupData GetGroupFromItem(object item)
        {
            foreach (GroupData group in _groups)
            {
                var parent = group.GetParentGroup(item);
                if (parent != null)
                    return parent;
            }

            return null;
        }

        private void NotifyCollectionReset()
        {
            NotifyOfPropertyChange("Item[]");
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        ///     Notifies subscribers of the property change.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public virtual void NotifyOfPropertyChange([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        //-----------------------------------------------------
    }
}