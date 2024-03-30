using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UIElements;

// inherits from enemy abstract class
public class Wasp : Enemy
{
    public InventoryScriptableObject UserInventory;
    [SerializeField] int pollenKillPenalty = 1;

    [SerializeField] private int waspDamage = 5;
    [SerializeField] private  int waspSpeed = 8;
    private float waspChaseRange = 10;
    private float waspAttackRange = 2.5f;
    private float waspAttackCooldown = 1;
    [SerializeField] private float waspPathRange = 10;


    protected override void Awake()
    {
        base.Awake();
        //SetEnemyStats(5, 5, 10, 2.5f, 1, 5);
        SetEnemyStats(waspDamage, waspSpeed, waspChaseRange, waspAttackRange, waspAttackCooldown, waspPathRange);
    }

    // if enough in inventory on wasp killing bee 1 pollen will be taken
    private protected override void OnKill()
    {
        base.OnKill();
        if (UserInventory != null) 
        {
            if (UserInventory.GetResourceCount(ResourceType.Pollen) >= pollenKillPenalty)
            {
                UserInventory.UpdateInventory(ResourceType.Pollen, -pollenKillPenalty);
            }
        }
        
    }

}
