using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class CardSceneManager : MonoBehaviour
{
    DiceManager diceManager = DiceManager.Instance;
    JsonManager jsonManager = JsonManager.Instance;
    static readonly float CARD_XOFFSET = 500;
    static readonly float DICE_OFFSET = 200;
    static readonly float weaght = 0.05f;

    public GameObject diceContainer;
    public GameObject cardContainer;
    CardDeck cardDeck;
    int handCardCount = 3;

    UIPolygon propGraph;
    List<float> propGraphData;

    void Awake()
    {
        diceManager.Init();
        jsonManager.Init();

        cardDeck = new CardDeck();
        cardDeck.Init(EventCard.GetCardsByJsonObjs(jsonManager.eventCardObjs));
        
        foreach(Card card in cardDeck.cardList)
        {
            if (card is EventCard)
            {
                Debug.Log(card.ToString());
            }
        }

        propGraph = GameObject.Find("PropGraph").GetComponent<UIPolygon>();
        propGraphData = Enumerable.Repeat(0.5f, 6).ToList();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Score()
    {
        if (diceManager.dicePortDict.Count == 0) return;
        foreach (var pair in diceManager.dicePortDict)
        {
            Debug.Log($"{pair.Key}, ${pair.Value}");
            int value = pair.Key.value;
            int affect = pair.Value.affect;
            propGraphData[GetPropIndex(pair.Value.prop)] += value * affect * weaght;
        }
        diceManager.dicePortDict.Clear();
        Debug.Log("data: " + propGraphData);
        propGraph.DrawPolygon(propGraphData);
    }

    int GetPropIndex(Property prop)
    {
        return
            prop == Property.SPIRIT ? 0 :
            prop == Property.COURAGE ? 1 :
            prop == Property.AGILITY ? 2 :
            prop == Property.LOGIC ? 3 :
            prop == Property.THOUGHT ? 4 :
            prop == Property.STRENTH ? 5 : 0;
    }

    public void RollAllDice()
    {
        DeleteAllChildren(diceContainer);
        diceManager.RollAllDice();
        List<Dice> dices = diceManager.diceList;
        for (int i=0; i<dices.Count; i++) 
        {
            Dice dice = dices[i];
            string prefabName =
                dice.limit == 6 ? "SquareDice" :
                dice.limit == 4 ? "TriangleDice" : "";
            GameObject prefab = PrefabFactory.Instance.CreateInstance("Ui Prefab/dice/" + prefabName, new Vector2((-1 + i%3) * DICE_OFFSET, (-1 + i / 3) * DICE_OFFSET), Quaternion.identity);
            if (prefab != null)
            {
                prefab.transform.SetParent(diceContainer.transform, false);
                prefab.GetComponentInChildren<TextMeshProUGUI>().text = dice.value.ToString();
                prefab.GetComponentInChildren<DiceDragger>().dice = dice;
                Debug.Log("dice: " + dice.value + ", " + diceManager.diceList[i].value);
            }
        }
    }

    public void DealCards()
    {
        DeleteAllChildren(cardContainer);
        List<Card> cards = cardDeck.DealCards(handCardCount);
        for (int i=0; i<cards.Count; i++)
        {
            Card card = cards[i];
            if (card is EventCard eventCard)
            {
                GameObject prefab = PrefabFactory.Instance.CreateInstance("Ui Prefab/card/EventCard", new Vector2((-cards.Count/2 + i)*CARD_XOFFSET, 0), Quaternion.identity);
                if (prefab != null)
                {
                    prefab.transform.SetParent(cardContainer.transform, false);
                    Transform content = prefab.transform.Find(EventCard.CHILD_CONTENT);
                    if (content != null)
                    {
                        TextMeshProUGUI textMeshPro = content.GetComponent<TextMeshProUGUI>();
                        if (textMeshPro != null)
                        {
                            textMeshPro.text = eventCard.content;
                        } else
                        {
                            Debug.Log("no content text");
                        }
                    }
                    for (int portIndex = 0; portIndex < eventCard.dicePortList.Count; portIndex++)
                    {
                        DicePort dicePort = eventCard.dicePortList[portIndex];
                        GameObject portPrefab = PrefabFactory.Instance.CreateInstance("Ui Prefab/card/DicePropPort", Vector2.zero, Quaternion.identity);
                        if (portPrefab != null)
                        {
                            portPrefab.GetComponentInChildren<AreaTrigger>().dicePort = dicePort;
                            Transform portContainer = prefab.transform.Find(EventCard.PORT_CONTAINER);
                            Transform portParent = portContainer.transform.Find("Port" + portIndex);
                            if (portParent != null)
                            {
                                portPrefab.transform.SetParent (portParent.transform, false);
                                TextMeshProUGUI textMeshPro = portPrefab.GetComponentInChildren<TextMeshProUGUI>();
                                if (textMeshPro != null)
                                {
                                    textMeshPro.text = JsonManager.GetPropStr(dicePort.prop) + " " + (dicePort.affect> 0 ?"+":"") + dicePort.affect;
                                } else
                                {
                                    Debug.Log("no textMeshPro!");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void DeleteAllChildren(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public GameObject GetCardInstance(Card card)
    {
        return new GameObject();
    }
}