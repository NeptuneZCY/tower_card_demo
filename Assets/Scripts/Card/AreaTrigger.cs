using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    private RectTransform rectTransform;
    public DicePort dicePort;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // ���ĳ�����Ƿ��ڷ�Χ��
    public bool IsInBounds(Vector2 position)
    {
        //Debug.Log("bound: " + rectTransform.rect.ToString() + "position: " + position.ToString());
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, position, Camera.current);
    }

    // �����屻����÷�Χʱ����
    public void OnItemDropped(DiceDragger item)
    {
        Debug.Log($"���� {item.name} ������ {name}, dice value: {item.dice.value}");
        item.transform.position = rectTransform.position;
        DiceManager.Instance.dicePortDict.Add(item.dice, dicePort);
    }
}
