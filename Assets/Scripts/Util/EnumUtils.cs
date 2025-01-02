using System;
using System.Collections;
using System.Collections.Generic;

public static class EnumUtils
{
    /// <summary>
    /// 将字符串转换为指定的枚举类型。
    /// </summary>
    /// <typeparam name="T">目标枚举类型</typeparam>
    /// <param name="value">字符串值</param>
    /// <returns>对应的枚举值</returns>
    /// <exception cref="ArgumentException">如果类型不是枚举类型抛出异常</exception>
    /// <exception cref="ArgumentNullException">如果输入字符串为 null 抛出异常</exception>
    /// <exception cref="ArgumentException">如果字符串无法匹配任何枚举值抛出异常</exception>
    public static T StringToEnum<T>(string value) where T : Enum
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value), "Input string cannot be null or empty.");
        }

        // 特殊逻辑处理
        if (typeof(T) == typeof(PortType))
        {
            return (T)(object)(
                value == "三角" ? PortType.TRIANGLE :
                value == "方形" ? PortType.SQUARE :
                throw new ArgumentException($"'{value}' is not a valid value for enum {typeof(T).Name}.")
            );
        }

        if (typeof(T) == typeof(Property))
        {
            return (T)(object)(
                value == "力量" ? Property.STRENTH :
                value == "敏捷" ? Property.AGILITY :
                value == "思虑" ? Property.THOUGHT :
                value == "勇气" ? Property.COURAGE :
                value == "灵性" ? Property.SPIRIT :
                value == "逻辑" ? Property.LOGIC :
                throw new ArgumentException($"'{value}' is not a valid value for enum {typeof(T).Name}.")
            );
        }

        // 通用处理
        if (!Enum.IsDefined(typeof(T), value))
        {
            throw new ArgumentException($"'{value}' is not a valid value for enum {typeof(T).Name}.");
        }

        return (T)Enum.Parse(typeof(T), value, true); // true 表示忽略大小写
    }

    public static List<T> ListStringToEnum<T>(List<string> strList) where T : Enum
    {
        if (strList == null) return null;
        List<T> enumList = new List<T>();

        foreach(string str in strList)
        {
            enumList.Add(StringToEnum<T>(str));
        }
        return enumList;
    }
}
