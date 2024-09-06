using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates {GUARD,PATROL,CHASE,DEAD}

[RequireComponent(typeof(NavMeshAgent), typeof(CharacterStats))]

public class EnemyController : MonoBehaviour, IEndGameObserver
{
    private NavMeshAgent agent;
    protected CharacterStats characterStats;
    private EnemyStates enemyStates;
    private Collider coll;

    protected GameObject attackTarget;

    private Animator animator;

    private float speed;
    private float lastAttackTime;

    [Header("Basic Settings")]
    public float sightRadius;
    public bool isGuard;
    public float lookAtTime;
    private float remainLookAtTime;

    [Header("Patrol State")]
    public float patrolRange;

    private Vector3 wayPoint;
    private Vector3 birthPos;
    //����ʱ��ת�Ƕ�
    private Quaternion birthRotation;

    //bool��϶���
    bool isWalk;
    bool isChase;
    bool isFollow;
    bool isDead;
    bool playerDead;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        coll = GetComponent<Collider>();

        speed = agent.speed;
        birthPos = transform.position;
        birthRotation = transform.rotation;
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

        //TODO:�����л����޸�
        GameManager.Instance.AddObserver(this);
    }

    private void Update()
    {
        if(characterStats.CurrentHealth <= 0)
            isDead = true;
        
        if(!playerDead)
        { 
            SwitchStates();
            SwitchAnimations();
            lastAttackTime -= Time.deltaTime;
        }        
    }

    private void OnEnable()
    {
        //GameManager.Instance.AddObserver(this);
    }

    private void OnDisable()
    {
        if (!GameManager.IsInitialized)
            return;
        GameManager.Instance.RemoveObserver(this);
    }

    void SwitchAnimations()
    {
        animator.SetBool("Walk",isWalk);
        animator.SetBool("Chase",isChase);
        animator.SetBool("Follow",isFollow);
        animator.SetBool("Death", isDead);
    }
    
    void SwitchStates()
    {
        if (isDead)
            enemyStates = EnemyStates.DEAD;          
        //�������player�л���chase;
        else if(FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
            //Debug.Log("foundplayer!");
        }

        switch(enemyStates)
        {
            case EnemyStates.GUARD:
                EnemyGuard();
                break;
            case EnemyStates.PATROL:
                EnemyPatrol();
                break;
            case EnemyStates.CHASE:
                EnemyChase();
                break;
            case EnemyStates.DEAD:
                EnemyDead();
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

    bool TargetInAttackRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        else return false;
    }

    bool TargetInSkillRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        else return false;
    }

    void EnemyGuard()
    {
        isChase = false;

        //�ص�������
        if(transform.position != birthPos)
        {
            isWalk = true;
            agent.isStopped = false;
            agent.destination = birthPos;

            if (Vector3.Distance(birthPos, transform.position) <= agent.stoppingDistance)
            {
                //Debug.Log("111");
                isWalk = false;
                //�ص�����ʱ�ĽǶ� Lerp�������ǻ���ת��ԭʼ�Ƕ� ����ָ��ת���ٶ�
                transform.rotation = Quaternion.Lerp(transform.rotation, birthRotation, 0.01f);
            }
        }
    }

    void EnemyPatrol()
    {
        isChase = false;
        agent.speed = speed * 0.5f;

        //�ж��Ƿ������Ѳ�ߵ�
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
            //�ص���ʼ״̬
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
            agent.isStopped = false;
            agent.destination = attackTarget.transform.position;
        }
        //�ڹ�����Χ���򹥻�
        if (TargetInAttackRange() || TargetInSkillRange()) 
        {
            isFollow = false;
            agent.isStopped = true;

            //������ȴ
            if(lastAttackTime < 0)
            {
                lastAttackTime = characterStats.attackData.coolDown;

                //�����ж�
                characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;

                //ִ�й���
                Attack();
            }
        }
    }

    void EnemyDead()
    {
        coll.enabled = false;
        agent.radius = 0;
        //�ӳ�2s��
        Destroy(gameObject, 2f);
    }

    void Attack()
    {
        transform.LookAt(attackTarget.transform.position);

        if (TargetInAttackRange())
        {
            //����������
            animator.SetTrigger("Attack");
            animator.SetBool("Critical", characterStats.isCritical);
        }
        if (TargetInSkillRange())
        {
            //Զ�̼��ܹ�������
            animator.SetTrigger("Skill");
        }
    }


    //����̸֮�������˰�HIT��ӵ���Ӧ����֡���ϣ�����
    void Hit()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    private void GetNewPoint()
    {
        //��ԭ�ȴ���ʱ��
        remainLookAtTime = lookAtTime;
        
        float randomX = Random.Range(-patrolRange,patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(birthPos.x + randomX, transform.position.y, birthPos.z + randomZ);

        //��ֹ�ߵ�not walkable�ĵ�
        NavMeshHit hit;
        //1:areaMask,A mask that specifies the NavMesh areas allowed when finding the nearest point. ��ʱָ����walkable
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }

    public void EndNotify()
    {
        //����ʤ������
        animator.SetBool("Win", true);
        playerDead = true;

        isChase = false;
        isWalk = false;
        attackTarget = null;
    }
}
