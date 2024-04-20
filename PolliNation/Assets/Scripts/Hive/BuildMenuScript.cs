using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuScript : MonoBehaviour {
    private ILaunchMenuButton launchMenuButton;
    private Vector3 tilePosition;
    private BuildingType? selectedBuildingType;
    private ResourceType? selectedResourceType;
    public InventoryDataSingleton myInventory;
    public HiveGameManager hiveGameManager;
    public SnackbarScript snackbar;
    private Image prevTypeImage;
    private Image prevSelectedResourceTypeButton;

    // Static dictionary containing the resources that can be associated with each building type
    public static Dictionary<BuildingType, List<ResourceType>> buildingResources = new() {
        { BuildingType.Storage, new List<ResourceType>() {
                ResourceType.Nectar,
                ResourceType.Pollen,
                ResourceType.Buds,
                ResourceType.Water,
                ResourceType.Honey,
                ResourceType.Propolis,
                ResourceType.RoyalJelly
            }
        },
        { BuildingType.Gathering, new List<ResourceType>() {
                ResourceType.Nectar,
                ResourceType.Pollen,
                ResourceType.Buds,
                ResourceType.Water
            }
        },
        { BuildingType.Production, new List<ResourceType>() {
                ResourceType.Honey,
                ResourceType.Propolis,
                ResourceType.RoyalJelly
            }
        },
    };

    // Static dictionary containing resource requirements for each building.
    // Used to check whether building can be built and to consume corresponding resources
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

    void Awake() {
        myInventory = new InventoryDataSingleton();
    }

    // Start is called before the first frame update
    void Start() {
        SetClose();
        
        // Finding the Build Button object in the scene
        GameObject menuButtonObject = GameObject.Find("Build Button Object");
        if (menuButtonObject != null) {
            launchMenuButton = menuButtonObject.GetComponent<ILaunchMenuButton>();
            if (launchMenuButton == null) {
                Debug.LogError("Button does not implement ILaunchMenuButton interface.");
            }
        }
        else {
            Debug.LogError("Menu button object reference not set.");
        }

        // Finding the Hive_GameManager object in the scene
        GameObject hiveGameManagerObject = GameObject.Find("Hive_GameManager");
        if (hiveGameManagerObject != null) {
            hiveGameManager = hiveGameManagerObject.GetComponent<HiveGameManager>();
            if (hiveGameManager == null) {
                Debug.LogError("HiveGameManager component could not be found in Hive_GameManager object.");
            }
        }
        else {
            Debug.LogError("Hive_GameManager object not found in the scene.");
        }
        
        // Finding the Snackbar object in the scene
        GameObject snackbarObject = GameObject.Find("Snackbar");
        if (snackbarObject != null) {
            snackbar = snackbarObject.transform.GetChild(0).GetChild(0).GetComponent<SnackbarScript>();
            if (snackbarObject == null) {
                Debug.LogError("Snackbar component could not be found.");
            }
        }
        else {
            Debug.LogError("Snackbar object not found in the scene.");
        }
    }

    public void SetOpen() {
        gameObject.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(true);
    }

    public void SetClose() {
        gameObject.SetActive(false);
    }

    // Methods to handle the selecting a building
    public void GatheringClick() {
        if (selectedBuildingType != BuildingType.Gathering) {
            selectedResourceType = null;
        }
        selectedBuildingType = BuildingType.Gathering;
        Debug.Log("Gathering building selected");
        if (prevSelectedResourceTypeButton != null && !prevSelectedResourceTypeButton.name.Contains("Gathering"))
        {   
           prevSelectedResourceTypeButton.color = Color.white;
        }
    }

    public void StorageClick() {
        if (selectedBuildingType != BuildingType.Storage) {
            selectedResourceType = null;
        }
        selectedBuildingType = BuildingType.Storage;
        Debug.Log("Storage building selected");
        if (prevSelectedResourceTypeButton != null && !prevSelectedResourceTypeButton.name.Contains("Storage"))
        {   
           prevSelectedResourceTypeButton.color = Color.white;
        }
    }

    public void ProductionClick() {
        if (selectedBuildingType != BuildingType.Production) {
            selectedResourceType = null;
        }
        selectedBuildingType = BuildingType.Production;
        Debug.Log("Production building selected");
        if (prevSelectedResourceTypeButton != null && !prevSelectedResourceTypeButton.name.Contains("Conversion"))
        {   
           prevSelectedResourceTypeButton.color = Color.white;
        }
    }

    // Methods to handle selecting a resource
    public void NectarResourceClick() {
        selectedResourceType = ResourceType.Nectar;
        Debug.Log("Nectar resource selected");
    }
    public void PollenResourceClick() {
        selectedResourceType = ResourceType.Pollen;
        Debug.Log("Pollen resource selected");
    }
    public void WaterResourceClick() {
        selectedResourceType = ResourceType.Water;
        Debug.Log("Water resource selected");
    }

    public void BudsResourceClick() {
        selectedResourceType = ResourceType.Buds;
        Debug.Log("Buds resource selected");
    }

    public void HoneyResourceClick() {
        selectedResourceType = ResourceType.Honey;
        Debug.Log("Honey resource selected");
    }

    public void PropolisResourceClick() {
        selectedResourceType = ResourceType.Propolis;
        Debug.Log("Propolis resource selected");
    }

    public void RoyalJellyResourceClick() {
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

    public void Build() {
        if (selectedBuildingType == null) {
            Debug.LogWarning("No building selected!");
            snackbar.SetText("No building selected!");
            return;
        }
        if (selectedResourceType == null) {
            Debug.LogWarning("No resource selected!");
            snackbar.SetText("No resource selected!");
            return;
        }
        // Check if building/resource types are a valid match
        if (!buildingResources[(BuildingType)selectedBuildingType].Contains((ResourceType) selectedResourceType)) {
            Debug.LogWarning("Invalid building/resource pair!");
            snackbar.SetText("Invalid building/resource pair!");
            return;
        }

        // Check if the player can afford to build the selected building and consume those resources
        if (ConsumeResources((BuildingType) selectedBuildingType)) {
            // Converting the tileId from a Vector2 to a Vector3 for positioning in the world space
            hiveGameManager.Build((BuildingType) selectedBuildingType, (ResourceType) selectedResourceType, tilePosition);
            ExitMenu();
        }
        else {
            Debug.Log("Insufficient resources for this building");
            snackbar.SetText("Insufficient resources for this building", 3);
        }
    }

    // Consumes resources required to build the given building type
    // Returns true if succeeded and false if not (due to lack of resources)
    private bool ConsumeResources(BuildingType buildingType) {
        Dictionary<ResourceType, int> formula = buildingFormulas[buildingType];
        if (HiveGameManager.CanAfford(formula, myInventory)) {
            foreach (ResourceType resource in formula.Keys) {
                myInventory.UpdateInventory(resource, -formula[resource]);
            }
            return true;
        }
        return false;
    }

    public void OpenMenuForTile(Vector3 tilePosition) {   
        this.tilePosition = tilePosition; // Store the tile position
        SetOpen(); // Show the build menu 
    }

    public void ExitMenu() {
        Canvas canvas = GetComponentInParent<Canvas>();
        canvas.gameObject.SetActive(false);
        Debug.Log("Exit button was clicked");
        launchMenuButton?.ReappearButton();

        // Removes selections
        selectedBuildingType = null;
        selectedResourceType = null;
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