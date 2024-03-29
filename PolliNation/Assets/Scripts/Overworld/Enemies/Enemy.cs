using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
    protected float pathRange;
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
    float enemyAttackRange = 2.5f,
    float enemyAttackCooldown = 1, 
    float enemyPathRange = 5)
    {
        damage = enemyDamage;
        speed = enemySpeed;
        chaseRange = enemyChaseRange;
        attackRange = enemyAttackRange;
        attackCooldown = enemyAttackCooldown;
        pathRange = enemyPathRange;
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
        Vector3 targetVector = RandomTargetVector();
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
            x = UnityEngine.Random.Range(-pathRange, pathRange);
            z = UnityEngine.Random.Range(-pathRange, pathRange);
            loopCounter += 1;
            // may not be possible to meet conditions to break loop
            if (loopCounter > 1000) {
                Debug.Log("Broke RandomTargetVectorloop");
                break;
            }
        } while(Math.Abs(x) < minAxisTravel && Math.Abs(z) < minAxisTravel);
        
        return new Vector3(x,0,z);
    }


    /// <summary>
    /// Method called when bee is in roaming state.
    /// </summary>
    private protected virtual void Roaming()
    {
        //move and rotate wasp towards roamingPosition
        //transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, roamingPosition, Time.deltaTime * speed), 
        //Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(roamingPosition - transform.position), Time.deltaTime));


        Debug.Log("pathRange: " + pathRange);
        transform.SetPositionAndRotation(Vector3.MoveTowards(rigidBody.position, roamingPosition, Time.deltaTime * speed), 
        Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(roamingPosition - rigidBody.position), Time.deltaTime));
        // if wasp gets to roamingPosition turn around and loop
        if (Vector3.Distance(rigidBody.position, roamingPosition) <= 0.01f)
        {   
            roamingPosition = startingPosition;
            startingPosition = rigidBody.position;
        }
        
    }

    /// <summary>
    /// Method called when bee is in Chase state.
    /// </summary>
    private protected virtual void Chase()
    {
        // chase bee
        transform.SetPositionAndRotation(Vector3.MoveTowards(rigidBody.position, bee.transform.position, Time.deltaTime * speed), 
        Quaternion.Slerp(rigidBody.rotation, Quaternion.LookRotation (bee.transform.position - rigidBody.position), Time.deltaTime)); 
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
        transform.SetPositionAndRotation(Vector3.MoveTowards(rigidBody.position, bee.transform.position, Time.deltaTime * speed), 
        Quaternion.Slerp(rigidBody.rotation, Quaternion.LookRotation (bee.transform.position - rigidBody.position), Time.deltaTime));
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

    // Unity lifecycle methods

    protected virtual void Awake()
    {
        // find bee game object 
        bee = GameObject.Find("Overworld_Bee");
        // gets rigidbody component from gameObject script is attached to
        rigidBody = gameObject.GetComponent<Rigidbody>();
        if (bee == null)
        {
            Debug.Log("Bee not found");
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
            //Debug.Log("wall position: " + other.transform.position);
            Debug.Log("Wasp position: " + transform.position);
        } else if (other.name ==  "Overworld_Bee")
        {
            Debug.Log("hit bee");
        }
    }
    
}