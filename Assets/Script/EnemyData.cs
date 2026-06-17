using UnityEngine;

[System.Serializable]
public enum EnemyType
{
    Normal,
    Hacker,
    Boss
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "CORE/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Info")]
    public string enemyName;

    [Header("Status")]
    public int maxHealth = 100;
    public int damage = 10;

    [Header("Move")]
    public float moveSpeed = 5f;

    [Header("Combat")]
    public float attackRange = 1f;
    public float attackCooldown = 1f;

    [Header("Reward")]
    public int reward = 10;

    [Header("Type")]
    public EnemyType enemyType;
}