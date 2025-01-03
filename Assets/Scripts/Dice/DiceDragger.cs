using UnityEngine;
using UnityEngine.EventSystems;  // 导入EventSystems命名空间

public class DiceDragger : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector2 offset;  // 存储点击位置的偏移
    private RectTransform rectTransform;  // 获取 UI 元素的 RectTransform
    private bool isDragging = false;  // 判断是否正在拖动
    private Vector2 origin;
    public Dice dice;

    public RectTransform containerRectTransform;  // `DiceContainer` 的 RectTransform，用于限制范围

    void Start()
    {
        // 获取当前物体的 RectTransform
        rectTransform = GetComponent<RectTransform>();
        containerRectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
        origin = rectTransform.position;
    }

    // 当点击时，记录点击位置的偏移量
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 localPointerPosition = eventData.position - (Vector2)rectTransform.position; // 获取点击位置相对于骰子的位置
        offset = localPointerPosition;
        isDragging = true;
    }

    // 当拖动时，更新物体位置，并确保物体不超出容器范围
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector2 newPosition = eventData.position - offset;

            // 获取容器的边界
            float containerMinX = containerRectTransform.position.x + containerRectTransform.rect.xMin;
            float containerMaxX = containerRectTransform.position.x + containerRectTransform.rect.xMax;
            float containerMinY = containerRectTransform.position.y + containerRectTransform.rect.yMin;
            float containerMaxY = containerRectTransform.position.y + containerRectTransform.rect.yMax;

            // 获取骰子的大小
            float diceWidth = rectTransform.rect.width;
            float diceHeight = rectTransform.rect.height;

            // 限制物体在容器内
            float clampedX = Mathf.Clamp(newPosition.x, containerMinX + diceWidth / 2, containerMaxX - diceWidth / 2);
            float clampedY = Mathf.Clamp(newPosition.y, containerMinY + diceHeight / 2, containerMaxY - diceHeight / 2);

            // 更新物体位置
            rectTransform.position = new Vector3(clampedX, clampedY, rectTransform.position.z);
        }
    }

    // 当拖动结束时，停止拖动
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        var areaTriggerList = FindObjectsOfType<AreaTrigger>();
        foreach ( var areaTrigger in areaTriggerList )
        {
            if (areaTrigger.IsInBounds(Input.mousePosition))
            {
                Debug.Log($"{name} 放入了 {areaTrigger.name} 的范围！");
                areaTrigger.OnItemDropped(this);
                return;
            }
        }
        rectTransform.position = origin;
        DiceManager.Instance.dicePortDict.Remove(dice);
    }
}
