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

        //cardList.Add(new Card("一个奇怪的集市出现在夜晚，每个商贩都没有面孔。你可以买到任何东西，但代价是你的一段记忆。", dicePortList));

        LoadCardsFromJSON();
    }

    public static string GetPropStr(Property prop)
    {
        switch (prop)
        {
            case Property.STRENTH:
                return "力量";
            case Property.AGILITY:
                return "敏捷";
            case Property.SPIRIT:
                return "灵性";
            case Property.LOGIC:
                return "逻辑";
            case Property.THOUGHT:
                return "思虑";
            case Property.COURAGE:
                return "勇气";
            default:
                return "力量";
        }
    }

    void LoadCardsFromJSON()
    {
        // 从 Resources 文件夹中加载 JSON 文件
        TextAsset jsonFile = Resources.Load<TextAsset>("Json/卡牌");

        if (jsonFile != null)
        {
            // 将 JSON 字符串转换为 CardList 对象
            eventCardObjs = ListFromJson<EventCardObj>(jsonFile.ToString());
            //Debug.Log("Loaded " + eventCardObjs.Count + " cards.");

            // 输出所有卡片的信息
            //foreach (EventCardObj card in eventCardObjs)
            //{
            //    Debug.Log("Card " + card.序号 + ": " + card.描述内容);
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
    STRENTH, // 力量
    AGILITY, // 敏捷
    SPIRIT, // 灵性
    LOGIC, // 逻辑
    THOUGHT, // 思虑
    COURAGE, // 勇气
}