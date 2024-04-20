using System.Collections.Generic;
using UnityEngine;

public class HiveGameManager : MonoBehaviour {
    public GameObject buildingStoragePrefab;
    public GameObject buildingGatheringPrefab;
    public GameObject buildingProductionPrefab;
    public HiveDataSingleton hiveSingleton;
    public InventoryDataSingleton inventorySingleton;
    // Flag used to enable/disable building on a tile
    public bool building = false;
    // List of Building GameObjects used to destroy buildings
    private Dictionary<Vector3, Building> _buildings = new();

    // Start is called before the first frame update
    void Start() {
        hiveSingleton = new();
        inventorySingleton = new();
        // Getting all of the buildings from the HiveSingleton and instantiating them
        InstantiateBuildingsFromSingleton();
        TutorialStatic.Snackbar = FindObjectOfType<SnackbarScript>(true);
        TutorialStatic.EnteredHive();
    }

    public void Build(BuildingType buildingType, ResourceType resourceType, Vector3 position) {
        Building newBuilding = InstantiateBuilding(buildingType, resourceType, position);
        TutorialStatic.BuiltStation(buildingType, resourceType);
        if (newBuilding != null) {
            hiveSingleton.AddBuildingData(buildingType, resourceType, position);
            (int, int) stationLevels = hiveSingleton.GetStationLevels(resourceType);
            if (buildingType == BuildingType.Storage) {
                hiveSingleton.UpdateStationLevels(resourceType, stationLevels.Item1 + 1, stationLevels.Item2);
            } else {
                hiveSingleton.UpdateStationLevels(resourceType, stationLevels.Item1, stationLevels.Item2 + 1);
            }
        }
    }

    public void DestroyBuilding(Vector3 tilePosition) {
        // Destroy the building GameObject
        if (_buildings.ContainsKey(tilePosition)) {
            Debug.Log("DESTROY: Found gameObject");
            _buildings[tilePosition].DestroyGameObject();
            _buildings.Remove(tilePosition);
        }

        // Remove the building data corresponding to the given tilePosition
        BuildingData bd = hiveSingleton.GetBuildingDataByTilePosition(tilePosition);
        hiveSingleton.BuildingData.Remove(bd);

        // Update station levels
        (int, int) stationLevels = hiveSingleton.GetStationLevels(bd.ResourceType);
        if (bd.BuildingType == BuildingType.Storage) {
            hiveSingleton.UpdateStationLevels(bd.ResourceType, stationLevels.Item1 - 1, stationLevels.Item2);
            inventorySingleton.UpdateInventory(bd.ResourceType, 0);
        } else {
            hiveSingleton.UpdateStationLevels(bd.ResourceType, stationLevels.Item1, stationLevels.Item2 - 1);
            // Reset workers if no remaining gathering/conversion stations
            if (hiveSingleton.GetStationLevels(bd.ResourceType).productionLevel == 0) {
                hiveSingleton.AssignWorkers(bd.ResourceType, 0);
            }
        }
    }

    private GameObject GetBuildingPrefab(BuildingType buildingType) {
        switch (buildingType) {
            case BuildingType.Storage:
                return buildingStoragePrefab;
            case BuildingType.Gathering:
                return buildingGatheringPrefab;
            case BuildingType.Production:
                return buildingProductionPrefab;
            default:
                Debug.LogError("Unknown building type: " + buildingType);
                return null;
        }
    }

    // Method to instantiate buildings from the hiveSingleton BuildingData
    private void InstantiateBuildingsFromSingleton() {
        List<BuildingData> buildingData = hiveSingleton.GetBuildingData();
        
        // Check if the list is empty
        if (buildingData == null || buildingData.Count == 0) {
            Debug.Log("No buildings found in hiveSingleton as yet.");
            return;
        }

        // Iterate over each building in the list
        foreach (BuildingData bd in buildingData) {
            // Instantiate the building from the building data
            InstantiateBuilding(bd.BuildingType, bd.ResourceType, bd.Position);
        }
    }

    private Building InstantiateBuilding(BuildingType buildingType, ResourceType resourceType, Vector3 position) {
        GameObject buildingPrefab = GetBuildingPrefab(buildingType);
        if (buildingPrefab != null) {
            // Instantiate the building prefab at the specified position
            GameObject newBuildingObject = Instantiate(buildingPrefab, position, Quaternion.identity);

            // Get the Building component from the instantiated object
            Building newBuilding = newBuildingObject.GetComponent<Building>();

            // Associate the building with the selected resource 
            if (newBuilding != null) {
                newBuilding.UpdateResourceDisplay(resourceType);
                _buildings.Add(position, newBuilding);
                Debug.Log($"{resourceType} {buildingType} Station instantiated");
                return newBuilding;
            }
            else {
                Debug.LogError("Failed to get Building component from instantiated object.");
                return null;
            }
        }
        else {
            Debug.LogError("Failed to get building prefab for building type: " + buildingType);
            return null;
        }
    }

    public bool IsOccupied(Vector3 tilePosition) {
        BuildingData buildingData = hiveSingleton.GetBuildingDataByTilePosition(tilePosition);
        return buildingData != null;
    }

    // Made method static to check if the given formula can be afforded based on what is in the inventory
    public static bool CanAfford(Dictionary<ResourceType, int> formula, InventoryDataSingleton inventory) {
        foreach (ResourceType resource in formula.Keys) {
            if (inventory.GetResourceCount(resource) < formula[resource]) {
                return false;
            }
        }
        return true;
    }
}
