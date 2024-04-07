using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ResourceType;


public class Building : MonoBehaviour
{
    public InventoryScriptableObject myInventory;
    [SerializeField]
    private BuildingType myNewBuildingType;
    private ResourceType myResourceType;
    [SerializeField]
    private Vector2Int tileId; // TileId stored as x, z coordinates
    public BuildingType Type
    { get { return myNewBuildingType; } set { myNewBuildingType = value; } }
    
    public ResourceType ResourceType
    { get { return myResourceType; } set { myResourceType = value; } }
    
    public Vector2 TileID 
    { get { return TileID; } set { TileID = value; } }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Building(BuildingType type, ResourceType resourceType, Vector2 tileID)
    {
        Type = type;
        ResourceType = resourceType;
        TileID = tileID;
    }

    // Made method static to check if a Building can be afforded based on what is in the inventory
    // After pollen or nectar has been collected to the required amount
    public static bool CanAfford(BuildingType type, InventoryScriptableObject inventory)
    {
        if (type == BuildingType.Gathering)
        {
            return inventory.GetResourceCount(ResourceType.Pollen) >= 5;
            
            
        }
        else if(type == BuildingType.Storage)
        {
            return inventory.GetResourceCount(ResourceType.Nectar) >= 10 &&
                inventory.GetResourceCount(ResourceType.Pollen) >= 5;


        }
        else if(type == BuildingType.Production)
        {
            return inventory.GetResourceCount(ResourceType.Nectar) >= 20 &&
                inventory.GetResourceCount(ResourceType.Pollen) >= 10;
            
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
