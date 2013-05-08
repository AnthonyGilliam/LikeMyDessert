using System;
using System.Collections.Generic;
using System.Reflection;

namespace Global.Utilities
{
    /// <summary>
    /// Class that uses reflection to get around some of the restrictions
    /// placed on objects using access modifiers. Be very careful when
    /// using this class to violate access restrictions. Access modifiers
    /// are in place for a reason. Also consider whether the access modifier
    /// needs to be changed on the object rather than violating it.
    /// </summary>
    public static class Reflector
    {
        /// <summary>
        /// Method for creating an instance of an object using its private constructor.
        /// The private constructor must not require any parameters.
        /// </summary>
        /// <typeparam name="T">The type to be instantiated.</typeparam>
        /// <returns>An instance of the specified type.</returns>
        public static T CreateEmptyPrivateConstructorDomainObject<T>()
        {
            Type t = typeof(T);
            Type[] ctorParams = new Type[0];
            ConstructorInfo info = t.GetConstructor(BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, ctorParams, null);
            return (T)info.Invoke(null);
        }

        /// <summary>
        /// Method for setting the value of a member variable. Typically, this method
        /// would be used to set the value of a member variable that does not have
        /// a corresponding property or whose property is read-only.
        /// </summary>
        /// <param name="obj">The object whose member variable will be set.</param>
        /// <param name="fieldName">The name of the member variable to 
        /// assign the new value to.</param>
        /// <param name="data">The value to assign to the member variable.</param>
        public static void SetField(object obj, string fieldName, object data)
        {
            if (obj != null)
            {
                Type t = obj.GetType();
                System.Reflection.FieldInfo f = null;

                // Loop here to check base types as well.
                // Perhaps the caller is attempting to set a value in an inherited type.
                do
                {
                    f = t.GetField(fieldName
                        , BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                    t = t.BaseType;
                } while (null == f && t != typeof(object));

                if (null != f)
                {
                    f.SetValue(obj, data);
                }
                else
                {
                    throw new ApplicationException("Could not find the field specified for object.");
                }
            }
        }

        /// <summary>
        /// Method for retrieving the value of a member variable. If the member
        /// variable does not have a corresponding property, this method can
        /// be used to retrieve the value of a variable that is not ordiarily
        /// accessible.
        /// </summary>
        /// <typeparam name="T">The type of the object whose member
        /// variable is being retrieved.</typeparam>
        /// <param name="o">The instance that contains the desired member variable.</param>
        /// <param name="fieldName">The name of the member variable to read.</param>
        /// <returns>The value of the member variable.</returns>
        public static T GetPrivateFieldFromObject<T>(object o, string fieldName)
        {
            Type t = o.GetType();
            System.Reflection.FieldInfo f = null;

            // Loop here to check base types as well.
            // Perhaps the caller is attempting to set a value in an inherited type.
            do
            {
                f = t.GetField(fieldName
                    , BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                t = t.BaseType;
            } while (null == f && t != typeof(object));

            if (null != f)
            {
                return (T)f.GetValue(o);
            }
            else
            {
                throw new ApplicationException("Could not find the field specified for object.");
            }
        }

        /// <summary>
        /// Helper method for getting a field in an object that may not
        /// be ordinarily accessible due to access modifiers.
        /// </summary>
        /// <param name="o">The object whose member variable should be retrieved.</param>
        /// <param name="fieldName">The name of the field to be retrieved.</param>
        /// <returns>The field's value.</returns>
        public static object GetFieldForAnonymousObject(object o, string fieldName)
        {
            Type t = o.GetType();
            System.Reflection.FieldInfo f = null;

            // Loop here to check base types as well.
            // Perhaps the caller is attempting to set a value in an inherited type.
            do
            {
                f = t.GetField(fieldName
                    , BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                t = t.BaseType;
            } while (null == f && t != typeof(object));

            if (null != f)
            {
                return f.GetValue(o);
            }
            else
            {
                throw new ApplicationException("Could not find the field specified for object.");
            }
        }

        /// <summary>
        /// Permits the caller to invoke a private method in a class.
        /// The caller should specify the type that is expected to be returned.
        /// The generic should match the return type of the method.
        /// </summary>
        /// <typeparam name="T">The return type of the method.</typeparam>
        /// <param name="o">The object whose private method will be called.</param>
        /// <param name="name">The method name.</param>
        /// <param name="args">Parameters to be passed to the method.</param>
        /// <returns>The object that was returned from the method.</returns>
        public static T InvokePrivateMethod<T>(object o, string name, params object[] args)
        {
            Type t = o.GetType();
            Type[] types = null == args ? new Type[0] : new Type[args.Length];
            for (int i = 0; i < types.Length; i++)
            {
                types[i] = args[i].GetType();
            }

            MethodInfo method = null;
            // Loop here to check base types as well.
            // Perhaps the caller is attempting to set a value in an inherited type.
            do
            {
                // Allows for public as well. No reason to fail on a method that should be availabel anyway.
                method = t.GetMethod(name
                    , BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static
                    , null
                    , types
                    , null);
                t = t.BaseType;
            } while (null == method && t != null);

            return (T)method.Invoke(o, args);
        }

        /// <summary>
        /// Permits the caller to set a public or private property in a class.
        /// </summary>
        /// <param name="o">The object whose public or private property will be set.</param>
        /// <param name="name">The property name.</param>
        /// <param name="newValue">The new value to set the property to.</param>
        public static void SetProperty(
            object o
            , string name
            , object newValue)
        {
            Type t = o.GetType();

            PropertyInfo property = null;
            // Loop here to check base types as well.
            // Perhaps the caller is attempting to set a value in an inherited type.
            do
            {
                // Allows for public as well. No reason to fail on a method that should be availabel anyway.
                property = t.GetProperty(name
                    , BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                t = t.BaseType;
            } while (null == property && t != null);

            property.SetValue(
                o
                , newValue
                , null);
        }
        /// <summary>
        /// Permits the caller to get a public or private property in a class.
        /// </summary>
        /// <typeparam name="T">The Type of the property to be returned from this method</typeparam>
        /// <param name="o">The object whose public or private property will be retrieved.</param>
        /// <param name="name">The property name.</param>
        /// <returns>Typed object having parameter specified name</returns>
        public static T GetProperty<T>(
            object o
            , string name)
        {
            Type t = o.GetType();

            PropertyInfo property = null;
            // Loop here to check base types as well.
            // Perhaps the caller is attempting to set a value in an inherited type.
            do
            {
                // Allows for public as well. No reason to fail on a method that should be availabel anyway.
                property = t.GetProperty(name
                    , BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                t = t.BaseType;
            } while (null == property && t != null);

            return (T)property.GetValue(
                o
                , null);
        }

        /// <summary>
        /// Returns an object from collection by ID.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetObjectByID<T>(IList<T> objects, Guid id)
        {
            Type objType = typeof(T);
            PropertyInfo propertyInfo = null;
            object returnObj = null;
            
            foreach (T obj in objects)
            {
                propertyInfo = objType.GetProperty("ID"
                    , BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

                if ((propertyInfo != null) && (propertyInfo.GetValue(obj, null) != null))
                {
                    if (id == new Guid(propertyInfo.GetValue(obj, null).ToString()))
                    {
                        returnObj = obj;
                        break;
                    }
                }
            }

            return returnObj == null ? default(T) : (T)returnObj;
        }

        /// <summary>
        /// Creates and returns a list of given type with a single given item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static List<T> ConvertToList<T>(T item)
        {
            List<T> list = new List<T>();
            if (item != null)
            {
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// Copies the values of one object's fields to the fields of another 
        /// object where the fields have the same name.  Be aware that you may not get
        /// what you expect if fields with the same meaning have different names or 
        /// if fields with the same name have different meanings.
        /// Exceptions are not caught for copying fields with incompatible types.
        /// Use with caution!
        /// </summary>
        /// <typeparam name="TDestType">Type of the Destination object</typeparam>
        /// <param name="source">Source object for copy operation</param>
        /// <param name="dest">Reference to Destinaton object for copy operation</param>
        public static void CopyAllFields<TDestType>(object source, ref TDestType dest)
        {
            Type sType = source.GetType();
            Type dType = typeof(TDestType);

            BindingFlags allFields = BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

            foreach(FieldInfo fi in sType.GetFields(allFields))
            {
                FieldInfo dFi = dType.GetField(fi.Name, allFields);
                
                if(dFi!=null)
                {
                    dFi.SetValue(dest, fi.GetValue(source));
                }
            }
        }
    }
}