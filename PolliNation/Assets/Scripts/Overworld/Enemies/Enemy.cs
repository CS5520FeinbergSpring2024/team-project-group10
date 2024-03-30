using System;
using System.Collections;
using Unity.VisualScripting;
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
    private float apothem;
    // temp high placeholder values
    float xAxisLimit = 1000;
    float zAxisLimit = 1000;

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
    float enemyAttackRange = 2.5f,
    float enemyAttackCooldown = 1, 
    float enemyPatrolRange = 5)
    {
        damage = enemyDamage;
        speed = enemySpeed;
        chaseRange = enemyChaseRange;
        attackRange = enemyAttackRange;
        attackCooldown = enemyAttackCooldown;
        patrolRange = enemyPatrolRange;
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
    /// Method to get a random vector within given wasp patrol ranges
    /// </summary>
    private Vector3 RandomTargetVector()
    {   
        // initialize loop variables
        float x,z;
        float minAxisTravel = 2;
        int loopCounter = 0;
        do 
        {
            x = UnityEngine.Random.Range(-patrolRange, patrolRange);
            z = UnityEngine.Random.Range(-patrolRange, patrolRange);
            loopCounter += 1;
            // may not be possible to meet conditions to break loop
            if (loopCounter > 1000) {
                Debug.Log("Broke RandomTargetVectorloop");
                break;
            }
        } while(Math.Abs(x) < minAxisTravel || Math.Abs(z) < minAxisTravel);
        
        return new Vector3(x,0,z);
    }

    protected Boolean CheckPositionInBounds(UnityEngine.Vector3 position){
        float difX = Math.Abs(position.x) - xAxisLimit;
        float difZ = Math.Abs(position.z) - zAxisLimit;
        if (difX >= 0 || difZ >= 0) {
            return false;
        }
        else {
            return true;
        }
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

    /// <summary>
    /// Method called wasp hits boundary wall to move it away
    ///  from boundary wall in case it gets stuck
    /// </summary>
    protected void CheckStuck(UnityEngine.Vector3 struckWall){
        Debug.Log("In stuck method");
        float difX = Math.Abs(roamingPosition.x) - Math.Abs(struckWall.x);
        float difZ = Math.Abs(roamingPosition.z) - Math.Abs(struckWall.z);
        if (difX > 0) {
            roamingPosition.x -= difX * 1.2f * Math.Sign(struckWall.x);
        }
        if (difZ > 0) {
            Debug.Log("Z coordinate too far");
            roamingPosition.z -= difZ * 1.2f * Math.Sign(struckWall.z);
        }
        
        Debug.Log("Wasp RP: " + roamingPosition);
        //rigidBody.position = Vector3.MoveTowards(rigidBody.position, roamingPosition, Time.fixedDeltaTime * speed);
        if (Vector3.Distance(rigidBody.position, roamingPosition) <= 0.1f)
        {   
            roamingPosition = startingPosition;
            startingPosition = rigidBody.position;
        } 
        //transform.position = roamingPosition;
        //roamingPosition = startingPosition;
        //startingPosition = rigidBody.position;
    }

    // Unity lifecycle methods

    protected virtual void Awake()
    {
        // find bee game object 
        bee = GameObject.Find("Overworld_Bee");
        // gets rigidbody component from gameObject script is attached to
        rigidBody = gameObject.GetComponent<Rigidbody>();
        if (bee == null)
        {
            Debug.Log("Bee object not found");
        }
        // get locations of all boundarys to map out max spawning distances on both axis
        GameObject[] boundaries = GameObject.FindGameObjectsWithTag("Meadow_Boundary");
        if (boundaries != null) {
            FindMapBoundaries(boundaries);
        }
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
        if (other.CompareTag("Meadow_Boundary")) {
            Debug.Log("Wasp hit wall");
            Debug.Log("Wasp RP: " + roamingPosition);
            Debug.Log("Wall Position: " + other.transform.position);
            //other.transform.position
            CheckStuck(other.transform.position);
    
        } else if (other.name ==  "Overworld_Bee")
        {
            Debug.Log("hit bee");
        }
    }

    /// <summary>
    /// Method to get bounds from boundary gameobjects surrounding hexagon shaped map
    /// </summary>
    private void FindMapBoundaries(GameObject[] boundaries) {
        

        foreach (GameObject boundary in boundaries) {
                // get position of wall
                UnityEngine.Vector3 wallPos = boundary.transform.position;
                // find min x and z positions of boundary walls
                if (Math.Abs(wallPos.x) < xAxisLimit && wallPos.x != 0)
                {   
                    xAxisLimit = Math.Abs(wallPos.x) ;
                }
                if (Math.Abs(wallPos.z) < zAxisLimit && wallPos.z != 0)
                {   
                    zAxisLimit = Math.Abs(wallPos.z) ;
                }
            }  
            
            if (xAxisLimit < zAxisLimit)
            {
                apothem = Mathf.Sqrt(3)/2 * xAxisLimit;
            } else {
                apothem = Mathf.Sqrt(3)/2 * zAxisLimit;
            }
    }
    
}