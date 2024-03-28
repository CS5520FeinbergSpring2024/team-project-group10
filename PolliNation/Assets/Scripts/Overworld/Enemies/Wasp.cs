using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Wasp : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private int speed = 4;
    private Vector3 startingPosition;

    private float pathRange = 10f;

    private float chaseRange = 7f;
    private float damageRange = 2f;
    private float prevAttackTime = -1f;
    private float attackCooldown = 1f;
    private float minDistance = 5;
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
        float x = UnityEngine.Random.Range(-pathRange, pathRange);
        float z = UnityEngine.Random.Range(-pathRange, pathRange);
        // generate new random numbers until one of the axis is greater than min distance
        while (Math.Abs(x) < minDistance && Math.Abs(z) < minDistance) {
            x = UnityEngine.Random.Range(-pathRange, pathRange);
            z = UnityEngine.Random.Range(-pathRange, pathRange);
        }

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
        //prevent wasp from rotation on x and z axis on collisions
        transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0);
    }

}
