using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Wasp : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    public BeeHealth beeHealth;
    private Vector3 startingPosition;

    // range for random vector generation 
    private float min = 5;
    private float max = 20;
    private int speed = 1;
    private Vector3 roamingPosition;
    public Rigidbody rigidBody;

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
        // keep same height as origin
        float y = startingPosition.y;
        //float x = startingPosition.x;
        return new Vector3(x,y,z);
    }

    void Update()
    {   
        //move and rotate wasp towards roamingPosition
        transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, roamingPosition, Time.deltaTime * speed), 
        Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (roamingPosition - transform.position), Time.deltaTime));

        // if bee gets to roamingPosition have bee move to new roaming position
        if (Vector3.Distance(transform.position, roamingPosition) == 0)
        {
            roamingPosition = startingPosition;
            startingPosition = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            //beeHealth.TakeDamage(damage);
            Debug.Log("In OnTriggerEnter gameObject tag matched");

        }
    }



}
