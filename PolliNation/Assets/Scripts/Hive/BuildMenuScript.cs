using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.ComponentModel;

public class BuildMenuScript : MonoBehaviour
{
    private GameObject menuButtonObject;
    private ILaunchMenuButton launchMenuButton;
    private List<Building> buildingsList;
    private List<ResourceType> resourceList;
    private Vector2 currentTileID;
    private BuildingType selectedBuildingType;
    private ResourceType selectedResourceType;
    public Building selectedBuilding;
    public GameObject buildingGatheringPrefab;
    public GameObject buildingStoragePrefab;
    public GameObject buildingProductionPrefab;
    public Resource selectedResource;
    public InventoryScriptableObject myInventory;
    public HiveGameManager hiveGameManager;

    // Reference to the future data class 
    //public DataClass buildingData; 

    // Start is called before the first frame update
    void Start()
    {
        setClose();
        loadData();


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

    // Update is called once per frame
    void Update()
    {

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


    public void loadData()
    {
        // Loading the data from future data class
        // Commenting for now for future implementation of Data class

        //if (buildingData != null)
        //{
        //    // Load buildings and resources data from the data class
        //    buildingsList = buildingData.LoadBuildings();
        //    resourceList = buildingData.LoadResources();
        //    Debug.Log("Data loaded successfully!");
        //}
        //else
        //{
        //    Debug.LogWarning("Data class is null");
        //}

    }

    public void saveChanges()
    {
        // Saving data to the future data class
        // Commenting for now for future implementation of Data class

        //if (buildingData != null)
        //{
        //    // Save buildings and resources data to the data class
        //    buildingData.SaveBuildings(buildingsList);
        //    buildingData.SaveResources(resourceList);
        //    Debug.Log("Changes saved");
        //}
        //else
        //{
        //    Debug.LogWarning("Changes not saved");
        //}
    }


    // Methods to handle the selecting a building
    public void GatheringClick()
    {
        selectedBuildingType = BuildingType.Gathering;
        Debug.Log("Gathering building selected");
    }

    public void StorageClick()
    {
        selectedBuildingType = BuildingType.Storage;
        Debug.Log("Storage building selected");
    }

    public void ProductionClick()
    {
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

    public void Build()
    {
        if (selectedBuildingType == BuildingType.Gathering)
        {
            // Check if the player can afford to build the selected building
            
            if (Building.CanAfford(BuildingType.Gathering, myInventory))
            {
                // Temp placeholder position for building prefab instantiation
                Vector3 position = new Vector3(0, 0, 0);

                // Converting the tileID from a Vector2Int to a Vector3 for positioning in the world space
                // Commented for now because it needs a Tile game object to get its position in 3D
                // The current tile is an image on a canvas so I don't think it doesnt exist in 3D space
                // to base another 3D object's position off of
                //Vector3 position = new Vector3(currentTileID.x, 0f, currentTileID.y);
                
                hiveGameManager.Build(selectedBuildingType, selectedResourceType, position);
            }
            else
            {
                Debug.Log("Insufficient resources for this building");
            }
        
           
        }else if(selectedBuildingType == BuildingType.Storage)
        {
            if (Building.CanAfford(BuildingType.Storage, myInventory))
            {
                // Temp position for instantiation
                Vector3 position = new Vector3(5, 0, 0);

                // Converting the tileID from a Vector2Int to a Vector3 for positioning in the world space
                //Vector3 position = new Vector3(currentTileID.x, 0f, currentTileID.y);

                hiveGameManager.Build(selectedBuildingType, selectedResourceType, position);
            }
            else
            {
                Debug.Log("Insufficient resources for this building");
            }

        }
        else if(selectedBuildingType == BuildingType.Production)
        {
            if (Building.CanAfford(BuildingType.Production, myInventory))
            {
                // Temp position for instantiation
                Vector3 position = new Vector3(10, 0, 0);

                // Converting the tileID from a Vector2Int to a Vector3 for positioning in the world space
                //Vector3 position = new Vector3(currentTileID.x, 0f, currentTileID.y);

                hiveGameManager.Build(selectedBuildingType, selectedResourceType, position);
            }
            else
            {
                Debug.Log("Insufficient resources for this building");
            }
        }
        else
        {
            Debug.LogWarning("No building selected!");
        }
    }


    public void OpenMenuForTile(Tile tile)
    {   
        currentTileID = tile.tileID; // Store the tile ID

        // Show the build menu    
        setOpen();
    }


    public void exitMenu()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        canvas.gameObject.SetActive(false);
        Debug.Log("Exit button was clicked");

        saveChanges();


        if (launchMenuButton != null)
        {
            launchMenuButton.ReappearButton();
        }
    }

}
