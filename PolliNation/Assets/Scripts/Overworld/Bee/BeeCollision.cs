using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeeCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.name.Equals("Overworld_HiveObject")) {
            SceneManager.LoadScene("Hive");
        }
    }
}
