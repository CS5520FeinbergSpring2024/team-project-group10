using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Wasp : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    //public BeeHealth beeHealth;
    private Vector3 startingPosition;

    // range for random vector generation 
    private float min = 5;
    private float max = 10;
    private int speed = 4;
    private float chaseRange = 7f;
    private float damageRange = 2f;
    private Vector3 roamingPosition;
    private GameObject bee;

 
    private enum State 
    {
        Roaming,
        Chase,
        Attack,
    }

    private State state;

    void Awake()
    {
       state = State.Roaming; 
       bee = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        // get the wasps origin position
        startingPosition = transform.position;
        roamingPosition = GetRoamingPosition();
    }

    private Vector3 GetRoamingPosition() {
        Vector3 randomvector = RandomEnemyMovementVector();
        return startingPosition + randomvector;
    }

    private Vector3 RandomEnemyMovementVector()
    {   
        // get random x and z values within specified range
        float x = Random.Range(min, max);
        float z = Random.Range(min, max);
        // keep same height
        float y = startingPosition.y;
        return new Vector3(x,y,z);
    }

    void Update()
    {   
        switch (state) {
        default:
        case State.Roaming:
            //move and rotate wasp towards roamingPosition
            transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, roamingPosition, Time.deltaTime * speed), 
            Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (roamingPosition - transform.position), Time.deltaTime));
            // if bee gets to roamingPosition have bee move to new roaming position
            if (Vector3.Distance(transform.position, roamingPosition) == 0)
            {   
                roamingPosition = startingPosition;
                startingPosition = transform.position;
            }
            //Check whether to change state
            ChangeState();
            break;
        case State.Chase:
            // chase bee
            transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, bee.transform.position, Time.deltaTime * speed), 
            Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (bee.transform.position - transform.position), Time.deltaTime));
            ChangeState();
            break;
        case State.Attack:
            Debug.Log("Health: " + bee.GetComponent<BeeHealth>());
            bee.GetComponent<BeeHealth>().TakeDamage(damage);
            ChangeState();
            break;
        }
        
    }

    // method to check distance from Bee and change state to chase if within range
    private void ChangeState() 
    {
        float distance = Vector3.Distance(transform.position, bee.transform.position);

        if (damageRange < distance && distance <= chaseRange)
        {
            state = State.Chase;
        } else if (distance <= damageRange) {
            state = State.Attack;
        } else {
            state = State.Roaming;
        }
    }

    void LateUpdate()
    {
        //prevent wasp from rotation on x and z axis
        transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0);
    }

}
