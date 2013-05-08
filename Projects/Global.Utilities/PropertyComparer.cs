using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;

namespace Global.Utilities
{
    /// <summary>
    /// REFACTOR: This class is referenced by all tiers even by Domain Model.
    /// These utilities should not have dependency on UI tier. 
    /// Fact that other layers dont need to know or depend on what UI is
    /// The dependency on System.Web should go away.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PropertyComparer<T> : IComparer<T>
    {
        private string _propertyName = "";
        private SortDirection _direction;

        /// <summary>
        /// Sort based on given parameters.
        /// </summary>
        /// <param name="propertyName">Property to be compared.</param>
        /// <param name="direction">Sort Direction (ASC or DESC).</param>
        public PropertyComparer(
            string propertyName
            , string direction)
        {
            _propertyName = propertyName;

            switch (direction.ToUpper())
            {
                case "ASC":
                    _direction = SortDirection.Ascending;
                    break;
                case "DESC":
                    _direction = SortDirection.Descending;
                    break;
                default:
                    _direction = SortDirection.Ascending;
                    break;
            }
        }

        /// <summary>
        /// Sort based on given parameters
        /// </summary>
        /// <param name="propertyName">Property to be compared.</param>
        /// <param name="direction">Sort Direction</param>
        public PropertyComparer(
            string propertyName
            , SortDirection direction)
        {
            _propertyName = propertyName;
            _direction = direction;
        }

        /// <summary>
        /// Sort based on given expression. 
        /// Expression could consists of property alone or
        /// combination of property and sort direction (ASC, DESC). 
        /// Eg. propertycomparer("myDate ASC")
        /// </summary>
        /// <param name="sortExpression">Expression for sorting. (PropertyName SortDirection)</param>
        public PropertyComparer(string sortExpression)
        {
            if (sortExpression == "" || sortExpression == null)
            {
                _propertyName = "";
                _direction = SortDirection.Ascending;
            }
            else if (sortExpression.ToLowerInvariant().EndsWith(" desc"))
            {
                _propertyName = sortExpression.Substring(0, sortExpression.Length - 5);
                _direction = SortDirection.Descending;

            }
            else if (sortExpression.ToLowerInvariant().EndsWith(" asc"))
            {
                _propertyName = sortExpression.Substring(0, sortExpression.Length - 4);

                _direction = SortDirection.Ascending;
            }
            else
            {
                _propertyName = sortExpression;
                _direction = SortDirection.Ascending;
            }
        }

        public int Compare(T x, T y)
        {
            if (_propertyName != null && _propertyName != "")
            {
                PropertyInfo propertyX = null;
                PropertyInfo propertyY = null;
                object px;
                object py;

                string[] propertyList = _propertyName.Split('.');

                object obj1 = x;
                object obj2 = y;

                for (int i = 0; i < propertyList.Length; i++)
                {
                    if (i == propertyList.Length - 1)
                    {
                        propertyX = obj1.GetType().GetProperty(propertyList[i]);
                        propertyY = obj2.GetType().GetProperty(propertyList[i]);
                    }
                    else
                    {
                        obj1 = obj1.GetType().GetProperty(propertyList[i]).GetGetMethod().Invoke(
                            obj1
                            , null);

                        obj2 = obj2.GetType().GetProperty(propertyList[i]).GetGetMethod().Invoke(
                            obj2
                            , null);
                    }
                }

                px = propertyX.GetValue(obj1, null);
                py = propertyY.GetValue(obj2, null);

                if ((px != null) && (py != null)
                    && (px.GetType().BaseType == typeof(System.Enum)))
                {
                    px = px.ToString();
                    py = py.ToString();
                }

                if (px != null)
                {
                    if ((px is int))
                    {
                        return Compare<int>((int)px, (int)py) * sortDirectionAsNumber(_direction);
                    }
                    if ((px is decimal))
                    {
                        return Compare<decimal>(
                            (decimal)px
                            , (decimal)py) * sortDirectionAsNumber(_direction);
                    }
                    if ((px is DateTime?))
                    {
                        if (px == null || py == null)
                        {
                            return 0;
                        }
                        return Compare<DateTime>(
                            (DateTime)px
                            , (DateTime)py) * sortDirectionAsNumber(_direction);
                    }
                    if ((px is double))
                    {
                        return Compare<double>(
                            (double)px
                            , (double)py) * sortDirectionAsNumber(_direction);
                    }
                    if ((px is string))
                    {
                        return Compare<string>(
                            (string)px
                            , (string)py) * sortDirectionAsNumber(_direction);
                    }
                    if ((px is decimal))
                    {
                        return Compare<decimal>(
                            (decimal)px
                            , (decimal)py) * sortDirectionAsNumber(_direction);
                    }
                    if ((px.GetType().IsEnum))
                    {
                        return Compare<Enum>((Enum)px, (Enum)py) * sortDirectionAsNumber(_direction);
                    }
                    MethodInfo methodX = propertyX.GetType().GetMethod("CompareTo");
                    if ((methodX == null == false))
                    {
                        return (int)methodX.Invoke(
                            px
                            , new object[] { py }) * sortDirectionAsNumber(_direction);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else if (py != null)
                {
                    if (_direction == SortDirection.Ascending)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private int Compare<K>(
            K x
            , K y) where K : IComparable
        {
            return x.CompareTo(y);
        }

        private int sortDirectionAsNumber(SortDirection direction)
        {
            if (direction == SortDirection.Descending)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}