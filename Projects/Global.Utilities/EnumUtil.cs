using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Global.Utilities
{
    public static class EnumUtil
    {
        public static string GetDescription(Enum value)
        {
            Type enumType = value.GetType();

            MemberInfo[] memberInfo = enumType.GetMember(value.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                DescriptionAttribute[] descriptions = (DescriptionAttribute[])memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (descriptions != null && descriptions.Length > 0)
                {
                    return descriptions[0].Description;
                }
            }

            return value.ToString();
        }

        public static string[] GetDescriptions<T>()
        {
            Type enumType = typeof(T);

            Array enumValues = Enum.GetValues(enumType);
            
            IList<string> descriptions = new List<string>();

            foreach (var value in enumValues)
            {
                descriptions.Add(GetDescription((Enum)value));
            }

            return descriptions.ToArray();
        }

        public static T GetValue<T>(string description)
        {
            Type enumType = typeof(T);

            if(!enumType.IsEnum)
            {
                throw new Exception(string.Format("The object {0} is NOT an Enum type.", enumType.Name));
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new Exception(string.Format("The description to evaluate for Enum {0} is null or empty.", enumType.Name));
            }

            Array enumValues = Enum.GetValues(enumType);

            foreach (var value in enumValues)
            {
                if (GetDescription((Enum)value) == description)
                {
                    return (T)value;
                }
            }

            throw new Exception(string.Format("The text {0} is not a description of the enum {1}", description, enumType.Name));
        }
    }
}
