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
    
    //get accessors 
    public int GetNectarCount => resourceCounts[ResourceType.Nectar];
    public int GetPollenCount => resourceCounts[ResourceType.Pollen];
    public int GetWaterCount => resourceCounts[ResourceType.Water];
    public int GetBudsCount => resourceCounts[ResourceType.Buds];
    public int GetHoneyCount => resourceCounts[ResourceType.Honey];
    public int GetPropolisCount => resourceCounts[ResourceType.Propolis];
    public int GetRoyalJellyCount => resourceCounts[ResourceType.RoyalJelly];

    // update method
    public void UpdateInventory(ResourceType resourceType, int amount) 
    {  
        if (resourceCounts[resourceType] + amount >= 0) {
            resourceCounts[resourceType] += amount;
        }
        OnInventoryChanged?.Invoke(this, EventArgs.Empty); 
    }

    public void ResetInventory() {
        resourceCounts[ResourceType.Nectar] = 0;
        resourceCounts[ResourceType.Pollen] = 0;
        resourceCounts[ResourceType.Water] = 0;
        resourceCounts[ResourceType.Buds] = 0;
        resourceCounts[ResourceType.Honey] = 0;
        resourceCounts[ResourceType.Propolis] = 0;
        resourceCounts[ResourceType.RoyalJelly] = 0;
        // check if there are subscribers to event and if so trigger;
        OnInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

}

