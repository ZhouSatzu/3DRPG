using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates {GUARD,PATROL,CHASE,DEAD}

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;

    private EnemyStates enemyStates;

    private GameObject attackTarget;

    private Animator animator;

    private float speed;

    [Header("Basic Settings")]
    public float sightRadius;
    public bool isGuard;
    public float lookAtTime;
    private float remainLookAtTime;

    [Header("Patrol State")]
    public float patrolRange;

    private Vector3 wayPoint;
    private Vector3 BirthPos;

    //bool配合动画
    bool isWalk;
    bool isChase;
    bool isFollow;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        speed = agent.speed;
        BirthPos = transform.position;
        remainLookAtTime = lookAtTime;
    }

    private void Start()
    {
        if(isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates=EnemyStates.PATROL;
            GetNewPoint();
        }
    }

    private void Update()
    {
        SwitchStates();
        SwitchAnimations();
    }

    void SwitchAnimations()
    {
        animator.SetBool("Walk",isWalk);
        animator.SetBool("Chase",isChase);
        animator.SetBool("Follow",isFollow);
    }
    
    void SwitchStates()
    {
        //如果发现player切换到chase;
        if(FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
            //Debug.Log("foundplayer!");
        }

        switch(enemyStates)
        {
            case EnemyStates.GUARD:
                break;
            case EnemyStates.PATROL:
                EnemyPatrol();
                break;
            case EnemyStates.CHASE:
                EnemyChase();
                break;
            case EnemyStates.DEAD:
                break;
        }
    }

    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                attackTarget = collider.gameObject;
                return true;
            }
        }

        attackTarget = null;
        return false;
    }

    void EnemyPatrol()
    {
        isChase = false;
        agent.speed = speed * 0.5f;

        //判断是否到了随机巡逻点
        if(Vector3.Distance(wayPoint, transform.position)<=agent.stoppingDistance)
        {
            isWalk = false;
            
            if(remainLookAtTime>0)
                remainLookAtTime -= Time.deltaTime;
            else
                GetNewPoint();
        }
        else
        {
            isWalk = true;
            agent.destination = wayPoint;
        }
    }
    void EnemyChase()
    {
        isWalk = false;
        isChase = true;

        agent.speed = speed;
        if (!FoundPlayer())
        {
            //回到初始状态
            isFollow = false;
            if (remainLookAtTime > 0)
            {
                agent.destination = transform.position;
                remainLookAtTime -= Time.deltaTime;
            }
            else if (isGuard)
                enemyStates = EnemyStates.GUARD;
            else
                enemyStates = EnemyStates.PATROL;
        }
        else
        {
            isFollow = true;
            agent.destination = attackTarget.transform.position;
        }
    }

    private void GetNewPoint()
    {
        //还原等待的时间
        remainLookAtTime = lookAtTime;
        
        float randomX = Random.Range(-patrolRange,patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(BirthPos.x + randomX, transform.position.y, BirthPos.z + randomZ);

        //防止走到not walkable的点
        NavMeshHit hit;
        //1:areaMask,A mask that specifies the NavMesh areas allowed when finding the nearest point. 此时指的是walkable
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
}
