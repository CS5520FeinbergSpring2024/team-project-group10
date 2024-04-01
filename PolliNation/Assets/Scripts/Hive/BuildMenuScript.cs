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
    private Tile currentTile;
    private BuildingType selectedBuildingType;
    private ResourceType selectedResourceType;
    public Building selectedBuilding;
    public GameObject buildingGatheringPrefab;
    public GameObject buildingStoragePrefab;
    public GameObject buildingProductionPrefab;
    public Resource selectedResource;
    public InventoryScriptableObject myInventory;

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
    public void SelectResource(Resource resource)
    {
        selectedResource = resource;
        Debug.Log("Selected resource: " + resource);
    }

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
        Debug.Log("Nectar resource selected");
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

                GameObject newBuildingObject = Instantiate(buildingGatheringPrefab, position, Quaternion.identity);

                // Get the Building component from the prefab
                Building newBuilding = newBuildingObject.GetComponent<Building>();

                // Associate the building with the selected resource 
                if (newBuilding != null)
                {
                    newBuilding.ResourceType = selectedResourceType;
                    Debug.Log("Building instantiated: " + selectedBuildingType + " with resource: " + selectedResourceType);
                }
                else
                {
                    Debug.LogError("Failed to get Building component from instantiated object.");
                }
            }
            else
            {
                Debug.Log("Insufficient resources for this building");
            }
        
           
        }else if(selectedBuildingType == BuildingType.Storage)
        {
            if (Building.CanAfford(BuildingType.Gathering, myInventory))
            {
                // Temp position for instantiation
                Vector3 position = new Vector3(0, 0, 0);

                GameObject newBuildingObject = Instantiate(buildingStoragePrefab, position, Quaternion.identity);

                
                Building newBuilding = newBuildingObject.GetComponent<Building>();

     
                if (newBuilding != null)
                {
                    newBuilding.ResourceType = selectedResourceType;
                    Debug.Log("Building instantiated: " + selectedBuildingType + " with resource: " + selectedResourceType);
                }
                else
                {
                    Debug.LogError("Failed to get Building component from instantiated object.");
                }
            }
            else
            {
                Debug.Log("Insufficient resources for this building");
            }

        }
        else if(selectedBuildingType == BuildingType.Production)
        {
            if (Building.CanAfford(BuildingType.Gathering, myInventory))
            {
                // Temp position for instantiation
                Vector3 position = new Vector3(0, 0, 0);

                GameObject newBuildingObject = Instantiate(buildingProductionPrefab, position, Quaternion.identity);
                
                Building newBuilding = newBuildingObject.GetComponent<Building>();

                
                if (newBuilding != null)
                {
                    newBuilding.ResourceType = selectedResourceType;
                    Debug.Log("Building instantiated: " + selectedBuildingType + " with resource: " + selectedResourceType);
                }
                else
                {
                    Debug.LogError("Failed to get Building component from instantiated object.");
                }
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
        // tile use for bind building by tile.SetCurrentBuilding() method
        currentTile = tile;

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
