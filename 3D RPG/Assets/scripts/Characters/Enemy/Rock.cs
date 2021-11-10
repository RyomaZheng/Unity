using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockStates { HitPlayer, HitEnemy, HitNothing }

    private Rigidbody rb;

    public RockStates rockStates;

    [Header("Basic Settings")]
    public float force;
    public int damage;

    public GameObject target;

    private Vector3 direction;

    public GameObject breakEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;
        rockStates = RockStates.HitPlayer;
        FlyToTarget();
    }

    void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude < 1)
        {
            rockStates = RockStates.HitNothing;
        }
    }

    public void FlyToTarget()
    {
        // 设置方向
        direction = (target.transform.position - transform.position + Vector3.up).normalized;

        // 施加力
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (rockStates)
        {
            case RockStates.HitPlayer:
                HitPlayer(collision);
                break;
            case RockStates.HitEnemy:
                HitEnemy(collision);
                break;
        }
    }

    private void HitEnemy(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.GetComponent<Golem>())
        {
            var otherStats = other.GetComponent<CharacterStats>();
            otherStats.TakeDamage(damage, otherStats);

            // 生成碎片效果
            Instantiate(breakEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    private void HitPlayer(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            other.GetComponent<NavMeshAgent>().isStopped = true;
            other.GetComponent<NavMeshAgent>().velocity = direction * force;

            other.GetComponent<Animator>().SetTrigger("dizzy");

            other.GetComponent<CharacterStats>().TakeDamage(damage, other.GetComponent<CharacterStats>());

            rockStates = RockStates.HitNothing;
        }
    }
}
