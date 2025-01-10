using System;
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
    RoleManager RoleManager = RoleManager.Instance;
    const float CARD_XOFFSET = 500;
    const float DICE_OFFSET = 200;
    const float weaght = 0.05f;

    public GameObject diceContainer;
    public GameObject cardContainer;
    public Player player;
    CardDeck cardDeck;
    int handCardCount = 3;

    UIPolygon propGraph;
    UIPolygon prePropGraph;

    Dictionary<Property, int> prePropDict;

    void Awake()
    {
        diceManager.Init();
        jsonManager.Init();

        cardDeck = new CardDeck();
        cardDeck.Init(EventCard.GetCardsByJsonDatas(jsonManager.eventCardDatas));

        foreach (Card card in cardDeck.cardList)
        {
            if (card is EventCard)
            {
                Debug.Log(card.ToString());
            }
        }

        propGraph = GameObject.Find("PropGraph").GetComponent<UIPolygon>();
        prePropGraph = GameObject.Find("PrePropGraph").GetComponent<UIPolygon>();
        DrawGraph();
    }

    void DrawGraph()
    {
        propGraph.DrawPolygon(GetPropertyRatioList());
    }

    List<float> GetPropertyList()
    {
        List<float> list = Enumerable.Repeat(0f, 6).ToList();
        Debug.Log("count: " + list.Count);
        foreach (KeyValuePair<Property, int> kvp in player.GetPropertyMap())
        {
            list[GetPropGraphIndex(kvp.Key)] = kvp.Value;
        }
        return list;
    }

    List<float> GetPropertyRatioList()
    {
        List<float> list = Enumerable.Repeat(0f, 6).ToList();
        Debug.Log("count: " + list.Count);
        foreach (KeyValuePair<Property, float> kvp in player.GetPropertyRatioMap())
        {
            list[GetPropGraphIndex(kvp.Key)] = kvp.Value;
        }
        return list;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 拖入骰之后预计分
    public void OnPrescore()
    {
        if (diceManager.dicePortDict.Count == 0) return;
        Dictionary<Property, int> affectDict = new Dictionary<Property, int>();
        foreach (var pair in diceManager.dicePortDict)
        {
            Debug.Log($"Score dice value: {pair.Key.value}, port: {pair.Value.affect}");
            int value = pair.Key.value;
            int affect = pair.Value.affect;
            Property prop = pair.Value.prop;
            affectDict.Add(prop, value * affect);
        }
        List<float> list = Enumerable.Repeat(0f, 6).ToList();
        foreach (KeyValuePair<Property, float> kvp in player.GetPropertyRatioDictByAffectDict(affectDict))
        {
            list[GetPropGraphIndex(kvp.Key)] = kvp.Value;
        }
        prePropGraph.DrawPolygon(list);
    }

    public void Score()
    {
        List<float> list = Enumerable.Repeat(0f, 6).ToList();
        prePropGraph.DrawPolygon(list);

        if (diceManager.dicePortDict.Count == 0) return;
        Dictionary<Property, int> propDict = new Dictionary<Property, int>();
        foreach (var pair in diceManager.dicePortDict)
        {
            Debug.Log($"Score dice value: {pair.Key.value}, port: {pair.Value.affect}");
            int value = pair.Key.value;
            int affect = pair.Value.affect;
            propDict.Add(pair.Value.prop, value * affect);
        }
        diceManager.dicePortDict.Clear();
        AffectPropListByDict(propDict);
        DrawGraph();

        JudgeEnding();
    }

    void JudgeEnding()
    {
        Ending ending = player.JudgeEnding();
        if (ending.prop != Property.NONE)
        {
            string endStr = "游戏结束\n";
            endStr += JsonManager.GetPropStr(ending.prop);
            endStr += ending.isOverHigh ? "超过了上限" : "跌破了下限";
            Debug.Log(endStr);
            GameObject endingObj = GameObject.Find("EndingBg");
            TextMeshProUGUI textMeshPro = endingObj.GetComponentInChildren<TextMeshProUGUI>();
            textMeshPro.text = endStr;
            endingObj.transform.localPosition = Vector2.zero;
        }
    }

    void AffectPropListByDict(Dictionary<Property, int> dict)
    {
        foreach (var pair in dict)
        {
            switch (pair.Key)
            {
                case Property.SPIRIT:
                    player.attribute_1 += pair.Value;
                    break;
                case Property.LOGIC:
                    player.attribute_2 += pair.Value;
                    break;
                case Property.AGILITY:
                    player.attribute_3 += pair.Value;
                    break;
                case Property.THOUGHT:
                    player.attribute_4 += pair.Value;
                    break;
                case Property.COURAGE:
                    player.attribute_5 += pair.Value;
                    break;
                case Property.STRENTH:
                    player.attribute_6 += pair.Value;
                    break;
                default:
                    break;
            }
        }
    }

    int GetPropGraphIndex(Property prop)
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
                            Transform portParent = portContainer.transform.Find("Port" + (portIndex + 1));
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