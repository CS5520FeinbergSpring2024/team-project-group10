using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building : MonoBehaviour
{
    public InventoryScriptableObject myInventory;
    [SerializeField]
    private BuildingType myNewBuildingType;
    [SerializeField]
    private ResourceType myResourceType;
    public BuildingType Type
    { get { return myNewBuildingType; } set { myNewBuildingType = value; } }
    public int Cost
    { get; set; }
    public ResourceType? ResourceType
    { get { return myResourceType; } set => myResourceType = (ResourceType)value; }
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

    // Made method static to check if a Building can be afforded based on what is in the inventory
    // After pollen or nectar has been collected to the required amount
    public static bool CanAfford(BuildingType type, InventoryScriptableObject inventory)
    {
        if (type == BuildingType.Gathering)
        {
            return inventory.GetResourceCount(global::ResourceType.Pollen) >= 5;
            
            
        }
        else if(type == BuildingType.Storage)
        {
            //return myInventory.GetResourceCount(global::ResourceType.Nectar) >= 10 &&
            //    myInventory.GetResourceCount(global::ResourceType.Pollen) >= 5;
            return true;
        
        }else if(type == BuildingType.Production)
        {
            //return myInventory.GetResourceCount(global::ResourceType.Nectar) >= 20 &&
            //        myInventory.GetResourceCount(global::ResourceType.Pollen) >= 10;
            return true;
        }
        else
        {
            Debug.Log("Building type not valid");
            return false;         
        }
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
