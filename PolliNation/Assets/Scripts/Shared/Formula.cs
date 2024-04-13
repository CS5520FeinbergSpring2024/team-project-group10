using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formula
{
    private InventoryScriptableObject inventoryScriptableObject;

    public Formula(InventoryScriptableObject inventoryScriptableObject)
    {
        this.inventoryScriptableObject = inventoryScriptableObject;
    }
    
    // Creating dictionaries for the converted resources with the formulas for each resource
    public static Dictionary<ResourceType, Dictionary<ResourceType, int>> conversionFormulas = new()
    {
        { ResourceType.Honey, new Dictionary<ResourceType, int>() {
                { ResourceType.Nectar, 2 },
                { ResourceType.Pollen, 1 }
            }
        },
        { ResourceType.Propolis, new Dictionary<ResourceType, int>() {
                { ResourceType.Nectar, 3 },
                { ResourceType.Buds, 3 }
            }
        },
        { ResourceType.RoyalJelly, new Dictionary<ResourceType, int>() {
                { ResourceType.Nectar, 2 },
                { ResourceType.Pollen, 2 },
                { ResourceType.Water, 2 }
            }
        },
    };

    public bool ConvertResource(ResourceType resourceType, int assignedWorkers, out int producedQuantity)
    {
        producedQuantity = 0;
        if (conversionFormulas.TryGetValue(resourceType, out var formula))
        {
            // Check if we have enough resources to do the conversion
            bool enoughResources = true;
            foreach (var requirement in formula)
            {
                if (requirement.Key != resourceType && requirement.Value * assignedWorkers > GetResourceCount(requirement.Key))
                {
                    enoughResources = false;
                    break;
                }
            }

            if (enoughResources == true)
            {
                // Calculating how much of the converted resource is gonna be produced
                producedQuantity = formula[resourceType] * assignedWorkers;

                // Update the inventory for the required resources
                foreach (var requirement in formula)
                {
                    if (requirement.Key != resourceType)
                    {
                        UpdateResource(requirement.Key, -requirement.Value * assignedWorkers);
                    }
                }

                return true;
            }
        }

        return false;
    }

    private int GetResourceCount(ResourceType resourceType)
    {
        return inventoryScriptableObject.GetResourceCount(resourceType);
    }

    private void UpdateResource(ResourceType resourceType, int amount)
    {
        inventoryScriptableObject.UpdateInventory(resourceType, amount);
    }
}
