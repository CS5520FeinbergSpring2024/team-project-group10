using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UIElements;

public class Wasp : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private int speed = 3;
    private float chaseRange = 7f;
    private float damageRange = 2f;
    private float prevAttackTime = -1f;
    private float attackCooldown = 1f;
    private Vector3 startingPosition;
    private Vector3 roamingPosition;
    private GameObject bee;


    private enum State 
    {
        Roaming,
        Chase,
        Attack,
        Stuck,
    }

    private State state;

    void Awake()
    {
       bee = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        state = State.Roaming;
        // get the wasps origin position and set target position for patrol path
        startingPosition = transform.position;
        roamingPosition = GetRoamingPosition();
    }

    private Vector3 GetRoamingPosition() {
        Vector3 randomvector = RandomTargetVector();
        return startingPosition + randomvector;
    }

    private Vector3 RandomTargetVector()
    {   
        // initialize loop variables
        float x,z;
        float pathRange = 3;
        float minAxisTravel = 1;

        do 
        {
            x = UnityEngine.Random.Range(-pathRange, pathRange);
            z = UnityEngine.Random.Range(-pathRange, pathRange);
        } while(Math.Abs(x) < minAxisTravel && Math.Abs(z) < minAxisTravel);
            
        
        return new Vector3(x,0,z);
    }

    void Update()
    {   
        //Debug.Log("Start position: " + startingPosition);
        //Debug.Log("Roaming Position: " + roamingPosition);
        switch (state) {
        default:
        case State.Roaming:
    
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
        case State.Stuck:
            Debug.Log("in stuck case");
            
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
        } else if(distance == 5000) { //((Time.time > waspStartTime + 3 ) 
        //&& (Math.Abs(gameObject.GetComponent<Rigidbody>().velocity.x) <= 0.1f 
        //|| Math.Abs(gameObject.GetComponent<Rigidbody>().velocity.z) <= 0.1f)) {
            state = State.Stuck;
        } else {
            state = State.Roaming;
        }
    }

    void LateUpdate()
    {
        //prevent wasp from rotation on x and z axis on collisions
        transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0);
    }


    
    //if wasp hits the boundary walls
    void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Boundary")) {
            //Debug.Log("wall position: " + other.transform.position);
            Debug.Log("Wasp position: " + transform.position);
            //int signWallx = Math.Sign(other.transform.position.x);
            //int signWallz = Math.Sign(other.transform.position.z);
            //other.transform.position
            //Debug.Log("detected boundary wall hit");
            //roamingPosition = startingPosition;
            //Debug.Log("new roaming: " + roamingPosition);
            //transform.position += new Vector3(-signWallx*1f,0,-signWallz*1f);
            //transform.Rotate(0,-180,0);
            //    roamingPosition = startingPosition;
                //startingPosition = transform.position;

            // rotate 180, set new starting position and roaming position
            //transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, roamingPosition, Time.deltaTime * speed), 
            //Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(roamingPosition - transform.position), Time.deltaTime));
        }
    }
    

}
