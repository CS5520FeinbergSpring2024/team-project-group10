
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores hive-related data, including buildings and bees.
/// Includes update methods for that data.
/// </summary>
public class HiveDataSingleton
{
  // For singleton.
  private static HiveDataSingleton _instance;
  public static HiveDataSingleton Instance
  {
    get { return _instance; }
  }

  // Instance variables. Should primarily be accessed through the Properties,
  // even within this class, or through Instance.<field> so that all of the 
  // data is attached to the singleton instance.

  private List<Building> _buildings;
  private List<Building> Buildings
  {
    get { return Instance._buildings; }
  }
  private int _totalWorkers;
  private int TotalWorkers
  {
    get { return Instance._totalWorkers; }
    set { Instance._totalWorkers = value; }
  }
  private Dictionary<ResourceType, int> _assignedWorkers;
  private Dictionary<ResourceType, int> AssignedWorkers
  {
    get { return Instance._assignedWorkers; }
    set { Instance._assignedWorkers = value; }
  }
  private Dictionary<ResourceType, (int storageLevel, int productionLevel)> _resourceLevels;
  private Dictionary<ResourceType, (int storageLevel, int productionLevel)> ResourceLevels
  {
    get { return Instance._resourceLevels; }
    set { Instance._resourceLevels = value; }
  }
  private event EventHandler _onStationLevelChanged;
  public event EventHandler OnStationLevelChanged
  {
    add { Instance._onStationLevelChanged += value; }
    remove { Instance._onStationLevelChanged -= value; }
  }

  public HiveDataSingleton()
  {
    // Make a singleton.
    if (_instance != null)
    {
      return;
    }
    _instance = this;

    Instance._buildings = new();
    TotalWorkers = 0;
    AssignedWorkers = new();
    ResourceLevels = new();

    foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
    {
      AssignedWorkers.Add(resourceType, 0);
      ResourceLevels.Add(resourceType, (0, 0));
    }
  }

  // Method to add a new building to the list of buildings
  public void AddBuilding(Building building)
  {
    Buildings.Add(building);
    Debug.Log("Building added to list. Total buildings: " + Buildings.Count);
    Debug.Log("Building prefab reference: " + building.gameObject);
  }

  public List<Building> GetBuildings()
  {
    Debug.Log("Number of buildings in the list: " + Buildings.Count);
    return Buildings;
  }

  // Method to add to total available workers 
  public void AddWorkers(int numberOfWorkers)
  {
    TotalWorkers += numberOfWorkers;
  }

  // Method to get number of available workers
  public int GetTotalWorkers()
  {
    return TotalWorkers;
  }

  // Method to assign workers to a resource type
  public void AssignWorkers(ResourceType resourceType, int numberOfWorkers)
  {
    AssignedWorkers[resourceType] = numberOfWorkers;
  }

  public int GetAssignedWorkers(ResourceType resourceType)
  {
    return AssignedWorkers[resourceType];
  }

  // Method to update the station levels for a specific resource type
  public void UpdateStationLevels(ResourceType resourceType, int storageLevel,
      int productionLevel)
  {
    ResourceLevels[resourceType] = (storageLevel, productionLevel);
    // notify subscribers
    Instance._onStationLevelChanged?.Invoke(Instance, EventArgs.Empty);
  }

  // Getting the current station level for the resource, if there is no station then its zero
  public (int storageLevel, int productionLevel) GetStationLevels(ResourceType resourceType)
  {
    return ResourceLevels[resourceType];
  }
}
