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
    private Dictionary<ResourceType, int> resourceCounts = new();
    public event EventHandler OnInventoryChanged;

    public InventoryScriptableObject() {
        foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType))) {
            resourceCounts.Add(resource, 0);
        }
    }
    
    public int GetResourceCount(ResourceType resourceType){
        return resourceCounts[resourceType];
    }

    // update method
    public void UpdateInventory(ResourceType resourceType, int amount) 
    {  
        if (resourceCounts[resourceType] + amount >= 0) {
            resourceCounts[resourceType] += amount;
        }
        OnInventoryChanged?.Invoke(this, EventArgs.Empty); 
    }

    public void ResetInventory() {
        foreach (ResourceType resource in resourceCounts.Keys){
            resourceCounts[resource] = 0;
        }
        // check if there are subscribers to event and if so trigger;
        OnInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

}

