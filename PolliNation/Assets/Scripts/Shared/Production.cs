using System.Collections;
using UnityEngine;

public class Production : MonoBehaviour
{
    public HiveDataSingleton hiveSingleton;
    public InventoryDataSingleton inventoryDataSingleton;
    public float productionInterval = 1f;
    public float productionPerWorker = 1f;
    private Formula formula;
    private static Production instance;

    // Start is called before the first frame update
    void Start()
    {
        hiveSingleton = new();
        
        // Initializng formula
        formula = new Formula(inventoryDataSingleton);
        Debug.Log("Formula has been initialized .");
        StartCoroutine(ProduceResources());
    }

    private void Awake()
    {
        Debug.Log("Production instance created.");
        // Ensuring only one instance of Production exists and not destroying when we change the scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            // Destroy duplicate instances
            Destroy(gameObject); 
        }
        inventoryDataSingleton = new();
    }

    private IEnumerator ProduceResources()
    {
        // This is a threshold for starting to produce again
        // float fullnessThreshold = 0.6f;

        while (true)
        {
            yield return new WaitForSeconds(productionInterval);

            // Handling the converted resources honey, propolis, and royal jelly with the formula
            foreach (ResourceType resourceType in new ResourceType[] { ResourceType.Honey, ResourceType.Propolis, ResourceType.RoyalJelly })
            {
                // get the number of assigned workers and produces resources each second 
                // with one resource being produced per worker
                int assignedWorkers = hiveSingleton.GetAssignedWorkers(resourceType);
                
                if (assignedWorkers > 0)
                {
                    // Quick check for the resource thats being processed right now
                    Debug.Log("Processing resource type: " + resourceType);
                    if (formula.ConvertResource(resourceType, assignedWorkers, productionPerWorker, out int producedQuantity))
                    {
                      Debug.Log("Conversion successful for " + resourceType + " with expected produced quantity: " + producedQuantity);
                      inventoryDataSingleton.UpdateInventory(resourceType, producedQuantity);
                    }
                    else
                    {
                      Debug.Log("The conversion failed");
                    }                 
                }
            }

            // Dealing with the raw resources that don't need to use the formula
            foreach (ResourceType resourceType in new ResourceType[] { ResourceType.Nectar, ResourceType.Pollen, ResourceType.Buds, ResourceType.Water })
            {
                int assignedWorkers = hiveSingleton.GetAssignedWorkers(resourceType);
                if (assignedWorkers > 0)
                {
                    int producedQuantity = Mathf.RoundToInt(assignedWorkers * productionPerWorker);
                    inventoryDataSingleton.UpdateInventory(resourceType, producedQuantity);                      
                }
            }
        }
    }
}
