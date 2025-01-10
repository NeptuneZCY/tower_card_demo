using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoleData
{
    public int id; // 序号
    public string name; // 角色名
    public string dice; // 初始骰子
    public float attribute_1; // 属性1初始值
    public float attribute_1_lowlimit; // 属性1默认下限
    public float attribute_1_uplimit; // 属性1默认上限
}
