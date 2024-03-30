using System;
using UnityEngine;

public class EnemiesOld : MonoBehaviour
{
    
    [SerializeField] protected int damage
      {
        get{ return damage; }
        set{
            if (value <= 0) {
                return;
            } else {
                damage = value;
            }
        }
    }
    [SerializeField] protected int speed
      {
        get{ return speed; }
        set{
            if (value <= 0) {
                return;
            } else {
                speed = value;
            }
        }
    }
    [SerializeField] protected int health
      { 
        get{ return health; }
        set{
            if (value <= 0) {
                return;
            } else {
                chaseRange = value;
            }
        }
    }
    protected float chaseRange
    {
        get{ return chaseRange; }
        set{
            if (value <= 0) {
                return;
            } else {
                chaseRange = value;
            }
        }
    }
    protected float attackRange
    {
        get{ return attackRange; }
        set{
            if (value <= 0) {
                return;
            } else {
                attackRange = value;
            }
        }
    }
    protected float prevAttackTime;
    protected float attackCooldown
    {
        get{ return attackCooldown; }
        set{
            if (value < 0) {
                return;
            } else {
                attackCooldown = value;
            }
        }
    }
    protected float pathRange
      {
        get{ return pathRange; }
        set{
            if (value <= 0) {
                return;
            } else {
                pathRange = value;
            }
        }
    }
    protected Vector3 startingPosition;
    protected Vector3 roamingPosition;
    protected GameObject bee;

    private enum State 
    {
        Roaming,
        Chase,
        Attack,
    }
    private State state;

    //protected Enemy()
   // {
    //    SetEnemyStats(1, 1, 10, 2,1,5);
    //}

    /// <summary>
    /// Method to set values for enemy stats.
    /// </summary>
    protected virtual void SetEnemyStats(int enemyDamage = 1, 
    int enemySpeed = 1, 
    float enemyChaseRange = 10, 
    float enemyAttackRange = 2,
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
        } else {
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
            Debug.Log("x: " + x);
            Debug.Log("z: " + z);
            Debug.Log(pathRange);
            // may not be possible to meet conditions to break loop
            if (loopCounter > 1000) {
                Debug.Log("Broke RandomTargetVector loop");
                break;
            }
        } while(Math.Abs(x) < minAxisTravel && Math.Abs(z) < minAxisTravel);
        
        return new Vector3(x,0,z);
    }

    // Unity lifecycle methods

    protected virtual void Awake()
    {
        // find bee game object 
        bee = GameObject.FindWithTag("Player");
    }

    private protected void Start()
    {
        state = State.Roaming;
        // get the wasps spawn origin position and set target position for patrol path
        startingPosition = transform.position;
        Debug.Log("starting position: " + startingPosition);
        roamingPosition = GetRoamingPosition();
        Debug.Log("roaming position: " + roamingPosition);
    }

    private protected void Update()
    {   
        switch (state) {
        default:
        case State.Roaming:
            //transform.position = new UnityEngine.Vector3(10f,0,10f);
            
            //move and rotate wasp towards roamingPosition
            transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, roamingPosition, Time.deltaTime * speed), 
            Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(roamingPosition - transform.position), Time.deltaTime));
            // if wasp gets to roamingPosition turn around and loop
            if (Vector3.Distance(transform.position, roamingPosition) <= 0.01f)
            {   
                roamingPosition = startingPosition;
                startingPosition = transform.position;
            }
        
            ChangeState();
            break;
        case State.Chase:
            // chase bee
            transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, bee.transform.position, Time.deltaTime * speed), 
            Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (bee.transform.position - transform.position), Time.deltaTime));
            ChangeState();
            break;
        case State.Attack:
            // continue chasing bee
            transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, bee.transform.position, Time.deltaTime * speed), 
            Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (bee.transform.position - transform.position), Time.deltaTime));
            if (Time.time > prevAttackTime + attackCooldown)
            {
                // attack bee
                bee.GetComponent<BeeHealth>().TakeDamage(damage);
                // set stored attack time
                prevAttackTime = Time.time;
            }
            ChangeState();
            break;
        }
    }

    private protected void LateUpdate()
    {
        //prevent wasp from rotation on x and z axis on collisions
        transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0);
    }


    
    //if wasp hits the boundary walls
    private protected void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Meadow_Boundary")) {
            //Debug.Log("wall position: " + other.transform.position);
            Debug.Log("Wasp position: " + transform.position);
        } else if (other.CompareTag("Player"))
        {
            Debug.Log("hit bee");
        }
    }
    
}