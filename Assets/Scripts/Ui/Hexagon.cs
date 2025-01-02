using UnityEngine;

public class Hexagon : MonoBehaviour
{
    public float radius = 500f;

    void Start()
    {
        DrawHexagon();
    }

    void DrawHexagon()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 7;  // 6 corners + 1 to close the loop
        lineRenderer.widthMultiplier = 2;

        // �����εĽǶ�������60�ȣ�
        float angleStep = 60f;
        for (int i = 0; i < 6; i++)
        {
            float angle = angleStep * i;
            Vector3 position = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
                Mathf.Sin(Mathf.Deg2Rad * angle) * radius,
                0f
            );
            lineRenderer.SetPosition(i, position);
        }

        // �պ�������
        lineRenderer.SetPosition(6, lineRenderer.GetPosition(0));
    }
}
