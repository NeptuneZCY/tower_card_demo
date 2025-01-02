using System;
using System.Collections;
using System.Collections.Generic;

public static class EnumUtils
{
    /// <summary>
    /// ���ַ���ת��Ϊָ����ö�����͡�
    /// </summary>
    /// <typeparam name="T">Ŀ��ö������</typeparam>
    /// <param name="value">�ַ���ֵ</param>
    /// <returns>��Ӧ��ö��ֵ</returns>
    /// <exception cref="ArgumentException">������Ͳ���ö�������׳��쳣</exception>
    /// <exception cref="ArgumentNullException">��������ַ���Ϊ null �׳��쳣</exception>
    /// <exception cref="ArgumentException">����ַ����޷�ƥ���κ�ö��ֵ�׳��쳣</exception>
    public static T StringToEnum<T>(string value) where T : Enum
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value), "Input string cannot be null or empty.");
        }

        // �����߼�����
        if (typeof(T) == typeof(PortType))
        {
            return (T)(object)(
                value == "����" ? PortType.TRIANGLE :
                value == "����" ? PortType.SQUARE :
                throw new ArgumentException($"'{value}' is not a valid value for enum {typeof(T).Name}.")
            );
        }

        if (typeof(T) == typeof(Property))
        {
            return (T)(object)(
                value == "����" ? Property.STRENTH :
                value == "����" ? Property.AGILITY :
                value == "˼��" ? Property.THOUGHT :
                value == "����" ? Property.COURAGE :
                value == "����" ? Property.SPIRIT :
                value == "�߼�" ? Property.LOGIC :
                throw new ArgumentException($"'{value}' is not a valid value for enum {typeof(T).Name}.")
            );
        }

        // ͨ�ô���
        if (!Enum.IsDefined(typeof(T), value))
        {
            throw new ArgumentException($"'{value}' is not a valid value for enum {typeof(T).Name}.");
        }

        return (T)Enum.Parse(typeof(T), value, true); // true ��ʾ���Դ�Сд
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
