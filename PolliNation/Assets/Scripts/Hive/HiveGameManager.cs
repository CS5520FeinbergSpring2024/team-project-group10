using System.Collections.Generic;
using UnityEngine;

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
            // Getting all of the buildings from the HiveScriptable and instantiating them
            InstantiateBuildingsFromScriptable();
        }
    }

    public void Build(BuildingType buildingType, ResourceType resourceType, Vector3 position)
    {
        Building newBuilding = InstantiateBuilding(buildingType, resourceType, position);
        if (newBuilding != null)
        {
            hiveScriptable.AddBuilding(newBuilding);
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
            Debug.Log(building);
            // Instantiate the building from the building data
            Vector3 position = new Vector3(building.TileID.x, 2f, building.TileID.y);
            Building newBuilding = InstantiateBuilding(building.Type, building.ResourceType, position);

            // Check if the instantiation was successful
            if (newBuilding != null)
            {
                Debug.Log("Building instantiated: " + newBuilding.Type + " with resource: " + newBuilding.ResourceType);
            }
            else
            {
                Debug.LogError("Failed to instantiate building from HiveScriptable: " + building.Type);
            }
        }

    }

    private Building InstantiateBuilding(BuildingType buildingType, ResourceType resourceType, Vector3 position)
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
                newBuilding.TileID = new Vector2(position.x, position.z);
                Debug.Log("Building instantiated: " + buildingType + " with resource: " + resourceType);
                return newBuilding;
            }
            else
            {
                Debug.LogError("Failed to get Building component from instantiated object.");
                return null;
            }
        }
        else
        {
            Debug.LogError("Failed to get building prefab for building type: " + buildingType);
            return null;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
