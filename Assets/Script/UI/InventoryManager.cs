using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Owned Turrets")]
    public List<OwnedTurret> ownedTurrets = new();

    [Header("Installed Turrets")]
    public List<Turret> activeTurrets = new();

    [Header("Selected Turret")]
    public TurretData selectedTurret;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (ownedTurrets.Count > 0)
        {
            selectedTurret = ownedTurrets[0].turretData;
        }
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
            ownedTurrets.Add(
                new OwnedTurret()
                {
                    turretData = data,
                    count = 1
                });
        }

        if (selectedTurret == null)
        {
            selectedTurret = data;
        }
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
                if (ownedTurrets.Count > 0)
                {
                    selectedTurret = ownedTurrets[0].turretData;
                }
                else
                {
                    selectedTurret = null;
                }
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

        activeTurrets.Remove(turret);

        Destroy(turret.gameObject);
    }
}