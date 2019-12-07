using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using MvvmCross.Converters;

namespace VaultLib.Core.External.CollectionViews
{
    public class GroupDescription
    {
        //-----------------------------------------------------

        #region Construction

        static GroupDescription()
        {
            CompareHeaderAscending = new AscendingHeaderComparer();
            CompareHeaderDescending = new DescendingHeaderComparer();
        }

        public GroupDescription()
        {
            //this.GroupNameComparer = new AscendingNameComparer();
        }

        public GroupDescription(string propertyName)
        {
            PropertyName = propertyName;
        }

        public GroupDescription(string propertyName, IMvxValueConverter converter)
        {
            PropertyName = propertyName;
            Converter = converter;
        }

        #endregion

        //-----------------------------------------------------

        #region Properties & Methods

        public static IComparer<object> CompareHeaderAscending { get; }
        public static IComparer<object> CompareHeaderDescending { get; }

        /// <summary>
        ///     Gets or sets a converter to apply to the property value or the item to produce
        ///     The final value that is used to determine which group(s) an item belongs to.
        /// </summary>
        public IMvxValueConverter Converter { get; set; }

        /// <summary>
        ///     Gets or sets the name of the property that is used to determine which group(s) an item belongs to. The default
        ///     value is null.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        ///     Gets or sets the comparer that will be used to sort groups corresponding to this group description based on the
        ///     group headers. The default value is null.
        /// </summary>
        public IComparer<object> GroupHeaderComparer { get; set; }

        /// <summary>
        ///     Returns the group name(s) for the given item.
        /// </summary>
        /// <param name="item">The item to return group names for.</param>
        /// <param name="groupLevel">The level of grouping.</param>
        /// <param name="culture">The System.Globalization.CultureInfo to supply to the converter.</param>
        /// <returns>Returns the group name(s) for the given item.</returns>
        public object GroupNameFromItem(object item, int groupLevel, CultureInfo culture)
        {
            object value;

            // get the property value 
            if (string.IsNullOrEmpty(PropertyName))
            {
                value = item;
            }
            else if (item != null)
            {
                var itemType = item.GetType();
                value = itemType.GetRuntimeProperty(PropertyName)?.GetValue(item);
            }
            else
            {
                value = null;
            }

            // apply the converter to the value
            if (Converter != null) value = Converter.Convert(value, typeof(object), groupLevel, culture);

            return value;
        }

        #endregion

        //-----------------------------------------------------
    }

    internal class AscendingHeaderComparer : IComparer, IComparer<object>
    {
        public int Compare(object x, object y)
        {
            return x.ToString().CompareTo(y.ToString());
        }
    }

    internal class DescendingHeaderComparer : IComparer, IComparer<object>
    {
        public int Compare(object x, object y)
        {
            return y.ToString().CompareTo(x.ToString());
        }
    }
}