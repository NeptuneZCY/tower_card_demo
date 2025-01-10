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

    // 检测某个点是否在范围内
    public bool IsInBounds(Vector2 position)
    {
        //Debug.Log("bound: " + rectTransform.rect.ToString() + "position: " + position.ToString());
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, position, Camera.current);
    }

    // 当物体被放入该范围时触发
    public void OnItemDropped(DiceDragger item)
    {
        Debug.Log($"物体 {item.name} 放入了 {name}, dice value: {item.dice.value}");
        item.transform.position = rectTransform.position;
        DiceManager.Instance.dicePortDict.Add(item.dice, dicePort);
    }
}
