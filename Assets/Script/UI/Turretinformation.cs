using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class Turretinformation : MonoBehaviour
{
    public TextMeshProUGUI Turretname;
    public TextMeshProUGUI TurretDMGtext;
    public TextMeshProUGUI TurretATKrateText;
    public TextMeshProUGUI TurretRangeText;
    public TextMeshProUGUI MemoryText;
    public TextMeshProUGUI InformationText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PanelUpdate(TurretData data)
    {
        Debug.Log(data.turretName);
        Turretname.text = data.turretName;
        TurretDMGtext.text = $"DMG : {data.damage}";
        TurretATKrateText.text = $"ATK-Rate : {data.attackRate}";
        TurretRangeText.text = $"Range : {data.attackRange}";
        MemoryText.text = $"Memory : {data.memoryCost}";
        InformationText.text = data.turretEx;
    }
}
