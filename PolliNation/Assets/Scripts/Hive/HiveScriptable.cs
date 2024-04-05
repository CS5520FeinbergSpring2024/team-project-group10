using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHive", menuName = "Data/Hive")]
public class HiveScriptable : ScriptableObject
{
    
    private List<Building> buildings = new List<Building>();
    private Dictionary<ResourceType, int> assignedWorkers = new Dictionary<ResourceType, int>();
    private Dictionary<ResourceType, (int storageLevel, int productionLevel)> resourceLevels = 
        new Dictionary<ResourceType, (int, int)>();
    
    public HiveScriptable() { 
        foreach(ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            assignedWorkers.Add(resourceType, 0);
        }

        foreach(ResourceType resourceType in Enum.GetValues(typeof(ResourceType))) 
        { 
            resourceLevels.Add(resourceType, (0, 0));
        }
    }

    private void Awake()
    {
        // Initialize the buildings list
        buildings = new List<Building>();

        
    }


    // Method to add a new building to the list of buildings
    public void AddBuilding(Building building)
    {
        buildings.Add(building);
    }

  
    public List<Building> GetBuildings()
    {
        return buildings;
    }


    // Method to assign workers to a resource type
    public void AssignWorkers(ResourceType resourceType, int numberOfWorkers)
    {
        assignedWorkers[resourceType] = numberOfWorkers;
    }

    
    public int GetAssignedWorkers(ResourceType resourceType)
    {
        return assignedWorkers[resourceType];
    }

    // Method to update the station levels for a specific resource type
    public void UpdateStationLevels(ResourceType resourceType, int storageLevel, 
        int productionLevel)
    {
        resourceLevels[resourceType] = (storageLevel, productionLevel);
    }

    // Getting the current station level for the resource, if there is no station then its zero
    public (int storageLevel, int productionLevel) GetStationLevels(ResourceType resourceType)
    {
        return resourceLevels[resourceType];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
