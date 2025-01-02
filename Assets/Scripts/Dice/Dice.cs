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
}
