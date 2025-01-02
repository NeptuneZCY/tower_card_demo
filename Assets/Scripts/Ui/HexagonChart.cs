using UnityEngine;
using UnityEngine.UI;

public class HexagonChart : MonoBehaviour
{
    public Image[] attributeImages;  // 6�����Ե�UI Image���
    public float[] attributeValues;  // 6�����Ե�ֵ��0��1֮�䣩

    void Start()
    {
        UpdateHexagonChart();
    }

    void UpdateHexagonChart()
    {
        for (int i = 0; i < 6; i++)
        {
            if (attributeImages[i] != null)
            {
                // ��������ֵ�ı������ɫ����͸����
                Color color = Color.Lerp(Color.red, Color.green, attributeValues[i]);
                attributeImages[i].color = color;
                attributeImages[i].fillAmount = attributeValues[i];  // �޸������
            }
        }
    }
}
