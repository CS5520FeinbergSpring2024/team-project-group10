using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeeHealth : MonoBehaviour
{
    private int maxHealth = 100;
    private int health = 100;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(health);
        health -= damage;
        // if bee health goes to 0 return to hive
        if(health <= 0) {
            SceneManager.LoadScene("Hive");
        }
    }

    // 
    public int Health
    {
        get { return health;}
    }
}
