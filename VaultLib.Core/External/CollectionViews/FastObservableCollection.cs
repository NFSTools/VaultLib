using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace VaultLib.Core.External.CollectionViews
{
    //based on http://stackoverflow.com/questions/7687000/fast-performing-and-thread-safe-observable-collection
    /// <summary>
    ///     Represents a dynamic data collection that provides notifications when items get added, removed, or when the whole
    ///     list is refreshed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FastObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        ///     Initializes a new instance of the System.Collections.ObjectModel.ObservableCollection(Of T) class.
        /// </summary>
        public FastObservableCollection()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the System.Collections.ObjectModel.ObservableCollection(Of T) class that contains
        ///     elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">collection: The collection from which the elements are copied.</param>
        /// <exception cref="System.ArgumentNullException">The collection parameter cannot be null.</exception>
        public FastObservableCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        /// <summary>
        ///     Adds the elements of the specified collection to the end of the ObservableCollection(Of T).
        /// </summary>
        /// <param name="notificationMode">
        ///     Accepted notificationModes are: 'NotifyCollectionChangedAction.Add' and
        ///     'NotifyCollectionChangedAction.Reset'
        /// </param>
        public void AddRange(IEnumerable<T> collection,
            NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Add)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            CheckReentrancy();

            if (notificationMode == NotifyCollectionChangedAction.Reset)
            {
                foreach (var i in collection) Items.Add(i);

                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

                return;
            }

            var startIndex = Count;
            var changedItems = collection is List<T> ? (List<T>)collection : new List<T>(collection);
            foreach (var i in changedItems) Items.Add(i);

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems,
                startIndex));
        }

        /// <summary>
        ///     Inserts the elements of the specified collection to the specified index of the ObservableCollection(Of T).
        /// </summary>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (index < 0 || index > Items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            CheckReentrancy();
            var changedItems = collection is List<T> ? (List<T>)collection : new List<T>(collection);

            for (var i = changedItems.Count - 1; i >= 0; i--) Items.Insert(index, changedItems[i]);

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems, index));
        }

        /// <summary>
        ///     Removes the first occurence of each item in the specified collection from ObservableCollection(Of T).
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="notificationMode">
        ///     By diffault this method notifies handlers of the 'CollectionChanged' event of removed items without specifiying any
        ///     index.
        ///     If handlers expect the 'OldStartingIndex' property of event args being set, then
        ///     NotifyCollectionChangedAction.Reset should be used.
        /// </param>
        public void RemoveRange(IEnumerable<T> collection,
            NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Remove)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            CheckReentrancy();

            if (notificationMode == NotifyCollectionChangedAction.Reset)
            {
                foreach (var i in collection) Items.Remove(i);

                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

                return;
            }

            var removedItems = new List<T>();
            var collectionChanged = false;
            foreach (var i in collection)
                if (Items.Contains(i))
                {
                    Items.Remove(i);
                    removedItems.Add(i);
                    collectionChanged = true;
                }

            if (collectionChanged)
            {
                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems));
            }
        }

        /// <summary>
        ///     Removes multiple items from ObservableCollection(Of T), starting from the given index.
        /// </summary>
        public void RemoveRangeAt(int index, int count)
        {
            if (index < 0 || index >= Items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (Items.Count - index < count)
                throw new InvalidOperationException(
                    "The given count is greater than the available items in the ObservableCollection(Of T) starting from the specified index");
            CheckReentrancy();
            var removedItems = new List<T>();
            for (var i = 0; i < count; i++)
            {
                removedItems.Add(Items[index]);
                Items.RemoveAt(index);
            }

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems, index));
        }

        /// <summary>
        ///     Clears the current collection and replaces it with the specified item.
        /// </summary>
        public void Replace(T item)
        {
            ReplaceRange(new[] { item });
        }

        /// <summary>
        ///     Clears the current collection and replaces it with the specified collection.
        /// </summary>
        public void ReplaceRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            Items.Clear();
            AddRange(collection, NotifyCollectionChangedAction.Reset);
        }

        /// <summary>
        ///     This method disposes all disposable items from ObservableCollection(Of T) then clears the collection.
        /// </summary>
        public void DisposeItems()
        {
            foreach (var i in Items)
                (i as IDisposable)?.Dispose();
            Items.Clear();
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        /// <summary>
        ///     This method removes the given generic list of items from ObservableCollection(Of T) and dispose them if they are
        ///     IDisposable and contained in the collection.
        /// </summary>
        public void DisposeItems(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            foreach (var item in collection)
                if (Items.Contains(item))
                {
                    (item as IDisposable)?.Dispose();
                    Items.Remove(item);
                }

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}