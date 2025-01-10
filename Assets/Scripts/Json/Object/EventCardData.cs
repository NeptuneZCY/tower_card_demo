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
    public string icon; // 卡牌图标路径
    public string key1_shape; // 卡槽1骰子形状
    public string key1_attri; // 卡槽1影响属性
    public int key1_affect; // 卡槽1属性影响方式
    public string key2_shape; // 卡槽2骰子形状
    public string key2_attri; // 卡槽2影响属性
    public int key2_affect; // 卡槽2属性影响方式
    public string key3_shape; // 卡槽3骰子形状
    public string key3_attri; // 卡槽3影响属性
    public int key3_affect; // 卡槽3属性影响方式
}
