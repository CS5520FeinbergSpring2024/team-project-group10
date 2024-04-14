using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeeHealth : MonoBehaviour
{
    private int maxHealth = 100;
    private int health;
    public GameObject deathScreen;
    private int deathScreenTime = 5;
    public int Health
    {
        get { return health;}
    }
    public event EventHandler OnHealthChanged;
    // set to determine healing rate
    private int healthRegerationPerSetTime = 1;
    private int setTime = 5;
    // For preventing healing while under attack
    private float lastAttack;
    private float healDelay = 3;
    
    void Awake()
    {
        health = maxHealth;
    }

    void Start()
    {   
        StartCoroutine(RegenerateHealth());
    }


    /// <summary>
    /// Method adjust health based on damage and 
    /// return the bee to hive it health reaches 0.
    /// Event handler added to notify listeners on change to health;
    /// </summary>
    public void TakeDamage(int damage)
    {   
        lastAttack = Time.time;
        health -= damage;
        // if bee health goes to 0 return to hive
        if(health <= 0) {
            StartCoroutine(BeeDeath());
        }
        // notify any listners
        OnHealthChanged?.Invoke(this, EventArgs.Empty); 
    }

    /// <summary>
    /// if bee health is below max health will regenerate health
    /// at set amount per set time interval
    /// </summary>
    IEnumerator RegenerateHealth()
    {
        while (true) {
        // if health is below max and bee hasn't been attacked 
        // for atleast healDelay seconds (to prevent healing while under attack)
        if (health < maxHealth && Time.time > lastAttack + healDelay)
        {   
            health += healthRegerationPerSetTime;
            OnHealthChanged?.Invoke(this, EventArgs.Empty); 
            yield return new WaitForSeconds(setTime);
        }
        else
        {
            yield return null;
        }
    }
    }

        /// <summary>
    /// if bee health is below max health will regenerate health
    /// at set amount per set time interval
    /// </summary>
    IEnumerator BeeDeath()
    {
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
            yield return new WaitForSeconds(deathScreenTime - 1);
            SceneManager.LoadScene("Hive");
            yield return new WaitForSeconds(1);
            deathScreen.SetActive(false);
            
        }
         yield return null;
    }
}
