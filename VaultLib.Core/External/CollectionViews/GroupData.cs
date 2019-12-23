using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace VaultLib.Core.External.CollectionViews
{
    internal class GroupData : IGroupData, IEnumerable<object>
    {
        //-----------------------------------------------------

        #region Construction

        public GroupData(object header)
        {
            Header = header;
            _internalItems = new FastObservableCollection<object>();
            Items = new ReadOnlyObservableCollection<object>(_internalItems);
            Level = 0;
            IsBottomLevel = true;
        }

        #endregion

        //-----------------------------------------------------

        #region Fields

        private readonly FastObservableCollection<object> _internalItems;

        #endregion

        //-----------------------------------------------------

        #region Properties

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Gets the header for this group. Header can be null if items are not grouped.
        /// </summary>
        public object Header { get; }

        public int LastIndex => Count - 1;

        /// <summary>
        ///     Gets the total count of items in this group including the header and subtree items.
        /// </summary>
        public int Count
        {
            get
            {
                var count = Header == null ? 0 : 1; //if this group has header, then header is at index '0'
                if (IsBottomLevel)
                    return count += _internalItems.Count;
                foreach (GroupData item in _internalItems) count += item.Count;
                return count;
            }
        }

        /// <summary>
        ///     Gets the number of items in 'Items' collection of this group.
        /// </summary>
        public int ItemCount => Items.Count;

        /// <summary>
        ///     Gets the level of this group in the grouping. The top group is at level '1' and level is increased by one for each
        ///     child group.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        ///     Gets a value that indicates whether this group has any subgroups.
        ///     Returns: true if this group is at the bottom level and does not have any subgroups; otherwise, false.
        /// </summary>
        public bool IsBottomLevel { get; private set; }

        /// <summary>
        ///     Gets childs of this DataGroup. Childs are either nested DataGroups or grouped items at the buttom level.
        /// </summary>
        public ReadOnlyObservableCollection<object> Items { get; }

        #endregion

        //-----------------------------------------------------

        #region Methods

        /// <summary>
        ///     Returns the object at the given index, including headers
        /// </summary>
        internal object GetObjectAt(int index)
        {
            if (index < 0 || index > LastIndex)
                throw new IndexOutOfRangeException(
                    $"The given {nameof(index)} value is out of range of the valid indexes for this collection.");

            if (IsBottomLevel)
                if (Header != null)
                    return index == 0 ? Header : _internalItems[--index];
                else
                    return _internalItems[index];
            if (index == 0)
                return Header;
            index--; //since this group header is at index 0
            foreach (GroupData group in _internalItems
            ) //we assume that _items either contains GroupData or other items if IsBottomLevel is true
                if (index <= group.LastIndex)
                    return group.GetObjectAt(index);
                else
                    index -= group.Count;
            throw new InvalidOperationException("Error in retrieving item from the group.");
            //return null;
        }

        /// <summary>
        ///     Returns the GroupData that has the given item as one of it's items
        /// </summary>
        internal GroupData GetParentGroup(object item)
        {
            if (IsBottomLevel)
            {
                if (_internalItems.Contains(item))
                    return this;
                return null;
            }

            foreach (GroupData group in _internalItems)
            {
                var parent = group.GetParentGroup(item);
                if (parent != null)
                    return parent;
            }

            return null;
        }

        /// <summary>
        ///     Removes the item at the given index in this group or this group nested groups  and retruns the group that contained
        ///     the removed item.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Retruns the group that contained the removed item at the given index.</returns>
        internal List<GroupData> RemoveObjectAt(int index)
        {
            if (index < 0 || index > LastIndex)
                throw new IndexOutOfRangeException(
                    $"The given {nameof(index)} value is out of range of the valid indexes for this collection.");
            if (IsBottomLevel)
            {
                if (Header != null)
                    _internalItems.RemoveAt(--index);
                else
                    _internalItems.RemoveAt(index);
                return _internalItems.Count == 0 ? new List<GroupData> { this } : new List<GroupData>();
            }

            List<GroupData> emptiedGroups = null;
            if (index == 0)
                throw new InvalidOperationException(
                    "Can not remove headers from the GroupDatas. Emptied groups will be automatically removed.");
            index--; //since this group header is at index 0
            foreach (GroupData group in _internalItems
            ) //we assume that _items either contains GroupData or other items if IsBottomLevel is true
                if (index <= group.LastIndex)
                {
                    emptiedGroups = group.RemoveObjectAt(index);
                    break;
                }
                else
                {
                    index -= group.Count;
                    if (index < 0)
                        return null;
                }

            if (emptiedGroups.Count > 0)
            {
                _internalItems.Remove(emptiedGroups.Last());
                if (_internalItems.Count == 0)
                    emptiedGroups.Add(this);
            }

            return emptiedGroups;
        }

        internal void AddItemToGroup(object item, int level, IEnumerable<object> headers)
        {
            if (headers == null || headers.Count() == 0)
            {
                _internalItems.Add(item);
                Level = level;
                IsBottomLevel = true;
                NotifyOfPropertyChange(nameof(Level));
                NotifyOfPropertyChange(nameof(Count));
                NotifyOfPropertyChange(nameof(ItemCount));
                NotifyOfPropertyChange(nameof(IsBottomLevel));
            }
            else
            {
                IsBottomLevel = false;
                var child = _internalItems.FirstOrDefault(i =>
                    i is GroupData && ((GroupData)i).Header.Equals(headers.First())) as GroupData;
                if (child != null)
                {
                    child.AddItemToGroup(item, ++level, headers.Skip(1));
                }
                else
                {
                    var newGroup = new GroupData(headers.First());
                    newGroup.AddItemToGroup(item, ++level, headers.Skip(1));
                    _internalItems.Add(newGroup);
                }
            }
        }

        internal void Move(int oldIndex, int newIndex)
        {
            _internalItems.Move(oldIndex, newIndex);
        }

        /// <summary>
        ///     This method only is called when this groupData has no header and items in the parent collectionView are not grouped
        ///     or sorted.
        /// </summary>
        internal void InsertRange(int index, IEnumerable<object> items)
        {
            _internalItems.InsertRange(index, items);
            NotifyOfPropertyChange(nameof(Count));
            NotifyOfPropertyChange(nameof(ItemCount));
        }

        internal void Insert(int index, object item)
        {
            _internalItems.Insert(index, item);
            NotifyOfPropertyChange(nameof(Count));
            NotifyOfPropertyChange(nameof(ItemCount));
        }

        /// <summary>
        ///     Returns the index that item should be placed in the collection based on the sorting criterias
        /// </summary>
        private static int GetInsertIndex(IList<object> items, object newItem, Comparison<object> comparison)
        {
            for (var i = 0; i < items.Count; i++)
                if (comparison(newItem, items[i]) <= 0)
                    return i;
            return items.Count;
        }

        /// <summary>
        ///     This method only is called when this groupData has no header and items in the parent collectionView are not
        ///     grouped.
        /// </summary>
        internal void RemoveRangeAt(int oldStartingIndex, int count)
        {
            _internalItems.RemoveRangeAt(oldStartingIndex, count);
            NotifyOfPropertyChange(nameof(Count));
            NotifyOfPropertyChange(nameof(ItemCount));
        }

        /// <summary>
        ///     This method only is called when this groupData has no header and items in the parent collectionView are not
        ///     grouped.
        /// </summary>
        internal void ReplaceAt(int index, object newValue)
        {
            _internalItems[index] = newValue;
        }

        public IEnumerator<object> GetEnumerator()
        {
            if (Header != null) yield return Header;
            foreach (var item in _internalItems)
                if (item is GroupData)
                    foreach (var child in (GroupData)item)
                        yield return child;
                else
                    yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void SortItems(Comparison<object> comparison)
        {
            if (IsBottomLevel)
            {
                var sorted = new List<object>(_internalItems);
                sorted.Sort(comparison);
                _internalItems.ReplaceRange(sorted);
            }
            else
            {
                foreach (GroupData group in _internalItems)
                    group.SortItems(comparison);
            }
        }

        internal int IndexOf(object value)
        {
            var index = -1;
            foreach (var item in this)
            {
                index++;
                if (item.Equals(value))
                    return index;
            }

            return -1;
        }

        internal void SortHeaders(IList<GroupDescription> description)
        {
            if (IsBottomLevel)
                return;
            var currentGroup = description[Level];
            var comparer = currentGroup.GroupHeaderComparer;
            if (comparer == null)
                return;

            var sorted = _internalItems.OrderBy(g => ((GroupData)g).Header, comparer)
                .ToList(); //check why to list is needed
            _internalItems.ReplaceRange(sorted);
            foreach (GroupData group in _internalItems) group.SortHeaders(description);
        }

        /// <summary>
        ///     Notifies subscribers of the property change.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void NotifyOfPropertyChange([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            PropertyChanged?.Invoke(this, propertyChangedEventArgs);
        }

        #endregion

        //-----------------------------------------------------
    }
}