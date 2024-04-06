using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HiveGameManager : MonoBehaviour
{
    public GameObject buildingGatheringPrefab;
    public GameObject buildingStoragePrefab;
    public GameObject buildingProductionPrefab;
    public HiveScriptable hiveScriptable;

    // Start is called before the first frame update
    void Start()
    {
        if (hiveScriptable == null)
        {
            Debug.LogError("Hive Scriptable is not assigned!");
            return;
        }
        else
        {
            Debug.Log("Hive Scriptable is correctly assigned");
        }

        // Getting all of the buildings from the HiveScriptable and instantiating them
        // Commented for now because the data in the SO needs to be saved in persistent
        // storage so that the prefabs can be re-instantaited. The building is
        // a component of the prefab. But during new game session the prefab is no longer
        // instantiated so the component will be null unless it is saved somewhere and then retrieved

        // InstantiateBuildingsFromScriptable();
    }

    public void Build(BuildingType buildingType, ResourceType resourceType, Vector3 position)
    {
        GameObject buildingPrefab = GetBuildingPrefab(buildingType);
        if (buildingPrefab != null)
        {
            // Instantiate the building prefab at the specified position
            GameObject newBuildingObject = Instantiate(buildingPrefab, position, Quaternion.identity);

            // Get the Building component from the instantiated object
            Building newBuilding = newBuildingObject.GetComponent<Building>();

            // Associate the building with the selected resource 
            if (newBuilding != null)
            {
                Debug.Log("Building component instantiated successfully.");
                newBuilding.ResourceType = resourceType;
                Debug.Log("Building instantiated: " + buildingType + " with resource: " + resourceType);
                // And adding the building to the list in the SO
                hiveScriptable.AddBuilding(newBuilding);
             
            }
            else
            {
                Debug.LogError("Failed to get Building component from instantiated object.");
            }
        }
    }

    private GameObject GetBuildingPrefab(BuildingType buildingType)
    {
        switch (buildingType)
        {
            case BuildingType.Gathering:
                return buildingGatheringPrefab;
            case BuildingType.Storage:
                return buildingStoragePrefab;
            case BuildingType.Production:
                return buildingProductionPrefab;
            default:
                Debug.LogError("Unknown building type: " + buildingType);
                return null;
        }
    }

    // Method to instantiate buildings from the HiveScriptable
    private void InstantiateBuildingsFromScriptable()
    {
        List<Building> buildings = hiveScriptable.GetBuildings();

        // Check if the list is empty
        if (buildings == null || buildings.Count == 0)
        {
            Debug.Log("No buildings found in HiveScriptable as yet.");
            return;
        }

        // Iterate over each building in the list
        foreach (Building building in buildings)
        {
            // Check if the building object is not null
            if (building != null)
            {
                Debug.Log("Building component instantiated successfully.");
                // Confirm that the GetBuildingPrefab method is functioning correctly
                GameObject buildingPrefab = GetBuildingPrefab(building.Type);

                // Check if the buildingPrefab is not null
                if (buildingPrefab != null)
                {
                    // Instantiate the building prefab at the specified position
                    GameObject newBuildingObject = Instantiate(buildingPrefab, building.transform.position, Quaternion.identity);

                    // Get the Building component from the instantiated object
                    Building newBuilding = newBuildingObject.GetComponent<Building>();

                    // Associate the building with the selected resource 
                    if (newBuilding != null)
                    {
                        Debug.Log("Building component instantiated successfully.");
                        newBuilding.ResourceType = building.ResourceType;
                        Debug.Log("Building instantiated: " + building.Type + " with resource: " + building.ResourceType);
                    }
                    else
                    {
                        Debug.LogError("Failed to get Building component from instantiated object.");
                    }
                }
                else
                {
                    Debug.LogError("Failed to get building prefab for building type: " + building.Type);
                }
            }
            else
            {
                Debug.LogError("Building object is null.");
            }
        }

    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
