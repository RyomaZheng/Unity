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
        // ����¼�
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
        // �Ƴ��¼�
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

        // �ָ�ԭ��stopDistance
        agent.stoppingDistance = stopDistance;

        // agent�����ж�����
        agent.isStopped = false;
        // �ƶ���Ŀ���
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
        // �жϹ���Ŀ�겻Ϊ��
        if (target != null)
        {
            // ���ù���Ŀ��
            attackTarget = target;

            // ���㱩��
            characterStats.isCritical = UnityEngine.Random.value <= characterStats.attackData.criticalChance;

            //�����ƶ�����Э��
            StartCoroutine(MoveToAttackTarget());
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        // agent�����ж�����
        agent.isStopped = false;
        // ���Ϊ������Χ
        agent.stoppingDistance = characterStats.attackData.attackRange;

        // player�������
        transform.LookAt(attackTarget.transform);

        // �жϵ���λ�ú�playerλ���Ƿ���ڹ������룬�ǵĻ�һֱ������λ���ƶ�
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStats.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        // ���˺�ֹͣ�ƶ���agent�����ж�ֹͣ
        agent.isStopped = true;

        // ������ȴʱ����ɺ󣬽��й�������
        if (lastAttackTime <= 0)
        {
            animator.SetBool("critical", characterStats.isCritical);
            animator.SetTrigger("attack");
            AudioManager.Instance.PlayMusicByName("Effect/weapon", false);
        }

        // ���ù�����ȴʱ��
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
}
