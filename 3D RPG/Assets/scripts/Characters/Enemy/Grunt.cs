using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : EnemyController
{
    [Header("Skill")]
    public float kickForce = 15;

    // animation方法
    public void KickOff()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            // 面向攻击目标
            transform.LookAt(attackTarget.transform);

            // 获取方向
            Vector3 direction = attackTarget.transform.position - transform.position;

            // 量化
            direction.Normalize();

            // 停止目标的移动
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("dizzy");
        }
    }
}
