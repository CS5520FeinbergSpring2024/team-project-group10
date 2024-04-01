using UnityEngine;
using UnityEngine.InputSystem.Android;

// inherits from enemy abstract class
public class Wasp : Enemy
{
    public InventoryScriptableObject UserInventory;
    [SerializeField] int pollenKillPenalty = 1;
    [SerializeField] private int waspDamage = 5;
    [SerializeField] private  int waspSpeed = 8;
    [SerializeField] private float waspChaseRange = 7;
    [SerializeField] private float waspAttackCooldown = 1;
    [SerializeField] private int waspHealth = 5;
    [SerializeField] private float waspPatrolRange = 10;
    public Animator animator;
    private SpriteRenderer damageIndicator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        damageIndicator = GetComponentInChildren<SpriteRenderer>();
        SetEnemyStats(waspDamage, waspSpeed, waspChaseRange, 
            waspAttackCooldown, waspPatrolRange, waspHealth);
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
    }

    private protected override void Attack()
    {
        base.Attack();
        animator.enabled = true;
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
