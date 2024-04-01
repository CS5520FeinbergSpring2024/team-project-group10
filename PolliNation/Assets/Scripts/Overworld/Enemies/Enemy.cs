
using System;
using System.Numerics;
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
    protected UnityEngine.Vector3 startingPosition;
    protected UnityEngine.Vector3 roamingPosition;
    protected GameObject bee;
    private Rigidbody rigidBody;
    private float waspHeight = 2;

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
        float distance = UnityEngine.Vector3.Distance(transform.position, bee.transform.position);

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
    /// Generates a random position to use as a roaming target for an enemy. 
    /// </summary>
    /// <returns> a random target roaming position for the enemy </returns>
    private UnityEngine.Vector3 GetRoamingPosition() 
    {
        UnityEngine.Vector3 targetVector;
        UnityEngine.Vector3 targetPosition;
        int loopCounter = 0;
        do 
        {
            targetVector = RandomTargetVector();
            loopCounter += 1;
            targetPosition = startingPosition + targetVector;
            if (loopCounter > 10000) {
                Debug.Log("Broke GetRoamingPositionLoop");
                targetPosition = new UnityEngine.Vector3(25,0,25);
                break;
            }
        } 
        while (!CheckPositionInBounds(startingPosition, targetPosition));
        return targetPosition;
    }

    /// <summary>
    /// Method to get a random vector of wasp patrol range
    /// </summary>
    private UnityEngine.Vector3 RandomTargetVector()
    {   
        // find point on circumference of circle with radius of patrol range
        float x,z, zSign;
        x = UnityEngine.Random.Range(-patrolRange, patrolRange);
        z = (float) Math.Sqrt(Math.Pow(patrolRange,2) - Math.Pow(x,2));
        zSign = UnityEngine.Random.Range(0,2)*2 -1;
        z *= zSign;
        return new UnityEngine.Vector3(x,waspHeight,z);
    }

    /// <summary>
    /// Check if path between two points intersects a boundary wall
    /// </summary>
    /// <returns> True 
    protected Boolean CheckPositionInBounds(UnityEngine.Vector3 startPos, UnityEngine.Vector3 endPos){
        if (Physics.Linecast(startPos, endPos, out RaycastHit hitInfo))
        {
            if (hitInfo.transform.CompareTag("Meadow_Boundary")) {
                return false;
            }
        }
        return true;
    }


    /// <summary>
    /// Roaming state behvaior - enemy will roam back and forth of patrol route.
    /// </summary>
    private protected virtual void Roaming()
    {
        
        //move and rotate wasp towards roamingPosition
        rigidBody.position = UnityEngine.Vector3.MoveTowards(rigidBody.position, roamingPosition, Time.fixedDeltaTime * speed);
        // if wasp gets to roamingPosition turn around and loop
        // check before setting rotation to avoid LookRotation zero error
        if (UnityEngine.Vector3.Distance(rigidBody.position, roamingPosition) <= 0.1f)
        {   
            roamingPosition = startingPosition;
            startingPosition = rigidBody.position;
        } 
        transform.rotation = UnityEngine.Quaternion.Lerp(rigidBody.rotation, 
        UnityEngine.Quaternion.LookRotation(roamingPosition - rigidBody.position), Time.fixedDeltaTime * speed);
    
    }

    /// <summary>
    /// Chase state behavior - enemy will chase the bee.
    /// </summary>
    private protected virtual void Chase()
    {
        // chase bee
        rigidBody.position = UnityEngine.Vector3.MoveTowards(rigidBody.position, 
        new UnityEngine.Vector3(bee.transform.position.x, waspHeight ,bee.transform.position.z), Time.fixedDeltaTime * speed);
        transform.rotation = UnityEngine.Quaternion.Slerp(rigidBody.rotation, 
        UnityEngine.Quaternion.LookRotation(bee.transform.position - rigidBody.position), Time.fixedDeltaTime * speed);
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
    /// Attack Mode behavior - enemy will continue chasing bee and attack. 
    /// </summary>
    private protected virtual void Attack()
    {
        // continue chasing bee
        rigidBody.position = UnityEngine.Vector3.MoveTowards(rigidBody.position, 
        new UnityEngine.Vector3(bee.transform.position.x, waspHeight, bee.transform.position.z),
         Time.fixedDeltaTime * speed);
        transform.rotation = UnityEngine.Quaternion.Slerp(rigidBody.rotation, 
        UnityEngine.Quaternion.LookRotation(bee.transform.position - rigidBody.position), Time.fixedDeltaTime * speed);

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
        // Get bee height to set wasp height slightly above
        waspHeight = bee.transform.position.y + 2;
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
        transform.localEulerAngles = new UnityEngine.Vector3(0,transform.localEulerAngles.y,0);
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