using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHive", menuName = "Custom/Hive")]
public class HiveScriptable : ScriptableObject
{
    [SerializeField]
    private List<Building> buildings = new List<Building>();
    [SerializeField]
    private Dictionary<ResourceType, int> assignedWorkers = new Dictionary<ResourceType, int>();
    [SerializeField]
    private Dictionary<ResourceType, (int storageLevel, int gatheringLevel, int productionLevel)> resourceLevels = 
        new Dictionary<ResourceType, (int, int, int)>();
    

    
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
        if (assignedWorkers.ContainsKey(resourceType))
        {
            assignedWorkers[resourceType] = numberOfWorkers;
        }
        else
        {
            assignedWorkers.Add(resourceType, numberOfWorkers);
        }
    }

    
    public int GetAssignedWorkers(ResourceType resourceType)
    {
        return assignedWorkers.ContainsKey(resourceType) ? assignedWorkers[resourceType] : 0;
    }

    // Method to update the station levels for a specific resource type
    public void UpdateStationLevels(ResourceType resourceType, int storageLevelIncrease, 
        int gatheringLevelIncrease, int productionLevelIncrease)
    {
        if (resourceLevels.ContainsKey(resourceType))
        {
            var currentLevels = resourceLevels[resourceType];
            resourceLevels[resourceType] = (currentLevels.storageLevel + storageLevelIncrease, 
                currentLevels.gatheringLevel + gatheringLevelIncrease, currentLevels.productionLevel + productionLevelIncrease);
        }
        else
        {
            resourceLevels.Add(resourceType, (storageLevelIncrease, gatheringLevelIncrease, productionLevelIncrease));
        }
    }

    // Getting the current station level for the resource, if there is no station then its zero
    public (int storageLevel, int gatheringLevel, int productionLevel) GetStationLevels(ResourceType resourceType)
    {
        return resourceLevels.ContainsKey(resourceType) ? resourceLevels[resourceType] : (0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
