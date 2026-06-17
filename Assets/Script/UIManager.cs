using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Core")]
    public Core corein;
    [Header("text")]
    public TextMeshProUGUI memoryText;
    public TextMeshProUGUI CoreHP;
    public TextMeshProUGUI CoreUploading;
    public TextMeshProUGUI CurrnetTurretText;
    public TextMeshProUGUI CurrnetTurretMemoryText;
    public TextMeshProUGUI CurrnetTurretTagText;
    public Image CurrnetTurretImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        memoryText.text = $"Memory\n{GameManager.Instance.usedMemory} / {GameManager.Instance.maxMemory}";
        CoreHP.text = $"HP : {corein.currentHP} / {corein.maxHP}";
        CoreUploading.text = $"CORE UPLOADING\n<size=24>{corein.uploadProgress}</size>";
        UpdateCurrentTurretUI();
    }
    void UpdateCurrentTurretUI()
    {
        TurretData selected = GameManager.Instance.selectedTurret;

        if (selected == null)
        {
            CurrnetTurretText.text = "None";
            CurrnetTurretImage.sprite = null;
            return;
        }

        OwnedTurret owned =
            GameManager.Instance.ownedTurrets.Find(
                x => x.turretData == selected);

        int count = owned != null ? owned.count : 0;

        CurrnetTurretText.text = $"{selected.turretName}\n" + $"Memory : {selected.memoryCost}\n" + $"Count : {count}";

        CurrnetTurretImage.sprite = selected.TurretImage;
        
    }
}
