using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores hive-related data, including buildings and bees.
/// Includes update methods for that data.
/// </summary>
public class HiveDataSingleton {
  // For singleton.
  private static HiveDataSingleton _instance;
  public static HiveDataSingleton Instance {
    get { return _instance; }
  }

  // Instance variables. Should primarily be accessed through the Properties,
  // even within this class, or through Instance.<field> so that all of the 
  // data is attached to the singleton instance.

  private List<BuildingData> _buildingData;
  internal List<BuildingData> BuildingData {
    get { return Instance._buildingData; }
    set {Instance._buildingData = value; }
  }

  private int _totalWorkers;
  internal int TotalWorkers {
    get { return Instance._totalWorkers; }
    set { Instance._totalWorkers = value; }
  }

  private Dictionary<ResourceType, int> _assignedWorkers;
  internal Dictionary<ResourceType, int> AssignedWorkers {
    get { return Instance._assignedWorkers; }
    set { Instance._assignedWorkers = value; }
  }

  private Dictionary<ResourceType, (int storageLevel, int productionLevel)> _resourceLevels;
  internal Dictionary<ResourceType, (int storageLevel, int productionLevel)> ResourceLevels {
    get { return Instance._resourceLevels; }
    set { Instance._resourceLevels = value; }
  }

  private event EventHandler _onStationLevelChanged;
  public event EventHandler OnStationLevelChanged {
    add { lock(this) Instance._onStationLevelChanged += value; }
    remove { lock(this) Instance._onStationLevelChanged -= value; }
  }

  public HiveDataSingleton() {
    // Make a singleton.
    if (_instance != null) {
      return;
    }
    _instance = this;

    BuildingData = new();
    TotalWorkers = 0;
    AssignedWorkers = new();
    ResourceLevels = new();

    foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType))) {
      AssignedWorkers.Add(resourceType, 0);
      ResourceLevels.Add(resourceType, (0, 0));
    }
  }

  // Method to add serializeable building data, detatched from the GameObject
  // so it will last beyond the life of the GameObject and scene.
  public void AddBuildingData(BuildingType buildingType, ResourceType resourceType, Vector3 position) {
    Debug.Log("Building data added to list. Total buildings: " + BuildingData.Count);
    BuildingData.Add(new(buildingType, resourceType, position));
  }

  public List<BuildingData> GetBuildingData() {
    Debug.Log("Number of buildings in the list: " + BuildingData.Count);
    return BuildingData;
  }

  // Returns the building data associated with the given tilePosition (or null if none exists).
  // Null return value is expected since this will be used to check for occupied tiles
  public BuildingData GetBuildingDataByTilePosition(Vector3 tilePosition) {
    foreach (BuildingData buildingData in BuildingData) {
      if (buildingData.Position == tilePosition) {
        return buildingData;
      }
    }
    return null;
  }

  // Method to add to total workers 
  public void AddWorkers(int numberOfWorkers) {
    TutorialSingleton.ReceivedWorkers();
    TotalWorkers += numberOfWorkers;
  }

  // Method to get total workers
  public int GetTotalWorkers() {
    return TotalWorkers;
  }

  // Method to assign workers to a resource type
  public void AssignWorkers(ResourceType resourceType, int numberOfWorkers) {
    AssignedWorkers[resourceType] = numberOfWorkers;
    // Notify subscribers
    Instance._onStationLevelChanged?.Invoke(Instance, EventArgs.Empty);
  }

  public int GetAssignedWorkers(ResourceType resourceType) {
    return AssignedWorkers[resourceType];
  }

  // Method to update the station levels for a specific resource type
  public void UpdateStationLevels(ResourceType resourceType, int storageLevel, int productionLevel) {
    ResourceLevels[resourceType] = (storageLevel, productionLevel);
    // Notify subscribers
    Instance._onStationLevelChanged?.Invoke(Instance, EventArgs.Empty);
  }

  // Getting the current station level for the resource, if there is no station then its zero
  public (int storageLevel, int productionLevel) GetStationLevels(ResourceType resourceType) {
    return ResourceLevels[resourceType];
  }
}