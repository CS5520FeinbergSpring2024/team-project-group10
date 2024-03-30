using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Bee : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private float speed = 10;
    private Vector2 moveDirection;
    private Rigidbody _RigidBody;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out _RigidBody);
    }

    // Update method for physics-related code
    void FixedUpdate() {
        // If joystick is moved, move bee and rotate it to direction of movement
        if(moveDirection.x != 0 || moveDirection.y != 0) {
            Vector3 moveAmount = new Vector3(moveDirection.x, 0, moveDirection.y);
            _RigidBody.position += Time.fixedDeltaTime * speed * moveAmount;
            // Set height to zero after each movement to ensure bee doesn't fly out of bounds
            _RigidBody.position = new Vector3(_RigidBody.position.x, 0, _RigidBody.position.z);

            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.y));
            _RigidBody.rotation = Quaternion.Lerp(_RigidBody.rotation, newRotation, Time.fixedDeltaTime * speed);
        }
}
    // Update is called once per frame
    void Update()
    {
        // Retrieve joystick input direction
        moveDirection = moveAction.action.ReadValue<Vector2>();
    }
    
    // Manage collisions with other objects
    void OnTriggerEnter(Collider other) {
        // Switch to hive scene when colliding with hive object
        if (other.name.Equals("Overworld_HiveObject")) {
            SceneManager.LoadScene("Hive");
        }
    }
}
