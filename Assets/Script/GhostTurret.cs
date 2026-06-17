using UnityEngine;

public class GhostTurret : MonoBehaviour
{
    [SerializeField]
    private LineRenderer rangeRenderer;

    [SerializeField]
    private Renderer bodyRenderer;

    public void SetRange(float range)
    {
        DrawCircle(range);
    }

    public void SetValid(bool canPlace)
    {
        Color color = canPlace ? Color.green : Color.red;
        color.a = 0.25f;

        bodyRenderer.material.SetColor("_Color", color);

        rangeRenderer.startColor = color;
        rangeRenderer.endColor = color;
    }

    private void DrawCircle(float radius)
    {
        int segments = 50;

        rangeRenderer.positionCount = segments + 1;

        for (int i = 0; i <= segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2f;

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            rangeRenderer.SetPosition(
                i,
                new Vector3(x, 0.05f, z));
        }
    }
}