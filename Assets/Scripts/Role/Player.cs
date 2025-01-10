using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("灵性")]
    public int attribute_1;
    public int attribute_1_lowlimit;
    public int attribute_1_highlimit;
    [Header("逻辑")]
    public int attribute_2;
    public int attribute_2_lowlimit;
    public int attribute_2_highlimit;
    [Header("敏捷")]
    public int attribute_3;
    public int attribute_3_lowlimit;
    public int attribute_3_highlimit;
    [Header("思虑")]
    public int attribute_4;
    public int attribute_4_lowlimit;
    public int attribute_4_highlimit;
    [Header("勇气")]
    public int attribute_5;
    public int attribute_5_lowlimit;
    public int attribute_5_highlimit;
    [Header("力量")]
    public int attribute_6;
    public int attribute_6_lowlimit;
    public int attribute_6_highlimit;

    public Dictionary<Property, int> GetPropertyMap()
    {
        var dict = new Dictionary<Property, int>();
        dict.Add(Property.SPIRIT, attribute_1);
        dict.Add(Property.LOGIC, attribute_2);
        dict.Add(Property.AGILITY, attribute_3);
        dict.Add(Property.THOUGHT, attribute_4);
        dict.Add(Property.COURAGE, attribute_5);
        dict.Add(Property.STRENTH, attribute_6);
        return dict;
    }

    public Dictionary<Property, float> GetPropertyRatioMap()
    {
        var dict = new Dictionary<Property, float>();
        dict.Add(Property.SPIRIT, (float)attribute_1 / (attribute_1_highlimit - attribute_1_lowlimit));
        dict.Add(Property.LOGIC, (float)attribute_2 / (attribute_2_highlimit - attribute_2_lowlimit));
        dict.Add(Property.AGILITY, (float)attribute_3 / (attribute_3_highlimit - attribute_3_lowlimit));
        dict.Add(Property.THOUGHT, (float)attribute_4 / (attribute_4_highlimit - attribute_4_lowlimit));
        dict.Add(Property.COURAGE, (float)attribute_5 / (attribute_5_highlimit - attribute_5_lowlimit));
        dict.Add(Property.STRENTH, (float)attribute_6 / (attribute_6_highlimit - attribute_6_lowlimit));
        return dict;
    }

    public Dictionary<Property, float> GetPropertyRatioDictByAffectDict(Dictionary<Property, int> valueDict)
    {
        var dict = new Dictionary<Property, float>();
        dict.Add(Property.SPIRIT, (float)(attribute_1 + valueDict.GetValueOrDefault(Property.SPIRIT, 0)) / (attribute_1_highlimit - attribute_1_lowlimit));
        dict.Add(Property.LOGIC, (float)(attribute_2 + valueDict.GetValueOrDefault(Property.LOGIC, 0)) / (attribute_2_highlimit - attribute_2_lowlimit));
        dict.Add(Property.AGILITY, (float)(attribute_3 + valueDict.GetValueOrDefault(Property.AGILITY, 0)) / (attribute_3_highlimit - attribute_3_lowlimit));
        dict.Add(Property.THOUGHT, (float)(attribute_4 + valueDict.GetValueOrDefault(Property.THOUGHT, 0)) / (attribute_4_highlimit - attribute_4_lowlimit));
        dict.Add(Property.COURAGE, (float)(attribute_5 + valueDict.GetValueOrDefault(Property.COURAGE, 0)) / (attribute_5_highlimit - attribute_5_lowlimit));
        dict.Add(Property.STRENTH, (float)(attribute_6 + valueDict.GetValueOrDefault(Property.STRENTH, 0)) / (attribute_6_highlimit - attribute_6_lowlimit));
        return dict;
    }

    public Ending JudgeEnding()
    {
        Ending ending = new Ending();
        if (attribute_1 >= attribute_1_highlimit)
        {
            ending.prop = Property.SPIRIT;
            ending.isOverHigh = true;
        }
        else if (attribute_1 <= attribute_1_lowlimit)
        {
            ending.prop = Property.SPIRIT;
            ending.isOverHigh = false;
        }
        else if (attribute_2 >= attribute_2_highlimit)
        {
            ending.prop = Property.LOGIC;
            ending.isOverHigh = true;
        }
        else if (attribute_2 <= attribute_2_lowlimit)
        {
            ending.prop = Property.LOGIC;
            ending.isOverHigh = false;
        }
        else if (attribute_3 >= attribute_3_highlimit)
        {
            ending.prop = Property.AGILITY;
            ending.isOverHigh = true;
        }
        else if (attribute_3 <= attribute_3_lowlimit)
        {
            ending.prop = Property.AGILITY;
            ending.isOverHigh = false;
        }
        else if (attribute_4 >= attribute_4_highlimit)
        {
            ending.prop = Property.THOUGHT;
            ending.isOverHigh = true;
        }
        else if (attribute_4 <= attribute_4_lowlimit)
        {
            ending.prop = Property.THOUGHT;
            ending.isOverHigh = false;
        }
        else if (attribute_5 >= attribute_5_highlimit)
        {
            ending.prop = Property.COURAGE;
            ending.isOverHigh = true;
        }
        else if (attribute_5 <= attribute_5_lowlimit)
        {
            ending.prop = Property.COURAGE;
            ending.isOverHigh = false;
        }
        else if (attribute_6 >= attribute_6_highlimit)
        {
            ending.prop = Property.STRENTH;
            ending.isOverHigh = true;
        }
        else if (attribute_6 <= attribute_6_lowlimit)
        {
            ending.prop = Property.STRENTH;
            ending.isOverHigh = false;
        }
        else
        {
            ending.prop = Property.NONE;
            ending.isOverHigh = false;
        }
        return ending;
    }


    void Awake()
    {
        //ranges = Enumerable.Repeat(new List<int> (), 6).ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
