using UnityEngine;

public enum AttackType { Single, Area }
public enum DebuffType { None, Slow, Knockback, Stun }
public enum TargetType { None, MaxHealth, MinHealth, CurrentHealth }
public enum Rating { Common, Rare, Epic }

[CreateAssetMenu(fileName = "NewTurretData", menuName = "ScriptableObjects/TurretData")]
public class TurretData : ScriptableObject
{
    [Header("Basic Info")]
    public float attackRange;
    public float attackRate;
    public int damage;
    public int memoryCost;

    [Header("Targeting")]
    public TargetType targetType;
    public AttackType attackType;
    public DebuffType debuffType;
    public float debuffValue; 
    public float debuffDuration; 

    [Header("Ex")]
    public string turretName;
    public string turretTag;
    public Sprite TurretImage;
    [TextArea(3, 10)]
    public string turretEx; 
    public GameObject turretPrefab;
    [Header("등급")]
    public Rating rating;
}
