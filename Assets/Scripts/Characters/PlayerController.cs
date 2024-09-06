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
        //死亡后广播
        if (isDead)
        {
            GameManager.Instance.NotifyObservers();
        }

        lastAttackTime -= Time.deltaTime;

    }

    private void SwitchAnimation()
    {
        //sqrMagnitude将vector3转化为float
        animator.SetFloat("Speed",agent.velocity.sqrMagnitude);
        animator.SetBool("Death",isDead);
    }

    private void Start()
    {
        //将函数注册到事件
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClick += EventAttack;

        GameManager.Instance.RigisterPlayer(characterStats);
    }

    public void MoveToTarget(Vector3 target)
    {
        //终止攻击
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

        //转向攻击目标
        transform.LookAt(attackTarget.transform);

        //不同武器设置攻击距离
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStats.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;

        //攻击
        if (lastAttackTime < 0)
        {
            animator.SetTrigger("Attack");
            //暴击判断
            characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
            animator.SetBool("Critical",characterStats.isCritical);
            //充值冷却时间
            lastAttackTime = characterStats.attackData.coolDown;
        }
    }

    //动画中的攻击判定
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

