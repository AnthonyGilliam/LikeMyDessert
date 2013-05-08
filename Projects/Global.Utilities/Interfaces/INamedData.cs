using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;


namespace Global.Utilities.Interfaces
{
    public interface INamedData : INamed
    {
        /// <summary>
        /// The Primary Key queried from the object's data table.
        /// </summary>
        Guid ID { get; }
    }

    public static class NamedData
    {
        public static Dictionary<Guid, T> BindObjectsToComboBox<T>(
            System.Windows.Forms.ComboBox boxToBind
            , IList<T> listToBind
            , bool sorted) where T : INamedData
        {
            if(sorted == true)
            {
                return BindObjectsToComboBox<T>(boxToBind
                    , listToBind
                    , "Name"
                    , true
                    , "Name");
            }
            else
            {
                return BindObjectsToComboBox<T>(boxToBind
                    , listToBind
                    , "Name");
            }
        }

        public static Dictionary<Guid, T> BindObjectsToComboBox<T>(
            System.Windows.Forms.ComboBox boxToBind
            , IList<T> listToBind
            , string propertyToBind) where T : INamedData
        {
            return BindObjectsToComboBox<T>(boxToBind
                , listToBind
                , propertyToBind
                , false
                , null);
        }

        public static Dictionary<Guid, T> BindObjectsToComboBox<T>(
            System.Windows.Forms.ComboBox boxToBind
            , IList<T> listToBind
            , string propertyToBind
            , bool sorted
            , string sortProperty) where T : INamedData
        {
            boxToBind.DisplayMember = propertyToBind;
            boxToBind.ValueMember = "ID";
            
            if (sorted == true)
            {
                boxToBind.DisplayMember = propertyToBind;
                boxToBind.ValueMember = "ID";

                IOrderedEnumerable<T> sortedObjects = listToBind.OrderBy(
                    o => Reflector.GetProperty<string>(o, propertyToBind));
                
                IList<T> sortedList = new List<T>();
                foreach (T obj in sortedObjects)
                {
                    sortedList.Add(obj);
                }
                boxToBind.DataSource = sortedList;
            }
            else
            {
                boxToBind.DataSource = listToBind;
            }

            Dictionary<Guid, T> objectDictonary = listToBind.ToDictionary<T, Guid>(o => o.ID);
            return objectDictonary;
        }
    }
}
