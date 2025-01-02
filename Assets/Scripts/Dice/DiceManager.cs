using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceManager
{
    static DiceManager _instance = new DiceManager();

    public static DiceManager Instance = _instance;

    public List<Dice> diceList = new List<Dice>();

    public Dictionary<Dice, DicePort> dicePortDict;

    public void Init()
    {
        dicePortDict = new Dictionary<Dice, DicePort>();
        diceList.AddRange(Enumerable.Range(0, 7).Select(i => new Dice(i, 6)).ToList());
        diceList.AddRange(Enumerable.Range(0, 2).Select(i => new Dice(i, 4)).ToList());
    }

    public void RollAllDice()
    {
        for (int i = 0; i < diceList.Count; i++)
        {
            Dice dice = diceList[i];
            dice.Roll();
        }
    }
}