using UnityEngine;

// inherits from enemy abstract class
public class Wasp : Enemy
{
    public InventoryScriptableObject UserInventory;
    [SerializeField] int pollenKillPenalty = 1;
    [SerializeField] private int waspDamage = 5;
    [SerializeField] private  int waspSpeed = 8;
    [SerializeField] private float waspChaseRange = 7;
    private float waspAttackRange = 2.5f;
    [SerializeField] private float waspAttackCooldown = 1;
    [SerializeField] private int waspHealth = 5;
    [SerializeField] private float waspPatrolRange = 10;


    protected override void Awake()
    {
        base.Awake();
        SetEnemyStats(waspDamage, waspSpeed, waspChaseRange, waspAttackRange, waspAttackCooldown, waspPatrolRange, waspHealth);
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
