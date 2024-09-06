using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private CharacterStats characterStats;

    private GameObject attackTarget;
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

    private void Update()
    {
        SwitchAnimation();

        isDead = characterStats.CurrentHealth <= 0;
        //������㲥
        if (isDead)
        {
            GameManager.Instance.NotifyObservers();
        }

        lastAttackTime -= Time.deltaTime;

    }

    private void SwitchAnimation()
    {
        //sqrMagnitude��vector3ת��Ϊfloat
        animator.SetFloat("Speed",agent.velocity.sqrMagnitude);
        animator.SetBool("Death",isDead);
    }

    private void Start()
    {
        //������ע�ᵽ�¼�
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClick += EventAttack;

        GameManager.Instance.RigisterPlayer(characterStats);
    }

    public void MoveToTarget(Vector3 target)
    {
        //��ֹ����
        StopAllCoroutines();

        if(isDead) 
            return;

        agent.stoppingDistance = stopDistance;

        agent.isStopped = false;
        agent.destination = target; 
    }
    
    private void EventAttack(GameObject target)
    {
        if(isDead)
            return;
        
        if (target != null)
        {
            attackTarget = target;
            StartCoroutine(MoveToAttackTarget());
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        agent.stoppingDistance = characterStats.attackData.attackRange;

        //ת�򹥻�Ŀ��
        transform.LookAt(attackTarget.transform);

        //��ͬ�������ù�������
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStats.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;

        //����
        if (lastAttackTime < 0)
        {
            animator.SetTrigger("Attack");
            //�����ж�
            characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
            animator.SetBool("Critical",characterStats.isCritical);
            //��ֵ��ȴʱ��
            lastAttackTime = characterStats.attackData.coolDown;
        }
    }

    //�����еĹ����ж�
    void Hit()
    {
        if (attackTarget.CompareTag("Attackable"))
        {
            if(attackTarget.GetComponent<Rock>())
            { 
                attackTarget.GetComponent<Rock>().rockState = Rock.RockStates.HitEnemy;
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce((transform.forward) * 20.0f,ForceMode.Impulse);
            }
        }
        else
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }
}

