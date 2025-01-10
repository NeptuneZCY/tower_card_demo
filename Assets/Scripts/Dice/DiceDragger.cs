using UnityEngine;
using UnityEngine.EventSystems;  // ����EventSystems�����ռ�

public class DiceDragger : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector2 offset;  // �洢���λ�õ�ƫ��
    private RectTransform rectTransform;  // ��ȡ UI Ԫ�ص� RectTransform
    private bool isDragging = false;  // �ж��Ƿ������϶�
    private Vector2 origin;
    public Dice dice;
    private CardSceneManager cardSceneManager;

    public RectTransform containerRectTransform;  // `DiceContainer` �� RectTransform���������Ʒ�Χ

    void Awake()
    {
        cardSceneManager = GameObject.Find("GameApp").GetComponent<CardSceneManager>();
    }

    void Start()
    {
        // ��ȡ��ǰ����� RectTransform
        rectTransform = GetComponent<RectTransform>();
        containerRectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
        origin = rectTransform.position;
    }

    // �����ʱ����¼���λ�õ�ƫ����
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 localPointerPosition = eventData.position - (Vector2)rectTransform.position; // ��ȡ���λ����������ӵ�λ��
        offset = localPointerPosition;
        isDragging = true;
        DiceManager.Instance.dicePortDict.Remove(dice);
        cardSceneManager.OnPrescore();
        Debug.Log($"Start drag dice, value: {dice.value}");
    }

    // ���϶�ʱ����������λ�ã���ȷ�����岻����������Χ
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector2 newPosition = eventData.position - offset;

            // ��ȡ�����ı߽�
            float containerMinX = containerRectTransform.position.x + containerRectTransform.rect.xMin;
            float containerMaxX = containerRectTransform.position.x + containerRectTransform.rect.xMax;
            float containerMinY = containerRectTransform.position.y + containerRectTransform.rect.yMin;
            float containerMaxY = containerRectTransform.position.y + containerRectTransform.rect.yMax;

            // ��ȡ���ӵĴ�С
            float diceWidth = rectTransform.rect.width;
            float diceHeight = rectTransform.rect.height;

            // ����������������
            float clampedX = Mathf.Clamp(newPosition.x, containerMinX + diceWidth / 2, containerMaxX - diceWidth / 2);
            float clampedY = Mathf.Clamp(newPosition.y, containerMinY + diceHeight / 2, containerMaxY - diceHeight / 2);

            // ��������λ��
            rectTransform.position = new Vector3(clampedX, clampedY, rectTransform.position.z);
        }
    }

    // ���϶�����ʱ��ֹͣ�϶�
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        var areaTriggerList = FindObjectsOfType<AreaTrigger>();
        foreach ( var areaTrigger in areaTriggerList )
        {
            if (areaTrigger.IsInBounds(Input.mousePosition))
            {
                Debug.Log($"{name} ������ {areaTrigger.name} �ķ�Χ��");
                areaTrigger.OnItemDropped(this);
                cardSceneManager.OnPrescore();
                return;
            }
        }
        rectTransform.position = origin;
    }
}
