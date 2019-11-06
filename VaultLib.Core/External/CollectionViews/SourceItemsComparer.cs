using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace VaultLib.Core.External.CollectionViews
{
    class SourceItemsComparer : IComparer<object>
    {
        Collection<SortDescription> _sortDescriptions;
        public SourceItemsComparer(Collection<SortDescription> sortDescriptions)
        {
            _sortDescriptions = sortDescriptions;
        }
        public int Compare(object object1, object object2)
        {
            int result = 0;
            int lvl = 0;
            if (_sortDescriptions.Count == 0)
                return result;
            while (lvl < _sortDescriptions.Count)
            {
                SortDescription currentSort = _sortDescriptions[lvl];
                int smaller = currentSort.Direction == ListSortDirection.Ascending ? -1 : 1;
                int greater = -smaller;
                if (object1 == null && object2 == null)
                {
                    lvl++;
                    continue;//this sort is match, continue to the next sort criteria
                }
                else if (object1 == null)
                    return smaller;
                else if (object2 == null)
                    return greater;

                if (currentSort.PropertyValueProvider != null)
                {
                    IComparable x = currentSort.PropertyValueProvider(object1);
                    IComparable y = currentSort.PropertyValueProvider(object2);
                    if (x == null && y == null)
                    {
                        lvl++;
                        continue;//this sort is match, continue to the next sort criteria
                    }
                    else if (x == null)
                        return smaller;
                    else if (y == null)
                        return greater;
                    result = x.CompareTo(y) * greater;
                    if (result != 0)
                        return result;
                    lvl++;
                }
                else if (!string.IsNullOrEmpty(currentSort.PropertyName))
                {
                    object val1 = object1.GetType().GetRuntimeProperty(currentSort.PropertyName)?.GetValue(object1);
                    object val2 = object2.GetType().GetRuntimeProperty(currentSort.PropertyName)?.GetValue(object2);
                    if (val1 == null && val2 == null)
                    {
                        lvl++;
                        continue;//this sort is match, continue to the next sort criteria
                    }
                    else if (val1 == null)
                        return smaller;
                    else if (val2 == null)
                        return greater;
                    IComparable x = (IComparable)val1;//items must be IComparable
                    result = x.CompareTo(val2) * greater;
                    if (result != 0)
                        return result;
                    lvl++;
                }
                else
                {
                    IComparable x = (IComparable)object1;//items must be IComparable
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