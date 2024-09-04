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
    private float lastAttackTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        SwitchAnimation();

        lastAttackTime -= Time.time;
    }

    private void SwitchAnimation()
    {
        //sqrMagnitude将vector3转化为float
        animator.SetFloat("Speed",agent.velocity.sqrMagnitude);
    }

    private void Start()
    {
        //将函数注册到事件
        MouseManager.instance.OnMouseClicked += MoveToTarget;
        MouseManager.instance.OnEnemyClick += EventAttack;
    }

    public void MoveToTarget(Vector3 target)
    {
        //终止攻击
        StopAllCoroutines();

        agent.isStopped = false;
        agent.destination = target; 
    }
    
    private void EventAttack(GameObject target)
    {
        if (target != null)
        {
            attackTarget = target;
            StartCoroutine(MoveToAttackTarget());
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;

        //转向攻击目标
        transform.LookAt(attackTarget.transform);

        //1代指攻击距离 TODO:为不同武器设置攻击距离
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > 1)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;

        //攻击
        if (lastAttackTime < 0)
        {
            animator.SetTrigger("Attack");
            //充值冷却时间
            lastAttackTime = 0.5f;
        }

    }
}

