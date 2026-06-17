using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OwnedTurret
{
    public TurretData turretData;
    public int count = 1;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TurretRewardManager turretRewards;
    public WaveManager waveManagers;

    [Header("Inventory")]
    public List<OwnedTurret> ownedTurrets = new();

    [Header("Installed Turrets")]
    public List<Turret> activeTurrets = new();

    [Header("Selected Turret")]
    public TurretData selectedTurret;

    public bool isPlacingTurret;
    [Header("Memory")] //포탑용량
    public int maxMemory = 100;
    public int usedMemory = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (ownedTurrets.Count > 0)
            selectedTurret = ownedTurrets[0].turretData;
    }

    public void AddTurret(TurretData data)
    {
        OwnedTurret turret =
            ownedTurrets.Find(x => x.turretData == data);

        if (turret != null)
        {
            turret.count++;
        }
        else
        {
            ownedTurrets.Add(new OwnedTurret
            {
                turretData = data,
                count = 1
            });
        }

        if (selectedTurret == null)
            selectedTurret = data;
    }

    public void ConsumeTurret(TurretData data)
    {
        OwnedTurret turret =
            ownedTurrets.Find(x => x.turretData == data);

        if (turret == null)
            return;

        turret.count--;

        if (turret.count <= 0)
        {
            ownedTurrets.Remove(turret);

            if (selectedTurret == data)
            {
                selectedTurret =
                    ownedTurrets.Count > 0 ?
                    ownedTurrets[0].turretData :
                    null;
            }
        }
    }

    public void RegisterTurret(Turret turret)
    {
        activeTurrets.Add(turret);
    }

    public void ReturnTurret(Turret turret)
    {
        AddTurret(turret.turretData);

        RemoveMemoryUsage(turret.turretData.memoryCost);

        activeTurrets.Remove(turret);

        Destroy(turret.gameObject);
    }
    public bool CanUseMemory(int amount)
    {
        return usedMemory + amount <= maxMemory;
    }

    public void AddMemoryUsage(int amount)
    {
        usedMemory += amount;
    }

    public void RemoveMemoryUsage(int amount)
    {
        usedMemory -= amount;

        if (usedMemory < 0)
            usedMemory = 0;
    }
}