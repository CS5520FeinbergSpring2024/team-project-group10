using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuScript : MonoBehaviour
{
    private GameObject menuButtonObject;
    private ILaunchMenuButton launchMenuButton;
    private Vector2 currentTileID;
    private BuildingType? selectedBuildingType;
    private ResourceType? selectedResourceType;
    public InventoryDataSingleton myInventory;
    public HiveGameManager hiveGameManager;
    private Image[] prevTypeImages = new Image[2];
    private Image prevTypeImage;
    private Image prevSelectedResourceTypeButton;

    // Reference to the future data class 
    //public DataClass buildingData; 

    void Awake()
    {
        myInventory = new InventoryDataSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        setClose();

        menuButtonObject = GameObject.Find("Build Button Object");

        if (menuButtonObject != null)
        {
            launchMenuButton = menuButtonObject.GetComponent<ILaunchMenuButton>();
            if (launchMenuButton == null)
            {
                Debug.LogError("Button does not implement ILaunchMenuButton interface.");
            }
        }
        else
        {
            Debug.LogError("Menu button object reference not set.");
        }

        // Finding the Hive_GameManager object in the scene
        GameObject hiveGameManagerObject = GameObject.Find("Hive_GameManager");
        if (hiveGameManagerObject != null)
        {
            hiveGameManager = hiveGameManagerObject.GetComponent<HiveGameManager>();
            if (hiveGameManager == null)
            {
                Debug.LogError("HiveGameManager component could not be found in Hive_GameManager object.");
            }
        }
        else
        {
            Debug.LogError("Hive_GameManager object not found in the scene.");
        }

    }

    public void setOpen()
    {
        Debug.Log("Menu set to open");
        gameObject.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(true);
    }

    public void setClose()
    {
        gameObject.SetActive(false);
    }

    // Methods to handle the selecting a building
    public void GatheringClick()
    {
        if (selectedBuildingType != BuildingType.Gathering) {
            selectedResourceType = null;
        }
        selectedBuildingType = BuildingType.Gathering;
        Debug.Log("Gathering building selected");
    }

    public void StorageClick()
    {
        if (selectedBuildingType != BuildingType.Storage) {
            selectedResourceType = null;
        }
        selectedBuildingType = BuildingType.Storage;
        Debug.Log("Storage building selected");
    }

    public void ProductionClick()
    {
        if (selectedBuildingType != BuildingType.Production) {
            selectedResourceType = null;
        }
        selectedBuildingType = BuildingType.Production;
        Debug.Log("Production building selected");
    }

    // Methods to handle selecting a resource
    public void NectarResourceClick()
    {
        selectedResourceType = ResourceType.Nectar;
        Debug.Log("Nectar resource selected");
    }
    public void PollenResourceClick()
    {
        selectedResourceType = ResourceType.Pollen;
        Debug.Log("Pollen resource selected");
    }
    public void WaterResourceClick()
    {
        selectedResourceType = ResourceType.Water;
        Debug.Log("Water resource selected");
    }

    public void BudsResourceClick()
    {
        selectedResourceType = ResourceType.Buds;
        Debug.Log("Buds resource selected");
    }

    public void HoneyResourceClick()
    {
        selectedResourceType = ResourceType.Honey;
        Debug.Log("Honey resource selected");
    }

    public void PropolisResourceClick()
    {
        selectedResourceType = ResourceType.Propolis;
        Debug.Log("Propolis resource selected");
    }

    public void RoyalJellyResourceClick()
    {
        selectedResourceType = ResourceType.RoyalJelly;
        Debug.Log("Royal Jelly resource selected");
    }

    public void TypeButtonPressed(Image buttonImage)
    {
        if (prevTypeImage != null)
        {
            prevTypeImage.color = Color.white;
        }
        buttonImage.color = Color.gray;
        prevTypeImage = buttonImage;
    }

    public void ResourceButtonPressed(Image buttonImage)
    {
        if (prevSelectedResourceTypeButton != null)
        {
            prevSelectedResourceTypeButton.color = Color.white;
        }
        buttonImage.color = Color.gray;
        prevSelectedResourceTypeButton = buttonImage;
    }

    public void Build()
    {
        if (selectedBuildingType == null) {
            Debug.LogWarning("No building selected!");
            return;
        }
        if (selectedResourceType == null) {
            Debug.LogWarning("No resource selected!");
            return;
        }
        // Check if building/resource types are a valid match
        if (!Building.buildingResources[(BuildingType)selectedBuildingType].Contains((ResourceType) selectedResourceType)) {
            Debug.LogWarning("Invalid building/resource pair!");
            return;
        }

        // Check if the player can afford to build the selected building and consume those resources
        if (ConsumeResources((BuildingType) selectedBuildingType))
        {
            // Converting the tileID from a Vector2 to a Vector3 for positioning in the world space
            Vector3 position = new Vector3(currentTileID.x, 2f, currentTileID.y);
            
            hiveGameManager.Build((BuildingType) selectedBuildingType, (ResourceType) selectedResourceType, position);
            exitMenu();
            ResetButtonColors();
        }
        else
        {
            Debug.Log("Insufficient resources for this building");
        }
    }

    // Consumes resources required to build the given building type
    // Returns true if succeeded and false if not (due to lack of resources)
    private bool ConsumeResources(BuildingType buildingType) {
        Dictionary<ResourceType, int> formula = Building.buildingFormulas[buildingType];
        if (Building.CanAfford(formula, myInventory)) {
            foreach (ResourceType resource in formula.Keys) {
                myInventory.UpdateInventory(resource, -formula[resource]);
            }
            return true;
        }
        return false;
    }


    public void OpenMenuForTile(Vector2 tileID)
    {   
        currentTileID = tileID; // Store the tile ID

        // Show the build menu    
        setOpen();
    }


    public void exitMenu()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        canvas.gameObject.SetActive(false);
        Debug.Log("Exit button was clicked");

        if (launchMenuButton != null)
        {
            launchMenuButton.ReappearButton();
        }
        ResetButtonColors();
    }

    private void ResetButtonColors()
    {
        if (prevTypeImage != null)
        {
            prevTypeImage.color = Color.white;
        }
        if (prevSelectedResourceTypeButton != null)
        {
            prevSelectedResourceTypeButton.color = Color.white;
        }
    }

}
