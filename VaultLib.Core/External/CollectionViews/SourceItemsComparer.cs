using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace VaultLib.Core.External.CollectionViews
{
    internal class SourceItemsComparer : IComparer<object>
    {
        private readonly Collection<SortDescription> _sortDescriptions;

        public SourceItemsComparer(Collection<SortDescription> sortDescriptions)
        {
            _sortDescriptions = sortDescriptions;
        }

        public int Compare(object object1, object object2)
        {
            var result = 0;
            var lvl = 0;
            if (_sortDescriptions.Count == 0)
                return result;
            while (lvl < _sortDescriptions.Count)
            {
                var currentSort = _sortDescriptions[lvl];
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

                    var x = (IComparable) val1; //items must be IComparable
                    result = x.CompareTo(val2) * greater;
                    if (result != 0)
                        return result;
                    lvl++;
                }
                else
                {
                    var x = (IComparable) object1; //items must be IComparable
                    result = x.CompareTo(object2) * greater;
                    if (result != 0)
                        return result;
                    lvl++;
                }
            }

            return result;
        }
    }
}