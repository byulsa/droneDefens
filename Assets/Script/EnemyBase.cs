using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Move,
    Attack,
    Hack,
    Dead
}

public class EnemyBase : MonoBehaviour
{
    protected NavMeshAgent agent;
    [Header("Data")]
    public EnemyData enemyData;

    [Header("Current Status")]
    [SerializeField] protected int currentHealth;
    [SerializeField] protected float currentMoveSpeed;
    protected int currentDamage;

    public float CurrentMoveSpeed
    {
        get => currentMoveSpeed;
        set
        {
            if (isStunned)
            {
                currentMoveSpeed = 0f;
            }
            else
            {
                // value(들어온 값)를 검사하고 최소 속도(1f)보다 작으면 1f로 제한
                float minSpeed = 1f;
                currentMoveSpeed = value <= minSpeed ? minSpeed : value;
            }

            if (agent != null && agent.enabled)
            {
                agent.speed = currentMoveSpeed;
            }
        }
    }

    protected EnemyState currentState;
    private float debuffTimer = 0f;
    private bool isSlowed = false;
    private bool isStunned = false;
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    protected virtual void Start()
    {
        
        currentHealth = enemyData.maxHealth;
        currentDamage = enemyData.damage;

        // 변수 직접 대입 대신 프로퍼티 사용
        CurrentMoveSpeed = enemyData.moveSpeed;
        currentState = EnemyState.Move;
    }
    public void Initialize()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        agent.enabled = true;

        currentHealth = enemyData.maxHealth;
        currentDamage = enemyData.damage;
        CurrentMoveSpeed = enemyData.moveSpeed;

        currentState = EnemyState.Move;

        isSlowed = false;
        isStunned = false;
        debuffTimer = 0f;
    }

    protected virtual void Update()
    {
        if (isSlowed || isStunned)
        {
            debuffTimer -= Time.deltaTime;
            if (debuffTimer <= 0f)
            {
                ResetDebuffs();
            }
        }
    }

    public void ApplyDebuff(DebuffType type, float value, float duration)
    {
        if (currentState == EnemyState.Dead) return;

        switch (type)
        {
            case DebuffType.Slow:
                if (!isStunned)
                {
                    isSlowed = true;
                    debuffTimer = duration;
                    // 프로퍼티를 통해 속도 설정
                    CurrentMoveSpeed = enemyData.moveSpeed * (1f - value);
                }
                break;

            case DebuffType.Stun:
                isStunned = true;
                isSlowed = false;
                debuffTimer = duration;
                // 프로퍼티를 통해 속도 설정
                CurrentMoveSpeed = 0f;
                if (agent != null && agent.enabled && agent.hasPath) agent.ResetPath();
                break;

            case DebuffType.Knockback:
                ExecuteKnockback(value);
                break;
        }
    }

    private void ExecuteKnockback(float force)
    {
        if (agent == null) return;
        agent.enabled = false;

        // 변경: CORE 반대 방향(뒤쪽) 벡터에서 Y축을 평평하게 깎음
        Vector3 knockbackDir = -transform.forward;
        knockbackDir.y = 0f;
        knockbackDir.Normalize(); // 방향 값 순수화
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(knockbackDir * force, ForceMode.Impulse);

        Invoke("CalculateNavMesh", 0.2f);
    }

    private void CalculateNavMesh()
    {
        if (currentState == EnemyState.Dead) return;
        agent.enabled = true;
        // 프로퍼티를 통해 속도 복구
        CurrentMoveSpeed = isSlowed ? enemyData.moveSpeed * 0.5f : enemyData.moveSpeed;
    }

    private void ResetDebuffs()
    {
        isSlowed = false;
        isStunned = false;
        // 프로퍼티를 통해 속도 원래대로 복구
        CurrentMoveSpeed = enemyData.moveSpeed;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        WaveManager.Instance.EnemyDead();
        currentState = EnemyState.Dead;
        StartCoroutine(DieSet());
        //Destroy(gameObject, 6f);
    }
    protected virtual IEnumerator DieSet()
    {
        yield return new WaitForSeconds(6);
        gameObject.SetActive(false);
    }
}