using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;

    private Animator animator;

    private GameObject attackTarget;

    private CharacterStats characterStats;

    private float lastAttackTime;

    private bool isDead;

    private float stopDistance;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();

        stopDistance = agent.stoppingDistance;
    }

    private void OnEnable()
    {
        // 添加事件
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
        GameManager.Instance.RigisterPlayer(characterStats);
    }

    private void OnDisable()
    {
        if (!MouseManager.isInitialized)
        {
            return;
        }
        // 移除事件
        MouseManager.Instance.OnMouseClicked -= MoveToTarget;
        MouseManager.Instance.OnEnemyClicked -= EventAttack;
    }

    // Update is called once per frame
    private void Start()
    {
        SaveManager.Instance.LoadPlayerData();
    }

    private void Update()
    {
        isDead = characterStats.CurrentHealth == 0;

        if (isDead)
        {
            GameManager.Instance.NotifyObservers();
        }

        SwitchAnimation();

        lastAttackTime -= Time.deltaTime;
    }

    public void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        if (isDead) return;

        // 恢复原样stopDistance
        agent.stoppingDistance = stopDistance;

        // agent设置行动开启
        agent.isStopped = false;
        // 移动到目标点
        agent.destination = target;
    }

    private void SwitchAnimation()
    {
        animator.SetFloat("speed", agent.velocity.sqrMagnitude);
        animator.SetBool("dead", isDead);
    }

    private void EventAttack(GameObject target)
    {
        if (isDead) return;
        // 判断攻击目标不为空
        if (target != null)
        {
            // 设置攻击目标
            attackTarget = target;

            // 计算暴击
            characterStats.isCritical = UnityEngine.Random.value <= characterStats.attackData.criticalChance;

            //开启移动攻击协程
            StartCoroutine(MoveToAttackTarget());
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        // agent设置行动开启
        agent.isStopped = false;
        // 变更为攻击范围
        agent.stoppingDistance = characterStats.attackData.attackRange;

        // player面向敌人
        transform.LookAt(attackTarget.transform);

        // 判断敌人位置和player位置是否大于攻击距离，是的话一直往敌人位置移动
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStats.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        // 到了后停止移动，agent设置行动停止
        agent.isStopped = true;

        // 攻击冷却时间完成后，进行攻击动画
        if (lastAttackTime <= 0)
        {
            animator.SetBool("critical", characterStats.isCritical);
            animator.SetTrigger("attack");
            AudioManager.Instance.PlayMusicByName("Effect/weapon", false);
        }

        // 重置攻击冷却时间
        lastAttackTime = characterStats.attackData.coolDown;
    }

    public void Hit()
    {
        if (attackTarget.CompareTag("Attackable"))
        {
            if (attackTarget.GetComponent<Rock>())
            {
                attackTarget.GetComponent<Rock>().rockStates = Rock.RockStates.HitEnemy;

                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
            }

        }
        else
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            AudioManager.Instance.PlayMusicByName("Enemy/Slime_GetHit", false);
            targetStats.TakeDamage(characterStats, targetStats);
        }

    }

    public void FootR(AudioClip obj)
    {
        AudioManager.Instance.PlayMusic(obj, "Effect", false);
    }

    public void FootL(AudioClip obj)
    {
        AudioManager.Instance.PlayMusic(obj, "Effect", false);
    }
}
