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
    private float waspAttackCooldown = 1;
    [SerializeField] private float waspPathRange = 5;


    protected override void Awake()
    {
        base.Awake();
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
