using System.Collections.Generic;
using UnityEngine;

public class HiveGameManager : MonoBehaviour
{
    public GameObject buildingGatheringPrefab;
    public GameObject buildingStoragePrefab;
    public GameObject buildingProductionPrefab;
    public HiveDataSingleton hiveSingleton;
    // Flag used to enable/disable building on a tile
    public bool building = false;

    // Start is called before the first frame update
    void Start()
    {
        hiveSingleton = new();
        // Getting all of the buildings from the HiveSingleton and instantiating them
        InstantiateBuildingsFromSingleton();
    }

    public void Build(BuildingType buildingType, ResourceType resourceType, Vector3 position)
    {
        Building newBuilding = InstantiateBuilding(buildingType, resourceType, position);
        if (newBuilding != null)
        {
            hiveSingleton.AddBuilding(newBuilding);
            (int, int) stationLevels = hiveSingleton.GetStationLevels(resourceType);
            if (buildingType == BuildingType.Storage) {
                hiveSingleton.UpdateStationLevels(resourceType, stationLevels.Item1 + 1, stationLevels.Item2);
            } else {
                hiveSingleton.UpdateStationLevels(resourceType, stationLevels.Item1, stationLevels.Item2 + 1);
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

    // Method to instantiate buildings from the hiveSingleton BuildingData
    private void InstantiateBuildingsFromSingleton()
    {
        // Clear gameObject list to create again to match the data list.
        hiveSingleton.GetBuildings().Clear();

        List<BuildingData> buildingData = hiveSingleton.GetBuildingData();
        
        // Check if the list is empty
        if (buildingData == null || buildingData.Count == 0)
        {
            Debug.Log("No buildings found in hiveSingleton as yet.");
            return;
        }

        // Iterate over each building in the list
        foreach (BuildingData bd in buildingData)
        {
            Debug.Log(bd);
            // Instantiate the building from the building data
            Building newBuilding = InstantiateBuilding(bd.Type, bd.ResourceType, bd.Position);

            // Check if the instantiation was successful
            if (newBuilding != null)
            {
                Debug.Log("Building instantiated: " + newBuilding.Type + " with resource: " + newBuilding.ResourceType);
            }
            else
            {
                Debug.LogError("Failed to instantiate building from hiveSingleton: " + bd.Type);
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
                newBuilding.UpdateResourceDisplay(resourceType);
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
    public bool IsOccupied(Vector2 tileID) {
        List<Building> buildings = hiveSingleton.GetBuildings();

        // Check if the list is empty
        if (buildings == null || buildings.Count == 0) {
            Debug.Log("No buildings found in hiveSingleton as yet.");
            return false;
        }

        // Iterate over each building in the list
        foreach (Building building in buildings) {
            if (building.TileID == tileID) {
                return true;
            }
        }

        return false;
    }
}
