using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bee : MonoBehaviour
{
    // Joystick Input
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Retrieve joystick direction
        Vector2 moveDirection = moveAction.action.ReadValue<Vector2>();

        // Move bee and rotate bee model in movement direction if joystick is moved
        if(moveDirection.x != 0 || moveDirection.y != 0) {
            transform.Translate(new Vector3(moveDirection.x * speed * Time.deltaTime, 0, moveDirection.y * speed * Time.deltaTime));
            GameObject child = gameObject.transform.GetChild(0).gameObject;
            child.transform.rotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.y));
        }
    }
}
