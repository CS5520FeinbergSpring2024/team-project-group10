using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ResourceType;

[CreateAssetMenu(fileName ="UserInventory", menuName = "Data/UserInventory")]
public class InventoryScriptableObject : ScriptableObject
{
    // set initial values to zero for now 
    // need to add method to get/set values from/to file with save system upon game start and end
    internal Dictionary<ResourceType, int> resourceCounts = new();
    internal Dictionary<ResourceType, int> resourceCarryLimits = new();
    internal Dictionary<ResourceType, bool> inventoryFull = new();
    public HiveScriptable hive;
    public event EventHandler OnInventoryChanged;
    private int storagePerLevel = 1000;

    public InventoryScriptableObject() {
        Debug.Log("LOADSCRIPTABLE");
        foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType))) {
            resourceCounts.Add(resource, 0);
            inventoryFull.Add(resource, false);

            // set initial user inventory carry only (what the user can "carry" wihtout storage buildings) limits 
            if (resource == ResourceType.Pollen || resource == ResourceType.Nectar 
            || resource == ResourceType.Water || resource == ResourceType.Buds)
            {
                resourceCarryLimits.Add(resource,100);
            }
            else
            {
                resourceCarryLimits.Add(resource,10);
            }
        }
    }
    
    /// <summary>
    /// Returns the inventory amount for the <c>resource</c>
    /// </summary>
    /// <param name="resource">resource type</param>
    /// <returns> amount of resource in inventory </returns>
    public int GetResourceCount(ResourceType resource){
        return resourceCounts[resource];
    }

    /// <summary>
    /// Returns the storage limit for the <c>resource</c>
    /// </summary>
    /// <param name="resource"> resource type</param>
    /// <returns> storage limit for resource </returns>
    public int GetStorageLimit(ResourceType resource){
        int storageCap = Math.Max(resourceCarryLimits[resource], hive.GetStationLevels(resource).storageLevel * storagePerLevel);
        return storageCap;
    }

    /// <summary>
    /// Updates the inventory count for the <c>resource</c> by <c>amount</c> 
    /// </summary>
    /// <param name="resource"> resource type</param>
    /// <param name="amount"> amount to update resource by </param>
    public void UpdateInventory(ResourceType resource, int amount) 
    {  
        int newInventoryValue = resourceCounts[resource] + amount;
        if (newInventoryValue >= 0 &&  (newInventoryValue <= GetStorageLimit(resource))) 
        {
            resourceCounts[resource] = newInventoryValue;
            inventoryFull[resource] = false;
        }
        else if (newInventoryValue < 0)
        {
            Debug.Log("Not enough resource in inventory");
            inventoryFull[resource] = false;
        }
        else
        {
            // set count to max allowed for resource 
            resourceCounts[resource] = GetStorageLimit(resource);
            inventoryFull[resource] = true;
            Debug.Log("Inventory full for resource, cannot add more to inventory");
        }
        OnInventoryChanged?.Invoke(this, EventArgs.Empty); 
    }

    public bool InventoryFull(ResourceType resource)
    {
        return inventoryFull[resource];
    }

}

