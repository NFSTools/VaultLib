using System.Collections.ObjectModel;
using System.ComponentModel;

namespace VaultLib.Core.External.CollectionViews
{
    public interface IGroupData : INotifyPropertyChanged
    {
        /// <summary>
        ///     Gets a value that indicates whether this group has any subgroups.
        ///     Returns: true if this group is at the bottom level and does not have any subgroups; otherwise, false.
        /// </summary>
        bool IsBottomLevel { get; }

        /// <summary>
        ///     Gets the level of this group in the grouping. The top group is at level '1' and level is increased by one for each
        ///     child group.
        /// </summary>
        int Level { get; }

        /// <summary>
        ///     Gets the total count off items in this group including the header and subtree items.
        /// </summary>
        int Count { get; }

        /// <summary>
        ///     Gets the number of items in 'Items' collection of this group.
        /// </summary>
        int ItemCount { get; }

        /// <summary>
        ///     Gets the header for this group. Header can be null if items are not grouped.
        /// </summary>
        object Header { get; }

        /// <summary>
        ///     Gets childs of this DataGroup. Childs are either nested DataGroups or grouped items at the buttom level.
        /// </summary>
        ReadOnlyObservableCollection<object> Items { get; }
    }
}