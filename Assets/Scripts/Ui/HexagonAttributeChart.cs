using UnityEngine;
using UnityEngine.UI;

public class HexagonAttributeChart : MonoBehaviour
{
    public Image[] hexagonSegments;  // ���������ͼ��
    public float[] attributes;  // ��������ֵ

    void Start()
    {
        UpdateHexagon();
    }

    void UpdateHexagon()
    {
        for (int i = 0; i < 6; i++)
        {
            // ��ÿ���������ɫ��������ֵ��̬����
            Color color = Color.Lerp(Color.red, Color.green, attributes[i]);
            hexagonSegments[i].color = color;
        }
    }
}
