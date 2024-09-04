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
        //sqrMagnitude��vector3ת��Ϊfloat
        animator.SetFloat("Speed",agent.velocity.sqrMagnitude);
    }

    private void Start()
    {
        //������ע�ᵽ�¼�
        MouseManager.instance.OnMouseClicked += MoveToTarget;
        MouseManager.instance.OnEnemyClick += EventAttack;
    }

    public void MoveToTarget(Vector3 target)
    {
        //��ֹ����
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

        //ת�򹥻�Ŀ��
        transform.LookAt(attackTarget.transform);

        //1��ָ�������� TODO:Ϊ��ͬ�������ù�������
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > 1)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;

        //����
        if (lastAttackTime < 0)
        {
            animator.SetTrigger("Attack");
            //��ֵ��ȴʱ��
            lastAttackTime = 0.5f;
        }

    }
}

