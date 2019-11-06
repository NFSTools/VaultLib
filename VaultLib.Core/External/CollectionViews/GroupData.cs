using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace VaultLib.Core.External.CollectionViews
{
    class GroupData : IGroupData, IEnumerable<object>
    {
        //-----------------------------------------------------
        #region Fields
        FastObservableCollection<object> _internalItems;
        object _header;
        int _level;
        #endregion
        //-----------------------------------------------------
        #region Construction
        public GroupData(object header)
        {
            this._header = header;
            _internalItems = new FastObservableCollection<object>();
            Items = new ReadOnlyObservableCollection<object>(_internalItems);
            _level = 0;
            this.IsBottomLevel = true;
        }
        #endregion
        //-----------------------------------------------------
        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Gets the header for this group. Header can be null if items are not grouped.
        /// </summary>
        public object Header
        {
            get
            {
                return _header;
            }
        }

        public int LastIndex => Count - 1;
        /// <summary>
        /// Gets the total count of items in this group including the header and subtree items. 
        /// </summary>
        public int Count
        {
            get
            {
                int count = _header == null ? 0 : 1; //if this group has header, then header is at index '0'
                if (IsBottomLevel)
                    return count += _internalItems.Count;
                foreach (GroupData item in _internalItems)
                {
                    count += item.Count;
                }
                return count;
            }
        }
        /// <summary>
        /// Gets the number of items in 'Items' collection of this group.
        /// </summary>
        public int ItemCount => Items.Count;
        /// <summary>
        /// Gets the level of this group in the grouping. The top group is at level '1' and level is increased by one for each child group.
        /// </summary>
        public int Level { get => _level; }
        /// <summary>
        /// Gets a value that indicates whether this group has any subgroups.
        /// Returns: true if this group is at the bottom level and does not have any subgroups; otherwise, false.
        /// </summary>
        public bool IsBottomLevel { get; private set; }
        /// <summary>
        /// Gets childs of this DataGroup. Childs are either nested DataGroups or grouped items at the buttom level.
        /// </summary>
        public ReadOnlyObservableCollection<object> Items { get; private set; }
        #endregion
        //-----------------------------------------------------
        #region Methods
        /// <summary>
        /// Returns the object at the given index, including headers
        /// </summary>
        internal object GetObjectAt(int index)
        {
            if (index < 0 || index > this.LastIndex)
                throw new IndexOutOfRangeException($"The given {nameof(index)} value is out of range of the valid indexes for this collection.");

            if (IsBottomLevel)
                if (_header != null)
                    return index == 0 ? _header : _internalItems[--index];
                else
                    return _internalItems[index];
            if (index == 0)
                return _header;
            else
                index--; //since this group header is at index 0
            foreach (GroupData group in _internalItems)//we assume that _items either contains GroupData or other items if IsBottomLevel is true
            {
                if (index <= group.LastIndex)
                    return group.GetObjectAt(index);
                else
                    index -= group.Count;
            }
            throw new InvalidOperationException("Error in retrieving item from the group.");
            //return null;
        }
        /// <summary>
        /// Returns the GroupData that has the given item as one of it's items
        /// </summary>
        internal GroupData GetParentGroup(object item)
        {
            if (IsBottomLevel)
            {
                if (_internalItems.Contains(item))
                    return this;
                return null;
            }
            else
            {
                foreach (GroupData group in _internalItems)
                {
                    GroupData parent = group.GetParentGroup(item);
                    if (parent != null)
                        return parent;
                }
                return null;
            }
        }
        /// <summary>
        /// Removes the item at the given index in this group or this group nested groups  and retruns the group that contained the removed item.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Retruns the group that contained the removed item at the given index.</returns>
        internal List<GroupData> RemoveObjectAt(int index)
        {
            if (index < 0 || index > this.LastIndex)
                throw new IndexOutOfRangeException($"The given {nameof(index)} value is out of range of the valid indexes for this collection.");
            if (IsBottomLevel)
            {
                if (_header != null)
                    _internalItems.RemoveAt(--index);
                else
                    _internalItems.RemoveAt(index);
                return _internalItems.Count == 0 ? new List<GroupData>() { this } : new List<GroupData>();
            }
            List<GroupData> emptiedGroups = null;
            if (index == 0)
                throw new InvalidOperationException("Can not remove headers from the GroupDatas. Emptied groups will be automatically removed.");
            else
                index--; //since this group header is at index 0
            foreach (GroupData group in _internalItems)//we assume that _items either contains GroupData or other items if IsBottomLevel is true
            {
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
                this._internalItems.Add(item);
                this._level = level;
                this.IsBottomLevel = true;
                NotifyOfPropertyChange(nameof(Level));
                NotifyOfPropertyChange(nameof(Count));
                NotifyOfPropertyChange(nameof(ItemCount));
                NotifyOfPropertyChange(nameof(IsBottomLevel));
            }
            else
            {
                this.IsBottomLevel = false;
                GroupData child = _internalItems.FirstOrDefault(i => (i is GroupData) && ((GroupData)i).Header.Equals(headers.First())) as GroupData;
                if (child != null)
                    child.AddItemToGroup(item, ++level, headers.Skip(1));
                else
                {
                    GroupData newGroup = new GroupData(headers.First());
                    newGroup.AddItemToGroup(item, ++level, headers.Skip(1));
                    this._internalItems.Add(newGroup);
                }
            }
        }

        internal void Move(int oldIndex, int newIndex)
        {
            _internalItems.Move(oldIndex, newIndex);
        }
        /// <summary>
        /// This method only is called when this groupData has no header and items in the parent collectionView are not grouped or sorted.
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
        /// Returns the index that item should be placed in the collection based on the sorting criterias 
        /// </summary>
        private static int GetInsertIndex(IList<object> items, object newItem, Comparison<object> comparison)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (comparison(newItem, items[i]) <= 0)
                    return i;
            }
            return items.Count;
        }
        /// <summary>
        /// This method only is called when this groupData has no header and items in the parent collectionView are not grouped.
        /// </summary>
        internal void RemoveRangeAt(int oldStartingIndex, int count)
        {
            this._internalItems.RemoveRangeAt(oldStartingIndex, count);
            NotifyOfPropertyChange(nameof(Count));
            NotifyOfPropertyChange(nameof(ItemCount));
        }
        /// <summary>
        /// This method only is called when this groupData has no header and items in the parent collectionView are not grouped.
        /// </summary>
        internal void ReplaceAt(int index, object newValue)
        {
            this._internalItems[index] = newValue;
        }

        public IEnumerator<object> GetEnumerator()
        {
            if (_header != null) yield return _header;
            foreach (var item in _internalItems)
            {
                if (item is GroupData)
                {
                    foreach (var child in (GroupData)item)
                    {
                        yield return child;
                    }
                }
                else
                    yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void SortItems(Comparison<object> comparison)
        {
            if (IsBottomLevel)
            {
                List<object> sorted = new List<object>(_internalItems);
                sorted.Sort(comparison);
                _internalItems.ReplaceRange(sorted);
            }
            else
                foreach (GroupData group in _internalItems)
                {
                    group.SortItems(comparison);
                }
        }

        internal int IndexOf(object value)
        {
            int index = -1;
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
            GroupDescription currentGroup = description[_level];
            IComparer<object> comparer = currentGroup.GroupHeaderComparer;
            if (comparer == null)
                return;

            var sorted = _internalItems.OrderBy(g => ((GroupData)g).Header, comparer).ToList();//check why to list is needed
            _internalItems.ReplaceRange(sorted);
            foreach (GroupData group in _internalItems)
            {
                group.SortHeaders(description);
            }
        }

        /// <summary>
        /// Notifies subscribers of the property change.
        /// </summary>
        /// <param name = "propertyName">Name of the property.</param>
        protected virtual void NotifyOfPropertyChange([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
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