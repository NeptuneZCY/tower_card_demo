using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck
{
    public List<Card> cardList;

    public void Init<T>(List<T> list) where T : Card
    {
        cardList = new List<Card> ();
        cardList.AddRange(list);
    }

    public List<Card> DealCards(int count = 1)
    {
        List<Card> dealCardList = new List<Card> ();
        for (int i = 0; i< count; i++)
        {
            if (cardList.Count <= 0)
            {
                break;
            }
            int randomIndex = Random.Range(0, cardList.Count);
            Card card = cardList[randomIndex];
            cardList.RemoveAt(randomIndex);
            dealCardList.Add(card);
        }
        return dealCardList;
    }
}
