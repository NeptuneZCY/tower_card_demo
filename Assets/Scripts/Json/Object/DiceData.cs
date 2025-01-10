using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DiceData
{
    public int id; // 序号
    public string name; // 名称备注
    public string dice_number; // 随机数
    public string dice_prefab; // 对应预制体
}
