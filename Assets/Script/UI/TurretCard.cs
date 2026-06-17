using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretCard : MonoBehaviour
{
    public TurretData turretData;

    public BundleCard parentBundle;
    public Turretinformation turretinformation;

    [Header("UI")]
    public TextMeshProUGUI Title;
    public TextMeshProUGUI TAGText;
    public Image TurretImage;

    public void SetData(TurretData data)
    {
        turretData = data;
        Title.text = data.turretName;
        TAGText.text = data.turretTag;
        TurretImage.sprite = data.TurretImage;
    }

    public void InfoUI()
    {
        turretinformation.PanelUpdate(turretData);
    }
}