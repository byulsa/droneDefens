using UnityEngine;
using System.Collections.Generic;

public class Turret : MonoBehaviour
{
    public TurretData turretData;
    public LayerMask enemyLayer;
    public LayerMask obstacleLayer;

    [Header("Rotation Setup")]
    public Transform partToRotate;
    public float turnSpeed = 10f;
    
    [Header("Particle Effects")]
    public ParticleSystem FireEffect;

    private float attackCountdown = 0f;
    private List<Collider> enemiesInRange = new List<Collider>();
    private Transform currentTarget;
    public Animator animator;
    public MonoBehaviour recoilComponent;

    private void Start()
    {
        animator ??= GetComponent<Animator>();
    }

    private void Update()
    {
        attackCountdown -= Time.deltaTime;

        FindEnemies();
        UpdateTarget();

        if (currentTarget != null)
        {
            LockOnTarget();
        }

        if (enemiesInRange.Count > 0 && attackCountdown <= 0f)
        {
            Attack();
            attackCountdown = 1f / turretData.attackRate;
        }
    }

    private void FindEnemies()
    {
        enemiesInRange.Clear();
        Collider[] colliders = Physics.OverlapSphere(transform.position, turretData.attackRange, enemyLayer);
        enemiesInRange.AddRange(colliders);
    }

    private void UpdateTarget()
    {
        // 범위 공격(Area)이라면 무조건 가장 가까운 적을 조준 (None 처리)
        if (turretData.attackType == AttackType.Area)
        {
            currentTarget = GetClosestVisibleEnemy();
            return;
        }

        // 단일 공격(Single)일 때는 turretData에 기입된 타겟팅 설정에 따라 탐색
        switch (turretData.targetType)
        {
            case TargetType.None:
                currentTarget = GetClosestVisibleEnemy();
                break;
            case TargetType.MaxHealth:
                currentTarget = GetEnemyByHealth(true, false);
                break;
            case TargetType.MinHealth:
                currentTarget = GetEnemyByHealth(false, false);
                break;
            case TargetType.CurrentHealth:
                currentTarget = GetEnemyByHealth(false, true);
                break;
        }
    }

    private Transform GetClosestVisibleEnemy()
    {
        Transform closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (var enemyCollider in enemiesInRange)
        {
            if (enemyCollider == null) continue;

            Vector3 startPos = partToRotate != null ? partToRotate.position : transform.position;
            Vector3 targetPos = enemyCollider.transform.position;
            Vector3 direction = targetPos - startPos;
            float distanceToEnemy = direction.magnitude;

            if (Physics.Raycast(startPos, direction.normalized, distanceToEnemy, obstacleLayer))
            {
                continue;
            }

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemyCollider.transform;
            }
        }
        return closestEnemy;
    }

    private Transform GetEnemyByHealth(bool getMax, bool useCurrentHealth)
    {
        Transform selectedEnemy = null;
        float targetHealthValue = getMax ? Mathf.NegativeInfinity : Mathf.Infinity;

        foreach (var enemyCollider in enemiesInRange)
        {
            if (enemyCollider == null) continue;

            Vector3 startPos = partToRotate != null ? partToRotate.position : transform.position;
            Vector3 targetPos = enemyCollider.transform.position;
            Vector3 direction = targetPos - startPos;
            float distanceToEnemy = direction.magnitude;

            // 체력 타겟팅 기준을 잡을 때도 시야(벽)에 가려진 적은 제외
            if (Physics.Raycast(startPos, direction.normalized, distanceToEnemy, obstacleLayer))
            {
                continue;
            }

            EnemyBase enemy = enemyCollider.GetComponent<EnemyBase>();
            if (enemy == null) continue;

            // EnemyBase의 CurrentMoveSpeed 프로퍼티 디자인에 맞춰 실시간 체력 조회 연동
            float enemyHealth = useCurrentHealth ? enemy.GetComponent<EnemyBase>().GetType().GetField("currentHealth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(enemy) is int h ? h : 0 : enemy.enemyData.maxHealth;

            if (getMax)
            {
                if (enemyHealth > targetHealthValue)
                {
                    targetHealthValue = enemyHealth;
                    selectedEnemy = enemyCollider.transform;
                }
            }
            else
            {
                if (enemyHealth < targetHealthValue)
                {
                    targetHealthValue = enemyHealth;
                    selectedEnemy = enemyCollider.transform;
                }
            }
        }

        return selectedEnemy ?? GetClosestVisibleEnemy();
    }

    private void LockOnTarget()
    {
        if (partToRotate == null) return;
        Vector3 dir = currentTarget.position - partToRotate.position;
        dir.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        Vector3 rotation = Quaternion.Slerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;

        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void Attack()
    {
        // 1. 실제 데미지 프로세스 처리
        if (turretData.attackType == AttackType.Single)
        {
            if (currentTarget != null)
            {
                ExecuteAttack(currentTarget);
            }
        }
        else if (turretData.attackType == AttackType.Area)
        {
            foreach (var enemyCollider in enemiesInRange)
            {
                if (enemyCollider != null)
                {
                    ExecuteAttack(enemyCollider.transform);
                }
            }
        }

        // 2. 발사 연출 프로세스 처리 (범위 공격 시 이펙트나 반동이 중복 실행되는 것 방지)
        if (animator != null)
        {
            animator.SetTrigger("Fire");
        }
        
        (recoilComponent as IRecoil)?.PlayRecoil();
        
        if (FireEffect != null)
        {
            FireEffect.Play();
        }
    }

    private void ExecuteAttack(Transform enemyTransform)
    {
        EnemyBase enemy = enemyTransform.GetComponent<EnemyBase>();
        if (enemy == null) return;
        
        Debug.Log($"Attacking {enemy.name} for {turretData.damage} damage.");
        enemy.TakeDamage((int)turretData.damage);

        if (turretData.debuffType != DebuffType.None)
        {
            // 이전에 합의한 [터렛 -> 적] 넉백 방향 연동을 위해 transform.position 전달
            enemy.ApplyDebuff(turretData.debuffType, turretData.debuffValue, turretData.debuffDuration);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (turretData == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, turretData.attackRange);
    }
}