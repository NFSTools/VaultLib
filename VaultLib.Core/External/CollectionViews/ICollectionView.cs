using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace VaultLib.Core.External.CollectionViews
{
    public interface ICollectionView : IList, INotifyCollectionChanged, INotifyPropertyChanged
    {
        //
        // Summary:
        //     Gets a value that indicates whether this view supports grouping via the ICollectionView.GroupDescriptions
        //     property.
        //
        // Returns:
        //     true if this view supports grouping; otherwise, false.
        bool CanGroup { get; }

        //
        // Summary:
        //     Gets the top-level groups.
        //
        // Returns:
        //     A read-only collection of the top-level groups.
        ReadOnlyObservableCollection<IGroupData> Groups { get; }

        //
        // Summary:
        //     Gets or sets a callback used to determine if an item is suitable for inclusion
        //     in the view.
        //
        // Returns:
        //     A method used to determine if an item is suitable for inclusion in the view.
        Predicate<object> Filter { get; set; }

        //
        // Summary:
        //     Gets a value that indicates whether this view supports sorting via the SortDescriptions
        //     property.
        //
        // Returns:
        //     true if this view supports sorting; otherwise, false.
        bool CanSort { get; }

        //
        // Summary:
        //     Gets a collection of SortDescription objects that describe
        //     how the items in the collection are sorted in the view.
        //
        // Returns:
        //     A collection of SortDescription objects that describe how
        //     the items in the collection are sorted in the view.
        FastObservableCollection<SortDescription> SortDescriptions { get; }

        //
        // Summary:
        //     Gets a collection of GroupDescription objects that describe
        //     how the items in the collection are grouped in the view.
        //
        // Returns:
        //     A collection of GroupDescription objects that describe
        //     how the items in the collection are grouped in the view.
        FastObservableCollection<GroupDescription> GroupDescriptions { get; }

        //
        // Summary:
        //     Recreates the view.
        void Refresh();
    }
}