using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeeHealth : MonoBehaviour
{
    private int maxHealth = 100;
    private int health;
    public event EventHandler OnHealthChanged;
    
    void Awake()
    {
        health = maxHealth;
    }

    /// <summary>
    /// Method adjust health based on damage and 
    /// return the bee to hive it health reaches 0.
    /// Event handler added to notify listeners on change to health;
    /// </summary>
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

    public int Health
    {
        get { return health;}
    }
}
