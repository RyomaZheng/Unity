using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    [Header("Skill")]
    public float kickForce = 30;

    public GameObject rockPrefab;

    public Transform handPos;

    // Animation Event
    public void KickOff()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {


            // 面向攻击目标
            //transform.LookAt(attackTarget.transform);

            // 获取方向
            Vector3 direction = attackTarget.transform.position - transform.position;

            // 量化
            direction.Normalize();

            var targetStats = attackTarget.GetComponent<CharacterStats>();
            // 停止目标的移动
            targetStats.GetComponent<NavMeshAgent>().isStopped = true;
            targetStats.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            targetStats.GetComponent<Animator>().SetTrigger("dizzy");

            // 计算伤害
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    // Animation Event
    public void ThrowRock()
    {
        // target为空的话，获取目标
        if (attackTarget == null)
        {
            attackTarget = FindObjectOfType<PlayerController>().gameObject;
        }

        if (attackTarget != null)
        {
            // 生成石头
            var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);

            // 设置目标
            rock.GetComponent<Rock>().target = attackTarget;
        }

    }
}
