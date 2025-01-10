using LitJson;
using NPOI.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JsonManager
{
    public static readonly JsonManager Instance = new JsonManager();
    public List<Card> cardList = new List<Card>();
    public List<EventCardObj> eventCardObjs;

    public List<EventCardData> eventCardDatas;
    public List<RoleData> roleDatas;
    public List<DiceData> diceDatas;
    public List<AttributeData> attributeDatas;
    private const string EVENT_CARD_JSON_PATH = "Json_config/dice_card_config";
    private const string ROLE_JSON_PATH = "Json_config/role_config";
    private const string DICE_JSON_PATH = "Json_config/dice_key_shape";
    private const string ATTRIBUTE_PATH = "Json_config/attribute";

    public void Init()
    {
        //LoadCardsFromJSON();
        LoadEventCards();
        LoadAttributes();
        LoadDices();
        LoadRoles();
    }

    public static Property StringToProperty(string propStr)
    {
        List<int> intList = StringToIntList(propStr);
        Property prop = Property.NONE;
        intList.ForEach(i => prop |= Int2Prop(i));
        return prop;
    }

    public static DiceType StringToDiceType(string diceStr)
    {
        List<int> intList = StringToIntList(diceStr);
        DiceType type = DiceType.NONE;
        intList.ForEach(i => type |= Dice.GetDiceType(i));
        return type;
    }

    public static List<int> StringToIntList(string str)
    {
        List<string> stringList = str.Split(";").ToList();
        List<int> intList = new List<int>();
        foreach (string s in stringList)
        {
            if (int.TryParse(s, out int number))
            {
                intList.Add(number); // 转换成功，添加到列表
            }
            else
            {
                Debug.LogError($"stringToIntList无法转换的值: {s}");
            }
        }
        return intList;
    }

    public static Property Int2Prop(int index)
    {
        if (index < 0 || index > 5)
        {
            throw new ArgumentException("属性序号不合法！");
        }
        string name = Instance.attributeDatas[index].name;
        return EnumUtils.StringToEnum<Property>(name);
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

    public string GetAttributeByName(string name)
    {
        string attribute = null;
        foreach (var item in attributeDatas)
        {
            if (item.name == name)
            {
                attribute = item.attribute_name;
                break;
            }
        }
        return attribute;
    }

    public string GetNameByAttribute(string attribute)
    {
        string name = null;
        foreach(var item in attributeDatas)
        {
            if (item.attribute_name == attribute)
            {
                name = item.attribute_name;
                break;
            }
        }
        return name;
    }

    void LoadEventCards()
    {
        // 从 Resources 文件夹中加载 JSON 文件
        TextAsset jsonFile = Resources.Load<TextAsset>(EVENT_CARD_JSON_PATH);
        if (jsonFile != null)
        {
            // 将 JSON 字符串转换为 CardList 对象
            eventCardDatas = JsonMapper.ToObject<List<EventCardData>>(jsonFile.ToString());
        }
    }

    void LoadRoles()
    {
        // 从 Resources 文件夹中加载 JSON 文件
        TextAsset jsonFile = Resources.Load<TextAsset>(ROLE_JSON_PATH);
        if (jsonFile != null)
        {
            roleDatas = JsonMapper.ToObject<List<RoleData>>(jsonFile.ToString());
        }
    }

    void LoadDices()
    {
        // 从 Resources 文件夹中加载 JSON 文件
        TextAsset jsonFile = Resources.Load<TextAsset>(DICE_JSON_PATH);
        if (jsonFile != null)
        {
            diceDatas = JsonMapper.ToObject<List<DiceData>>(jsonFile.ToString());
        }
    }

    void LoadAttributes()
    {
        // 从 Resources 文件夹中加载 JSON 文件
        TextAsset jsonFile = Resources.Load<TextAsset>(ATTRIBUTE_PATH);
        if (jsonFile != null)
        {
            attributeDatas = JsonMapper.ToObject<List<AttributeData>>(jsonFile.ToString());
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
    NONE = 0,
    SPIRIT = 1 << 0, // 灵性
    LOGIC = 1 << 1, // 逻辑
    AGILITY = 1 << 2, // 敏捷
    THOUGHT = 1 << 3, // 思虑
    COURAGE = 1 << 4, // 勇气
    STRENTH = 1 << 5, // 力量
}