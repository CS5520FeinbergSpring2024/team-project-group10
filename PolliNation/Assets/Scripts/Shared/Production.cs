using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Production : MonoBehaviour
{
    public HiveScriptable hiveScriptableObject;
    public InventoryScriptableObject inventoryScriptableObject;
    public ResourceType resourceType;
    public float productionInterval = 1f;
    public float productionPerWorker = 1f;
    private Formula formula;

    // Start is called before the first frame update
    void Start()
    {
        // Initializng formula
        formula = new Formula(inventoryScriptableObject); 
        StartCoroutine(ProduceResources());
    }

    private IEnumerator ProduceResources()
    {
        while (true)
        {
            yield return new WaitForSeconds(productionInterval);

            // Handling the converted resources honey, propolis, and royal jelly with the formula
            foreach (ResourceType resourceType in new ResourceType[] { ResourceType.Honey, ResourceType.Propolis, ResourceType.RoyalJelly })
            {
                // get the number of assigned workers and produces resources each second 
                // with one resource being produced per worker
                int assignedWorkers = hiveScriptableObject.GetAssignedWorkers(resourceType);
                if (assignedWorkers > 0)
                {
                    if (formula.ConvertResource(resourceType, assignedWorkers, out int producedQuantity))
                    {
                        inventoryScriptableObject.UpdateInventory(resourceType, producedQuantity);
                    }
                }
            }

            // Dealing with the raw resources that don't need to use the formula
            foreach (ResourceType resourceType in new ResourceType[] { ResourceType.Nectar, ResourceType.Pollen, ResourceType.Buds, ResourceType.Water })
            {
                int assignedWorkers = hiveScriptableObject.GetAssignedWorkers(resourceType);
                if (assignedWorkers > 0)
                {
                    int producedQuantity = Mathf.RoundToInt(assignedWorkers * productionPerWorker);
                    inventoryScriptableObject.UpdateInventory(resourceType, producedQuantity);
                }
            }
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
