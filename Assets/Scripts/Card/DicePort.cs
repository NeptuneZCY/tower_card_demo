using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicePort
{
    public DiceType type;
    public Property prop;
    public int affect;

    public DicePort(DiceType type, Property prop, int affect)
    {
        this.type = type;
        this.prop = prop;
        this.affect = affect;
    }

    public static int GetAffectByStr(string str)
    {
        return str == "++" ? 2 :
            str == "+" ? 1 :
            str == "-" ? -1 :
            str == "--" ? -2 : 0;
    }

    public override string ToString()
    {
        return "DicePort[portType: " + type.ToString() + ", prop: " + prop.ToString() + ", affect: " + affect + "]";
    }
}

public enum PortType
{
    NONE = 0,
    TRIANGLE = 1 << 0,
    SQUARE = 1 << 1
}

