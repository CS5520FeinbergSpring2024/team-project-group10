using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeeHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        // if bee health goes to 0 return to hive maybe take away some of a resource?
        if(health <= 0) {
            SceneManager.LoadScene("Hive");
        }
    }
}
