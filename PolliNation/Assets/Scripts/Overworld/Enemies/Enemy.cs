using System;
using Palmmedia.ReportGenerator.Core;
using UnityEngine;

/// <summary>
/// Contains logic for Enemy classes.
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    protected int damage;
    protected int speed;
    protected int health;
    protected float chaseRange;
    protected float attackRange;
    protected float prevAttackTime;
    protected float attackCooldown;
    protected float patrolRange;
    protected Vector3 startingPosition;
    protected Vector3 roamingPosition;
    protected GameObject bee;
    private Rigidbody rigidBody;

    private enum State 
    {
        Roaming,
        Chase,
        Attack,
    }
    private State state;

    /// <summary>
    /// Method to set values for enemy stats.
    /// </summary>
    protected virtual void SetEnemyStats(int enemyDamage = 1, 
    int enemySpeed = 1, 
    float enemyChaseRange = 10,
    float enemyAttackCooldown = 1, 
    float enemyPatrolRange = 5,
    int enemyHealth = 10,
    float enemyAttackRange = 0)
    {
        damage = enemyDamage;
        speed = enemySpeed;
        chaseRange = enemyChaseRange;
        attackRange = enemyAttackRange;
        if (attackRange == 0 )
        {
            SetAttackRangeByCollider();
        }
        attackCooldown = enemyAttackCooldown;
        patrolRange = enemyPatrolRange;
        health = enemyHealth;
    }

    /// <summary>
    /// Method to set attack range of enemy equal to diameter of bee collider
    /// </summary>
    private void SetAttackRangeByCollider()
    {
        float beeColliderRadius = bee.GetComponent<SphereCollider>().radius; 
        if (beeColliderRadius != 0)
           {
            attackRange = 2 * beeColliderRadius;
           }
    }

    /// <summary>
    /// Method to change enemy state based on distance from player bee
    /// </summary>
    private protected void ChangeState() 
    {
        float distance = Vector3.Distance(transform.position, bee.transform.position);

        if (attackRange < distance && distance <= chaseRange)
        {
            state = State.Chase;
        } else if (distance <= attackRange) {
            state = State.Attack;
        } 
        else {
            state = State.Roaming;
        }
    }
    

    /// <summary>
    /// Method to set a target roaming position 
    /// </summary>
    private Vector3 GetRoamingPosition() 
    {
        Vector3 targetVector;
        int loopCounter = 0;
        do 
        {
            targetVector = RandomTargetVector();
            loopCounter += 1;
            if (loopCounter > 1000) {
                Debug.Log("Broke GetRoamingPositionLoop");
                break;
            }
        } while (!CheckPositionInBounds(startingPosition + targetVector));
        return startingPosition + targetVector;
    }

    /// <summary>
    /// Method to get a random vector of wasp patrol range
    /// </summary>
    private Vector3 RandomTargetVector()
    {   
        // find point on circumference of circle with radius of patrol range
        float x,z, xSign,zSign;
        x = UnityEngine.Random.Range(-patrolRange, patrolRange);
        xSign = UnityEngine.Random.Range(0,2)*2 -1;
        x *= xSign;
        z = (float) Math.Sqrt(Math.Pow(patrolRange,2) - Math.Pow(x,2));
        zSign = UnityEngine.Random.Range(0,2)*2 -1;
        z *= zSign;
        return new Vector3(x,0,z);
    }

    /// <summary>
    /// Method check if path hits a map boundary wall. 
    /// </summary>
    protected Boolean CheckPositionInBounds(UnityEngine.Vector3 position){
        if (Physics.Linecast(position, startingPosition, out RaycastHit hitInfo))
        {
            if (hitInfo.transform.CompareTag("Meadow_Boundary")) {
                return false;
            }
        }
        return true;
    }


    /// <summary>
    /// Method called when wasp is in roaming state.
    /// </summary>
    private protected virtual void Roaming()
    {
        
        //move and rotate wasp towards roamingPosition
        rigidBody.position = Vector3.MoveTowards(rigidBody.position, roamingPosition, Time.fixedDeltaTime * speed);
        // if wasp gets to roamingPosition turn around and loop
        // check before setting rotation to avoid LookRotation zero error
        if (Vector3.Distance(rigidBody.position, roamingPosition) <= 0.1f)
        {   
            roamingPosition = startingPosition;
            startingPosition = rigidBody.position;
        } 
        transform.rotation = Quaternion.Lerp(rigidBody.rotation, Quaternion.LookRotation(roamingPosition - rigidBody.position), Time.fixedDeltaTime * speed);
    
    }

    /// <summary>
    /// Method called when wasp is in Chase state.
    /// </summary>
    private protected virtual void Chase()
    {
        // chase bee
        rigidBody.position = Vector3.MoveTowards(rigidBody.position, bee.transform.position, Time.fixedDeltaTime * speed);
        transform.rotation = Quaternion.Slerp(rigidBody.rotation, Quaternion.LookRotation(bee.transform.position - rigidBody.position), Time.fixedDeltaTime * speed);
    }

    /// <summary>
    /// Method called when bee health reaches 0. 
    /// Bee health class returns player to hive when health hits 0,
    /// specific enemy classes can add other behaviors if desired.
    /// </summary>
    private protected virtual void OnKill()
    {
        Debug.Log("Bee killed");
    }

    /// <summary>
    /// Method called when enemy is in attack mode. 
    /// </summary>
    private protected virtual void Attack()
    {
        // continue chasing bee
        rigidBody.position = Vector3.MoveTowards(rigidBody.position, bee.transform.position, Time.fixedDeltaTime * speed);
        transform.rotation = Quaternion.Slerp(rigidBody.rotation, Quaternion.LookRotation(bee.transform.position - rigidBody.position), Time.fixedDeltaTime * speed);

        if (Time.time > prevAttackTime + attackCooldown)
        {
            // attack bee
            bee.GetComponent<BeeHealth>().TakeDamage(damage);
            //check if bee health hits 0 call OnKill method
            if (bee.GetComponent<BeeHealth>().Health == 0)
            {
                OnKill();
            }
            // set stored attack time
            prevAttackTime = Time.time;
        }
    }

    /////// Unity lifecycle methods ///////

    protected virtual void Awake()
    {
        // find bee game object 
        bee = GameObject.Find("Overworld_Bee");
        // gets rigidbody component from gameObject script is attached to
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    private protected void Start()
    {
        state = State.Roaming;
        // get the wasps spawn origin position and set target position for patrol path
        startingPosition = transform.position;
        roamingPosition = GetRoamingPosition();
        
    }

    private protected virtual void FixedUpdate()
    {   
        switch (state) {
        default:
        case State.Roaming:
            Roaming();
            ChangeState();
            break;
        case State.Chase:
            Chase();
            ChangeState();
            break;
        case State.Attack:
            Attack();
            ChangeState();
            break;
        }
    }

    private protected void LateUpdate()
    {
        //prevent wasp from rotation on x and z axis on collisions
        transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0);
    }
    
    // logging when wasps hit stuff for testing purposes 
    private protected void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Meadow_Boundary")) 
        {
            Debug.Log("Wasp hit wall");
        } else if (other.name ==  "Overworld_Bee")
        {
            Debug.Log("hit bee");
        }
    }
    
}