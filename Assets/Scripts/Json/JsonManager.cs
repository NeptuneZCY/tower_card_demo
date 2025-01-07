using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonManager
{
    public static readonly JsonManager Instance = new JsonManager();
    public List<Card> cardList = new List<Card>();
    public List<EventCardObj> eventCardObjs;

    public void Init()
    {
        //List<DicePort> dicePortList = new List<DicePort>();
        //dicePortList.Add(new DicePort(PortType.TRIANGLE | PortType
        //    .TRIANGLE, Property.SPIRIT, 2));
        //dicePortList.Add(new DicePort(PortType.TRIANGLE | PortType
        //    .TRIANGLE, Property.THOUGHT, -1));
        //dicePortList.Add(new DicePort(PortType.TRIANGLE | PortType
        //    .TRIANGLE, Property.LOGIC, -1));

        //cardList.Add(new Card("һ����ֵļ��г�����ҹ��ÿ���̷���û����ס���������κζ����������������һ�μ��䡣", dicePortList));

        LoadCardsFromJSON();
    }

    public static string GetPropStr(Property prop)
    {
        switch (prop)
        {
            case Property.STRENTH:
                return "����";
            case Property.AGILITY:
                return "����";
            case Property.SPIRIT:
                return "����";
            case Property.LOGIC:
                return "�߼�";
            case Property.THOUGHT:
                return "˼��";
            case Property.COURAGE:
                return "����";
            default:
                return "����";
        }
    }

    void LoadCardsFromJSON()
    {
        // �� Resources �ļ����м��� JSON �ļ�
        TextAsset jsonFile = Resources.Load<TextAsset>("Json/����");

        if (jsonFile != null)
        {
            // �� JSON �ַ���ת��Ϊ CardList ����
            eventCardObjs = ListFromJson<EventCardObj>(jsonFile.ToString());
            //Debug.Log("Loaded " + eventCardObjs.Count + " cards.");

            // ������п�Ƭ����Ϣ
            //foreach (EventCardObj card in eventCardObjs)
            //{
            //    Debug.Log("Card " + card.��� + ": " + card.��������);
            //}
        }
        else
        {
            Debug.LogError("Failed to load JSON file.");
        }
    }

    public static string ListToJson<T>(List<T> l)
    {
        return JsonUtility.ToJson(new Serialization<T>(l));
    }

    public static List<T> ListFromJson<T>(string str)
    {
        return JsonUtility.FromJson<Serialization<T>>(str).ToList();
    }

    public static string DicToJson<TKey, TValue>(Dictionary<TKey, TValue> dic)
    {
        return JsonUtility.ToJson(new Serialization<TKey, TValue>(dic));
    }

    public static Dictionary<TKey, TValue> DicFromJson<TKey, TValue>(string str)
    {
        return JsonUtility.FromJson<Serialization<TKey, TValue>>(str).ToDictionary();
    }
}

public enum Property
{
    STRENTH, // ����
    AGILITY, // ����
    SPIRIT, // ����
    LOGIC, // �߼�
    THOUGHT, // ˼��
    COURAGE, // ����
}