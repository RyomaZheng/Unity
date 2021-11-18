using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { GUARD, PATROL, CHASE, DEAD }
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class EnemyController : MonoBehaviour, IEndGameObserver
{
    private Animator animator;

    private EnemyStates enemyStates;

    private NavMeshAgent agent;

    protected CharacterStats characterStats;

    private Quaternion guardRotation;

    private Collider coll;

    [Header("Basic Setting")]
    public float sightRadius;
    public bool isGuard;
    private float speed;
    protected GameObject attackTarget;
    public float lookAtTime;
    private float remainLookAtTime;
    private float lastAttackTime;

    [Header("Patrol state")]
    public float patrolRange;
    private Vector3 wayPoint;
    private Vector3 guardPoint;

    private bool isWalk;

    private bool isChase;

    private bool isFollow;

    private bool isDead;

    private bool isPlayerDead;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        characterStats = GetComponent<CharacterStats>();
        coll = GetComponent<Collider>();

        speed = agent.speed;
        remainLookAtTime = lookAtTime;

        guardPoint = transform.position;
        guardRotation = transform.rotation;
    }

    void Start()
    {
        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
        }
        // TODO:场景切换的时候做修改
        GameManager.Instance.AddObserver(this);
    }

    private void Update()
    {
        isDead = characterStats.CurrentHealth == 0;

        if (!isPlayerDead)
        {
            SwitchStates();
            SwitchAnimation();
            lastAttackTime -= Time.deltaTime;
        }

    }
    // TODO:场景切换的时候做修改
    /*private void OnEnable()
    {
        GameManager.Instance.AddObserver(this);
    }*/

    private void OnDisable()
    {
        if (!GameManager.isInitialized)
        {
            return;
        }
        GameManager.Instance.RemoveObserver(this);

        if (GetComponent<LootSpawner>() && isDead)
        {
            GetComponent<LootSpawner>().Spawnloot();
        }
    }

    void SwitchAnimation()
    {
        animator.SetBool("walk", isWalk);
        animator.SetBool("chase", isChase);
        animator.SetBool("follow", isFollow);
        animator.SetBool("critical", characterStats.isCritical);
        animator.SetBool("dead", isDead);
    }

    void SwitchStates()
    {
        if (isDead)
        {
            enemyStates = EnemyStates.DEAD;
        }
        else if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
            Debug.Log("发现player");
        }
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                SwitchGuardState();
                break;
            case EnemyStates.PATROL:
                SwitchPatrolState();
                break;
            case EnemyStates.CHASE:
                SwitchChaseState();
                break;
            case EnemyStates.DEAD:
                SwitchDeadState();
                break;
        }

    }

    private void SwitchDeadState()
    {
        coll.enabled = false;
        //agent.enabled = false;
        agent.radius = 0;

        Destroy(gameObject, 3f);
    }

    private void SwitchGuardState()
    {
        isChase = false;

        if (transform.position != guardPoint)
        {
            isWalk = true;
            agent.isStopped = false;
            agent.destination = guardPoint;

            if (Vector3.SqrMagnitude(guardPoint - transform.position) <= agent.stoppingDistance)
            {
                isWalk = false;
                transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.01f);
            }
        }
    }

    bool FoundPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                attackTarget = colliders[i].gameObject;
                return true;
            }
        }

        attackTarget = null;
        return false;
    }

    private void SwitchPatrolState()
    {
        // 追击状态改为false
        isChase = false;

        // 当前速度变慢
        agent.speed = speed * 0.5f;

        // 判断是否到了随机巡逻点
        if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
        {
            // 不再走动
            isWalk = false;

            // 判断时间是否大于0,0的话减去时间继续等待
            if (remainLookAtTime > 0)
            {
                remainLookAtTime -= Time.deltaTime;
            }
            else
            {
                // 寻找新的点
                GetNewWayPoint();
            }
        }
        else
        {
            isWalk = true;

            agent.destination = wayPoint;
        }
    }

    void SwitchChaseState()
    {
        // 在追击状态下恢复原本速度
        agent.speed = speed;

        // 追击player
        isWalk = false;
        isChase = true;

        if (!FoundPlayer())
        {
            // 拉脱回到上一个状态
            isFollow = false;

            if (remainLookAtTime > 0)
            {
                remainLookAtTime -= Time.deltaTime;
                agent.destination = transform.position;
            }
            else if (isGuard)
            {
                enemyStates = EnemyStates.GUARD;
            }
            else
            {
                enemyStates = EnemyStates.PATROL;
            }
        }
        else
        {
            isFollow = true;
            agent.isStopped = false;
            agent.destination = attackTarget.transform.position;
        }

        // 在攻击范围内则进行攻击
        if (TargetInAttackRange() || TargetInSkillRange())
        {
            isFollow = false;
            agent.isStopped = true;

            if (lastAttackTime < 0)
            {
                // 重新冷却
                lastAttackTime = characterStats.attackData.coolDown;

                // 暴击判断
                characterStats.isCritical = Random.value <= characterStats.attackData.criticalChance;

                // 执行攻击
                Attack();
            }
        }

    }

    void Attack()
    {
        transform.LookAt(attackTarget.transform);

        if (TargetInAttackRange())
        {
            // 近身攻击动画
            animator.SetTrigger("attack");
        }

        if (TargetInSkillRange())
        {
            // 技能攻击动画
            animator.SetTrigger("skill");
        }
    }

    bool TargetInAttackRange()
    {
        if (characterStats != null && attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        }
        else
        {
            return false;
        }
    }

    bool TargetInSkillRange()
    {
        if (characterStats != null && attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        }
        else
        {
            return false;
        }
    }

    void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;

        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(guardPoint.x + randomX, transform.position.y, guardPoint.z + randomZ);
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }

    public void Hit()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    public void EndNotify()
    {
        Debug.Log("进入EndNotify");
        // 播放胜利动画
        animator.SetBool("win", true);

        isPlayerDead = true;

        // 停止动作
        isChase = false;
        isWalk = false;
        attackTarget = null;
    }
}
