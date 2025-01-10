using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using NPOI.SS.Formula.Functions;
using System;


public class EventCard : Card
{
    public readonly static string CHILD_CONTENT = "Content";
    public readonly static string PORT_CONTAINER = "PortContainer";
    public string content;
    public List<DicePort> dicePortList = new List<DicePort>();

    public EventCard(string content, List<DicePort> list)
    {
        this.content = content;
        dicePortList.AddRange(list);
    }

    public static List<EventCard> GetCardsByJsonDatas(List<EventCardData> eventCardDatas)
    {
        List<EventCard> resultList = new List<EventCard>();
        foreach (EventCardData data in eventCardDatas)
        {
            if (data == null) continue;
            List<DicePort> portList = new List<DicePort>();
            if (!String.IsNullOrEmpty(data.key1_shape) && !String.IsNullOrEmpty(data.key1_attri) && data.key1_affect != 0)
            {
                Property prop1 = JsonManager.StringToProperty(data.key1_attri);
                portList.Add(new DicePort(JsonManager.StringToDiceType(data.key1_shape), prop1, data.key1_affect));
            }
            if (!String.IsNullOrEmpty(data.key2_shape) && !String.IsNullOrEmpty(data.key2_attri) && data.key2_affect != 0)
            {
                Property prop2 = JsonManager.StringToProperty(data.key2_attri);
                portList.Add(new DicePort(JsonManager.StringToDiceType(data.key2_shape), prop2, data.key2_affect));
            }
            if (!String.IsNullOrEmpty(data.key3_shape) && !String.IsNullOrEmpty(data.key3_attri) && data.key3_affect != 0)
            {
                Property prop3 = JsonManager.StringToProperty(data.key3_attri);
                portList.Add(new DicePort(JsonManager.StringToDiceType(data.key3_shape), prop3, data.key3_affect));
            }

            Debug.Log($"portList: {portList.Count}");
            resultList.Add(new EventCard(data.description, portList));
        }
        return resultList;
    }

    //public static List<EventCard> GetCardsByJsonObjs(List<EventCardObj> eventCardObjs)
    //{
    //    List<EventCard> resultList = new List<EventCard> ();
    //    foreach (EventCardObj obj in eventCardObjs)
    //    {
    //        if (obj == null) continue;
    //        if (string.IsNullOrEmpty(obj.序号)) continue;
    //        List<string> portTypeStrList = obj.放入形状.Split("/").ToList();
    //        List<string> propList = new List<string>();
    //        propList.Add(obj.属性影响1);
    //        propList.Add(obj.属性影响2);
    //        propList.Add(obj.属性影响3);
    //        propList.Add(obj.属性影响4);
    //        propList.Add(obj.属性影响5);
    //        propList.Add(obj.属性影响6); 
    //        List<DicePort> portList = new List<DicePort>();
    //        foreach (string propStr in propList)
    //        {
    //            if (propStr != null && propStr.Length >= 3)
    //            {
    //                List<PortType> portTypeList = EnumUtils.ListStringToEnum<PortType>(portTypeStrList);
    //                PortType portType = PortType.NONE;
    //                foreach (PortType type in portTypeList)
    //                {
    //                    portType |= type;
    //                }
    //                Property prop = EnumUtils.StringToEnum<Property>(propStr.Substring(0, 2));
    //                string affectStr = propStr.Substring(2);
    //                DicePort port = new DicePort(portType, prop, DicePort.GetAffectByStr(affectStr));
    //                portList.Add(port);
    //            }
    //        }
    //        resultList.Add (new EventCard(obj.描述内容, portList));
    //    }
    //    return resultList;
    //}

    public override string ToString()
    {
        string str = "EventCard[content: " + content + ", ";
        foreach (DicePort port in dicePortList) {
            str += "[dicePort: ";
            str += port.ToString();
            str += "], ";
        }
        str += "]";
        return str;
    }
}
