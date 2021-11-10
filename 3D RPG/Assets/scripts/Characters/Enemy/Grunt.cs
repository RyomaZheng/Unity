using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : EnemyController
{
    [Header("Skill")]
    public float kickForce = 15;

    // animation����
    public void KickOff()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            // ���򹥻�Ŀ��
            transform.LookAt(attackTarget.transform);

            // ��ȡ����
            Vector3 direction = attackTarget.transform.position - transform.position;

            // ����
            direction.Normalize();

            // ֹͣĿ����ƶ�
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("dizzy");
        }
    }
}
