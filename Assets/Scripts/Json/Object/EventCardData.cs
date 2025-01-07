using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EventCardData
{
    public int id; // 序号
    public string name; // 标题名称
    public string description; // 描述
    public string icon; // 图标路径
    public string key1_shape; // 卡槽1骰子形状
    public string key1_attri; // 卡槽1影响属性
}
