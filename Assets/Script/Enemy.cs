using UnityEngine;
using UnityEngine.AI;

public class Enemy : EnemyBase
{
    public Transform coreTarget;
    public Animator animator;

    private float lastAttackTime = -Mathf.Infinity; // 처음엔 바로 공격 가능하도록

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        GameObject core = GameObject.FindGameObjectWithTag("CORE");

        if (core != null)
        {
            coreTarget = core.transform;
        }
        agent.speed = currentMoveSpeed;
        currentState = EnemyState.Move;
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Move:
                Move();
                break;

            case EnemyState.Attack:
                Attack();
                break;

            case EnemyState.Dead:
                break;
        }
    }

    protected virtual void Move()
    {
        if (coreTarget == null)
            return;

        animator.SetBool("isAtk", false);
        float distance = Vector3.Distance(transform.position, coreTarget.position);

        if (distance <= enemyData.attackRange)
        {
            currentState = EnemyState.Attack;
            agent.ResetPath();
            return;
        }
        if (agent.destination != coreTarget.position && agent.enabled)
        {
            agent.SetDestination(coreTarget.position);
        }
    }

    protected virtual void Attack()
    {
        if (coreTarget == null)
        {
            currentState = EnemyState.Move;
            return;
        }

        float distance = Vector3.Distance(transform.position, coreTarget.position);

        if (distance > enemyData.attackRange)
        {
            animator.SetBool("isAtk", false);
            currentState = EnemyState.Move;
            return;
        }

        animator.SetBool("isAtk", true);

        // 쿨타임 체크
        if (Time.time - lastAttackTime >= enemyData.attackCooldown)
        {
            lastAttackTime = Time.time;

            Core core = coreTarget.GetComponent<Core>();
            if (core != null)
            {
                core.TakeDamage(currentDamage);
            }
        }
    }

    protected override void Die()
    {
        base.Die();
        animator.SetTrigger("die");
        agent.isStopped = true;
        gameObject.GetComponent<Collider>().enabled = false;
    }
}