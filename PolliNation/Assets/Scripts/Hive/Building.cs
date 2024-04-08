using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private BuildingType myNewBuildingType;
    private ResourceType myResourceType;
    [SerializeField] private Vector2 tileId; // TileId stored as x, z coordinates
    public BuildingType Type
    { get { return myNewBuildingType; } set { myNewBuildingType = value; } }
    
    public ResourceType ResourceType
    { get { return myResourceType; } set { myResourceType = value; } }
    
    public Vector2 TileID 
    { get { return tileId; } set { tileId = value; } }

    // Static dictionary containing resource requirments for each building.
    // Used to check whether building can be built and to consume resources when build
    public static Dictionary<BuildingType, Dictionary<ResourceType, int>> buildingFormulas = new() {
        { BuildingType.Storage, new Dictionary<ResourceType, int>() {
                { ResourceType.Nectar, 5 }
            }
        },
        { BuildingType.Gathering, new Dictionary<ResourceType, int>() {
                { ResourceType.Nectar, 10 },
                { ResourceType.Pollen, 5 }
            }
        },
        { BuildingType.Production, new Dictionary<ResourceType, int>() {
                { ResourceType.Nectar, 20 },
                { ResourceType.Pollen, 10 }
            }
        },
    };

    public Building(BuildingType type, ResourceType resourceType, Vector2 tileID)
    {
        Type = type;
        ResourceType = resourceType;
        TileID = tileID;
    }

    // Programatically sets the building's resource display
    public void UpdateResourceDisplay(ResourceType resourceType) {
        Material resourceMaterial;
        switch (resourceType) {
            case ResourceType.Nectar:
                resourceMaterial = Resources.Load<Material>("ResourceImage-Nectar");
                break;
            case ResourceType.Pollen:
                resourceMaterial = Resources.Load<Material>("ResourceImage-Pollen");
                break;
            case ResourceType.Buds:
                resourceMaterial = Resources.Load<Material>("ResourceImage-Buds");
                break;
            case ResourceType.Water:
                resourceMaterial = Resources.Load<Material>("ResourceImage-Water");
                break;
            case ResourceType.Honey:
                resourceMaterial = Resources.Load<Material>("ResourceImage-Honey");
                break;
            case ResourceType.Propolis:
                resourceMaterial = Resources.Load<Material>("ResourceImage-Propolis");
                break;
            case ResourceType.RoyalJelly:
                resourceMaterial = Resources.Load<Material>("ResourceImage-RoyalJelly");
                break;
            default:
                Debug.Log("Invalid resource");
                resourceMaterial = null;
                break;
        }
        Renderer resourceDisplayRenderer = gameObject.transform.GetChild(0).GetChild(6).gameObject.GetComponent<Renderer>();
        resourceDisplayRenderer.material = resourceMaterial;
    }

    // Made method static to check if a Building can be afforded based on what is in the inventory
    // After pollen or nectar has been collected to the required amount
    public static bool CanAfford(Dictionary<ResourceType, int> formula, InventoryScriptableObject inventory)
    {
        foreach (ResourceType resource in formula.Keys) {
            if (inventory.GetResourceCount(resource) < formula[resource]) {
                return false;
            }
        }
        return true;
    }
}
