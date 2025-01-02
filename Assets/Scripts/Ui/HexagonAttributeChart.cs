using UnityEngine;
using UnityEngine.UI;

public class HexagonAttributeChart : MonoBehaviour
{
    public Image[] hexagonSegments;  // 六个区域的图像
    public float[] attributes;  // 六个属性值

    void Start()
    {
        UpdateHexagon();
    }

    void UpdateHexagon()
    {
        for (int i = 0; i < 6; i++)
        {
            // 将每个区域的颜色根据属性值动态调整
            Color color = Color.Lerp(Color.red, Color.green, attributes[i]);
            hexagonSegments[i].color = color;
        }
    }
}
