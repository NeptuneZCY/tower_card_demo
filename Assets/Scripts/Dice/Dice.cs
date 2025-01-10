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
        value = UnityEngine.Random.Range(1, limit + 1);// ����һ��1��limit֮����������
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
            throw new ArgumentOutOfRangeException(nameof(value), "����ֵ������Ч��Χ�ڣ�");
        }
    }
}

public enum DiceType
{
    NONE = 0,
    DICE_4 = 1 << 4,
    DICE_6 = 1 << 6
}
