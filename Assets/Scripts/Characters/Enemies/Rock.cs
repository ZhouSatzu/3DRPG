using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockStates { HitPlayer, HitEnemy, HitNothing }
    
    private Rigidbody rb;
    public RockStates rockState;

    [Header("Basic Settings")]
    public float throwForce;
    public float hitForce;
    public GameObject target;
    public int damage;

    private Vector3 direction;
    public GameObject breakEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;
        rockState = RockStates.HitPlayer;
        FlyToTarget();
    }

    private void FixedUpdate()
    {
        if(rb.velocity.sqrMagnitude < 1f)
            rockState = RockStates.HitNothing;
    }

    public void FlyToTarget()
    {
        if(target == null)
            target = FindObjectOfType<PlayerController>().gameObject;
        direction = (target.transform.position - transform.position ).normalized;
        rb.AddForce(direction * throwForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        switch(rockState)
        {
            case RockStates.HitPlayer:
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = direction * hitForce;

                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage,other.gameObject.GetComponent<CharacterStats>());

                    rockState = RockStates.HitNothing;
                }
                break;
            
            case RockStates.HitEnemy:
                //只反击石头人
                if(other.gameObject.GetComponent<Golem>())
                {
                    var otherStats = other.gameObject.GetComponent<CharacterStats>();
                    otherStats.TakeDamage(damage, otherStats);
                    other.gameObject.GetComponent<Animator>().SetTrigger("Hurt");
                    Instantiate(breakEffect,transform.position,Quaternion.identity);
                    Destroy(gameObject);
                }
                break;
        }    
    }
}
