using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ThrowRangeVisualizer : MonoBehaviour
{
    public float radius = 3f;           // Bán kính ném bom
    public int segments = 100;          // Độ mịn của vòng tròn

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false; // Để vẽ theo local position
        DrawCircle();
    }

    void DrawCircle()
    {
        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0f));
            angle += 360f / segments;
        }
    }
}
