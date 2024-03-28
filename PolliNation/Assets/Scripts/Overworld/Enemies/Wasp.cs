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
        //Debug.Log("random vector: " + randomvector);
        return startingPosition + randomvector;
    }

    private Vector3 RandomEnemyMovementVector()
    {   
        // get random x and z values within specified range
        //float x = Random.Range(min, max);
        float z = Random.Range(min, max);
        // keep same height as origin
        float y = startingPosition.y;
        float x = startingPosition.x;
        return new Vector3(x,y,z);
    }

    void Update()
    {   Debug.Log("Current Position " + transform.position);
        Debug.Log("Roaming Position " + roamingPosition);
        transform.position = Vector3.MoveTowards(startingPosition, roamingPosition, Time.deltaTime * speed);
        //rigidBody.velocity = new Vector3(speed, roamingPosition.x, roamingPosition.z);

        // if bee gets to roamingPosition have bee move to new roaming position
        if (Vector3.Distance(transform.position, roamingPosition) < 1f)
        {
            roamingPosition = GetRoamingPosition();
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
