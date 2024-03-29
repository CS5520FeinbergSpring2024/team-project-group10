using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public InventoryScriptableObject myInventory;
    public BuildingType Type
    { get; set; }
    public int Cost
    { get; set; }
    public ResourceType? ResourceType 
    { get; set; }
    public Vector3 Position
    { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Building(BuildingType type, int cost, ResourceType? resourceType, Vector3 position)
    {
        Type = type;
        Cost = cost;
        ResourceType = resourceType;
        Position = position;
    }

    public bool CanAfford(BuildingType type)
    {
        if (type == BuildingType.Gathering)
        {
            if(myInventory.GetResourceCount(global::ResourceType.Nectar) >= 5)
            {
                return true;
            }
        }

        if(type == BuildingType.Storage)
        {
            if((myInventory.GetResourceCount(global::ResourceType.Nectar) >= 10) &&
                (myInventory.GetResourceCount(global::ResourceType.Pollen) >= 5))
            {
                return true;
            }
        }

        if(type == BuildingType.Production)
        {
            if((myInventory.GetResourceCount(global::ResourceType.Nectar) >= 20) &&
                    (myInventory.GetResourceCount(global::ResourceType.Pollen) >= 10))
            {
                return true;
            }
        }
        return true;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
