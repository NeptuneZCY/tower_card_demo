using UnityEngine;
using UnityEngine.UI;

public class HexagonChart : MonoBehaviour
{
    public Image[] attributeImages;  // 6个属性的UI Image组件
    public float[] attributeValues;  // 6个属性的值（0到1之间）

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
                // 根据属性值改变填充颜色或者透明度
                Color color = Color.Lerp(Color.red, Color.green, attributeValues[i]);
                attributeImages[i].color = color;
                attributeImages[i].fillAmount = attributeValues[i];  // 修改填充量
            }
        }
    }
}
