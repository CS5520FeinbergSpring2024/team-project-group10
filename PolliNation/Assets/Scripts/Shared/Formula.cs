using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formula
{
    private InventoryScriptableObject inventoryScriptableObject;

    public Formula(InventoryScriptableObject inventoryScriptableObject)
    {
        this.inventoryScriptableObject = inventoryScriptableObject;

        foreach (var pair in conversionFormulas)
        {
            Debug.Log("Key: " + pair.Key);
            foreach (var innerPair in pair.Value)
            {
                Debug.Log("Inner Key: " + innerPair.Key + ", Inner Value: " + innerPair.Value);
            }
        }
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
                {ResourceType.Water, 2 }
            }
        },
    };

    public bool ConvertResource(ResourceType resourceType, int assignedWorkers, out int producedQuantity)
    {
        producedQuantity = 0;
        Debug.Log("Attempting to convert: " + resourceType);
        Debug.Log("Resource type: " + resourceType);
        
        if (!conversionFormulas.ContainsKey(resourceType))
        {
            Debug.LogError("Conversion formula not found for resource type: " + resourceType);
            return false;
        }
        else
        {
            Debug.Log("The conversion formula was found");
        }

        // Access the formula directly from the dictionary
        var formula = conversionFormulas[resourceType];

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

            Debug.Log("You have enough resources for the conversion: " + enoughResources);

            if (enoughResources == true)
            {
            try
            { // Calculating how much of the converted resource is gonna be produced
                producedQuantity = formula[resourceType] * assignedWorkers;
                Debug.Log("Expected produced quantity is: " + producedQuantity);
            } catch(KeyNotFoundException ex) 
            { 
                Debug.LogError("Key not found in formula dictionary: " + ex.Message); 
            }
                // Calculating how much of the converted resource is gonna be produced
                producedQuantity = formula[resourceType] * assignedWorkers;
                Debug.Log("Expected produced quantity is: " + producedQuantity);

                // Update the inventory for the required resources
                foreach (var requirement in formula)
                {
                    if (requirement.Key != resourceType)
                    {
                        UpdateResource(requirement.Key, -requirement.Value * assignedWorkers);
                    }
                }
                Debug.Log("Conversion was successful");
                return true;
            }
        Debug.Log("Failed to convert the resoure");
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
