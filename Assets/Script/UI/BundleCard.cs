using UnityEngine;
using UnityEngine.UI;

public class BundleCard : MonoBehaviour
{
    public TurretData turretA;
    public TurretData turretB;

    [Header("Card")]
    public TurretCard cardA;
    public TurretCard cardB;

    [Header("Hover")]
    public float hoverDuration = 0.25f;
    public float hoverScale = 1.1f;

    private Vector3 originScale;
    private Vector3 targetScale;

    public Outline outline;

    void Start()
    {
        originScale = transform.localScale;
        targetScale = originScale;
    }

    void Update()
    {
        transform.localScale =
            Vector3.Lerp(
                transform.localScale,
                targetScale,
                Time.deltaTime / hoverDuration);
    }

    public void SetData(TurretData a, TurretData b)
    {
        turretA = a;
        turretB = b;

        cardA.SetData(a);
        cardB.SetData(b);
    }

    public void Hovering()
    {
        outline.effectColor = Color.yellow;
        targetScale = originScale * hoverScale;
    }

    public void ExitHover()
    {
        outline.effectColor = Color.white;
        targetScale = originScale;
    }
}