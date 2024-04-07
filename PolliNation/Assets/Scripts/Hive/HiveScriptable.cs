using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHive", menuName = "Data/Hive")]
public class HiveScriptable : ScriptableObject
{
    private List<Building> buildings = new List<Building>();
    private int availableWorkers = 0;
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


    // Method to add a new building to the list of buildings
    public void AddBuilding(Building building)
    {
        buildings.Add(building);
        Debug.Log("Building added to list. Total buildings: " + buildings.Count);
        Debug.Log("Building prefab reference: " + building.gameObject);
    }

  
    public List<Building> GetBuildings()
    {
        Debug.Log("Number of buildings in the list: " + buildings.Count);
        return buildings;
    }

    // Method to add to total available workers 
    public void AddWorkers(int numberOfWorkers)
    {
        availableWorkers += numberOfWorkers;
    }

    // Method to get number of available workers
    public int GetAvailableWorkers()
    {
        return availableWorkers;
    }

    // Method to assign workers to a resource type
    public void AssignWorkers(ResourceType resourceType, int numberOfWorkers)
    {
        if (numberOfWorkers <= availableWorkers)
        {
            assignedWorkers[resourceType] = numberOfWorkers;
            availableWorkers -= numberOfWorkers;
        }
        else
        {
            Debug.Log("Not enough available workers to assign.");
        }
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
