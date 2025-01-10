using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
{
    private int index = 0;

    public int limit = 6;

    public int value = -1;

    public Dice(int index, int limit)
    {
        this.limit = limit;
    }

    public void Roll()
    {
        value = UnityEngine.Random.Range(1, limit + 1);// 返回一个1到limit之间的随机整数
    }

    public static DiceType GetDiceType(int value)
    {
        Debug.Log($"GetDiceType: {value}");
        if (Enum.IsDefined(typeof(DiceType), (1 << value)))
        {
            return (DiceType)(1 << value);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(value), "输入值不在有效范围内！");
        }
    }
}

public enum DiceType
{
    NONE = 0,
    DICE_4 = 1 << 4,
    DICE_6 = 1 << 6
}
