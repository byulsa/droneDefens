using UnityEngine;

public class Core : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 500;
    public int currentHP;

    [Header("Upload")]
    [Range(0, 100)]
    public float uploadProgress = 0f;

    public float uploadSpeed = 0.01f;     // 초당 증가량
    public float waveClearBonus = 5f;     // 웨이브 클리어 보너스

    [Header("Protection Mode")]
    public bool isProtectionMode = false;
    public float protectionDuration = 10f;

    private float protectionTimer;

    [Header("Attack")]
    public LayerMask enemyLayer;
    public float attackRange = 10f;
    public int attackDamage = 100;
    public float attackCooldown = 1f;
    public float uploadCostPerAttack = 5f;

    private float lastAttackTime;

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        Upload();

        UpdateProtectionMode();

        if (isProtectionMode &&
            Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }

        if (uploadProgress >= 100f)
        {
            ClearGame();
        }
    }

    void Upload()
    {
        if(!WaveManager.Instance.isStarting) return;

        uploadProgress += uploadSpeed * Time.deltaTime;

        uploadProgress = Mathf.Clamp(uploadProgress, 0f, 100f);
    }

    void UpdateProtectionMode()
    {
        if (!isProtectionMode)
            return;

        protectionTimer -= Time.deltaTime;

        if (protectionTimer <= 0f)
        {
            isProtectionMode = false;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        // 보호 모드 진입
        isProtectionMode = true;
        protectionTimer = protectionDuration;

        if (currentHP <= 0)
        {
            GameOver();
        }
    }

    void Attack()
    {
        EnemyBase enemy = FindNearestEnemy();

        if (enemy == null)
            return;

        enemy.TakeDamage(attackDamage);

        uploadProgress -= uploadCostPerAttack;

        uploadProgress =
            Mathf.Clamp(uploadProgress, 0f, 100f);

        lastAttackTime = Time.time;
    }

    EnemyBase FindNearestEnemy()
    {
        Collider[] colliders =
            Physics.OverlapSphere(
                transform.position,
                attackRange,
                enemyLayer);

        EnemyBase nearestEnemy = null;

        float nearestDistance = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            EnemyBase enemy =
                col.GetComponent<EnemyBase>();

            if (enemy == null)
                continue;

            float distance =
                Vector3.Distance(
                    transform.position,
                    enemy.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    public void OnWaveClear()
    {
        uploadProgress += waveClearBonus;

        uploadProgress = Mathf.Clamp(uploadProgress, 0f, 100f);
    }

    void ClearGame()
    {
        Debug.Log("UPLOAD COMPLETE");
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRange);
    }
}