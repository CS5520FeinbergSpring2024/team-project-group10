using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeeHealth : MonoBehaviour
{
    private int maxHealth = 100;
    private int health = 100;
    public event EventHandler OnHealthChanged;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        // if bee health goes to 0 return to hive
        if(health <= 0) {
            SceneManager.LoadScene("Hive");
        }
        // notify any listners
        OnHealthChanged?.Invoke(this, EventArgs.Empty); 
    }

    // 
    public int Health
    {
        get { return health;}
    }
}
