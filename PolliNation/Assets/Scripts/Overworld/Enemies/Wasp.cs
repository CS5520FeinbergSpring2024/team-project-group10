using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Android;

// inherits from enemy abstract class
public class Wasp : Enemy
{
    public InventoryDataSingleton UserInventory;
    // percentage penalty on inventory on kill
    [SerializeField] private int pollenKillPenaltyPercent = 25;
    [SerializeField] private int nectarKillPenaltyPercent = 25;
    [SerializeField] private int waspDamage = 5;
    [SerializeField] private  int waspSpeed = 8;
    [SerializeField] private float waspChaseRange = 7;
    [SerializeField] private float waspAttackCooldown = 1;
    [SerializeField] private int waspHealth = 5;
    [SerializeField] private float waspPatrolRange = 10;
    private bool killedBee;
    public Animator animator;
    private SpriteRenderer damageIndicator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        damageIndicator = GetComponentInChildren<SpriteRenderer>();
        SetEnemyStats(waspDamage, waspSpeed, waspChaseRange, 
            waspAttackCooldown, waspPatrolRange, waspHealth);
        UserInventory = new();
        killedBee = false;
    }

    private protected override void Roaming()
    {
        base.Roaming();
        animator.enabled = false;
        damageIndicator.enabled = false;
    }

    private protected override void Chase()
    {
        base.Chase();
        animator.enabled = false;
        damageIndicator.enabled = false;
        if (OverworldSoundManager.instance != null)
        {
            OverworldSoundManager.instance.PlayWaspSoundFX();
        }
    }

    private protected override void Attack()
    {
        base.Attack();
        animator.enabled = true;
        damageIndicator.enabled = true;
        if (!killedBee)
        {
            if (OverworldSoundManager.instance != null)
            {
                OverworldSoundManager.instance.PlayWaspSoundFX();
            }
            Handheld.Vibrate();
        }
    }

    // on kill by wasp reduce inventory by set kill penalty percentages
    private protected override void OnKill()
    {
        base.OnKill();
        killedBee = true;
        if (UserInventory != null) 
        {
            int pollenAmount = (int) Math.Floor(UserInventory.GetResourceCount(ResourceType.Pollen) * (pollenKillPenaltyPercent/100.0));
            int nectarAmount = (int) Math.Floor(UserInventory.GetResourceCount(ResourceType.Nectar) * (nectarKillPenaltyPercent/100.0));
            UserInventory.UpdateInventory(ResourceType.Pollen, -pollenAmount);
            UserInventory.UpdateInventory(ResourceType.Nectar, -nectarAmount);
        }
    }

}
