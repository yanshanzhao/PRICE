using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;


    public static class EnumHelper
    {

        /// <summary>
        /// 扩展方法：根据枚举值得到属性Description中的描述, 如果没有定义此属性则返回空串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static String ToEnumDescriptionString(this int value, Type enumType)
        {
            NameValueCollection nvc = GetNVCFromEnumValue(enumType);
            return nvc[value.ToString()];
    }
    
        public static String GetDescription(this Enum value)
    {
        Type type = value.GetType();
        FieldInfo item = type.GetField(value.ToString(), BindingFlags.Public | BindingFlags.Static);
        if (item == null) return null;
        var attribute = Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)) as DescriptionAttribute;
        if (attribute != null && !String.IsNullOrEmpty(attribute.Description)) return attribute.Description;
        return null;
    }
    /// <summary>
    /// 根据枚举类型得到其所有的 值 与 枚举定义Description属性 的集合
    /// </summary>
    /// <param name="enumType"></param>
    /// <returns></returns>
    public static NameValueCollection GetNVCFromEnumValue(Type enumType)
        {
            NameValueCollection nvc = new NameValueCollection();
            Type typeDescription = typeof(DescriptionAttribute);
            System.Reflection.FieldInfo[] fields = enumType.GetFields();
            string strText = string.Empty;
            string strValue = string.Empty;
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        DescriptionAttribute aa = (DescriptionAttribute)arr[0];
                        strText = aa.Description;
                    }
                    else
                    {
                        strText = "";
                    }
                    nvc.Add(strValue, strText);
                }
            }
            return nvc;
        }
    }
