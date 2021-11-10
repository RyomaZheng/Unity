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


            // ���򹥻�Ŀ��
            //transform.LookAt(attackTarget.transform);

            // ��ȡ����
            Vector3 direction = attackTarget.transform.position - transform.position;

            // ����
            direction.Normalize();

            var targetStats = attackTarget.GetComponent<CharacterStats>();
            // ֹͣĿ����ƶ�
            targetStats.GetComponent<NavMeshAgent>().isStopped = true;
            targetStats.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            targetStats.GetComponent<Animator>().SetTrigger("dizzy");

            // �����˺�
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    // Animation Event
    public void ThrowRock()
    {
        // targetΪ�յĻ�����ȡĿ��
        if (attackTarget == null)
        {
            attackTarget = FindObjectOfType<PlayerController>().gameObject;
        }

        if (attackTarget != null)
        {
            // ����ʯͷ
            var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);

            // ����Ŀ��
            rock.GetComponent<Rock>().target = attackTarget;
        }

    }
}
