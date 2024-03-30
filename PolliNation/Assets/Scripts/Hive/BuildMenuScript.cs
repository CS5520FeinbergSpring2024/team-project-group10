using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class BuildMenuScript : MonoBehaviour
{
    private GameObject menuButtonObject;
    private GameObject buildingImageObject;
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

    // Function to handle selecting a building
    public void SelectBuilding(Building building)
    {
        selectedBuilding = building;
        Debug.Log("Selected building: " + building.Type);
    }

    // Separate methods to handle the building image clicks
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


    // Wrapper function for building button choise
    //public void HandleBuildingClick(BaseEventData eventData)
    //{
    //    PointerEventData pointerEventData = eventData as PointerEventData;
    //    if (pointerEventData != null)
    //    {
    //        GameObject clickedImage = EventSystem.current.currentSelectedGameObject;
    //        //string buttonName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

    //        if (clickedImage != null)
    //        {
    //            string buttonName = clickedImage.name;
    //            Debug.Log("The button name is:" + buttonName);
    //            switch (buttonName)
    //            {
    //                case "Storage Building Image":
    //                    selectedBuildingType = BuildingType.Storage;
    //                    Debug.Log("Storage building selected");
    //                    break;
    //                case "Gathering Building Image":
    //                    selectedBuildingType = BuildingType.Gathering;
    //                    Debug.Log("Gathering building selected");
    //                    break;
    //                case "Conversion Building Image":
    //                    selectedBuildingType = BuildingType.Production;
    //                    Debug.Log("Production building selected");
    //                    break;
    //                default:
    //                    Debug.LogError("That is not a valid button");
    //                    break;
    //            }

    //        }
    //        else
    //        {
    //            Debug.LogWarning("No game object image was selected");
    //        }

    //    }
    //    else
    //    {
    //        Debug.LogWarning("No PointerEventData received");
    //    }

    //} 


    // Simplified wrapper function to try to consolidate handle click for buildings
    public void HandleBuildingClick(GameObject clickedObject)
    {
        string buildingName = clickedObject.name;
        if (buildingName.Contains("Gathering"))
        {
            selectedBuildingType = BuildingType.Gathering;
            Debug.Log("Gathering building selected");
        }else if (buildingName.Contains("Storage"))
        {
            selectedBuildingType = BuildingType.Storage;
            Debug.Log("Storage building selected");
        }
        else
        {
            Debug.LogError("Invalid building type selected");
        }
    }

    public void OnImageClick()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        if (clickedObject != null)
        {
            HandleBuildingClick(clickedObject);
        }
        else
        {
            Debug.LogError("No GameObject clicked");
        }
    }


    // Function to handle selecting a resource
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

    public void Build()
    {
        if (selectedBuildingType == BuildingType.Gathering)
        {
            // Check if the player can afford to build the selected building
            // Commenting for now for future implementation of methods in Building class


            //if (selectedBuilding.CanAfford())
            //{
            //    // Instantiate the selected building at its specified position
            //    Instantiate(selectedBuilding, selectedBuilding.Position, Quaternion.identity);
            //    Debug.Log("Building instantiated: " + selectedBuilding.name);
            //}
            //else
            //{
            //    Debug.LogWarning("Cannot afford to build: " + selectedBuilding.name);
            //}

            Building selectedBuilding = new Building(selectedBuildingType, 0, selectedResourceType, new Vector3(0,0,0));
            if (selectedBuilding.CanAfford(BuildingType.Gathering))
            {
                // Temp position for instantiation
                Vector3 position = new Vector3(0, 0, 0);
                Instantiate(buildingGatheringPrefab, position, Quaternion.identity);
                Debug.Log("Building instantiated: " + selectedBuildingType);
            }
           
        }else if(selectedBuildingType == BuildingType.Storage)
        {
            Building selectedBuilding = new Building(selectedBuildingType, 0, selectedResourceType, new Vector3(0, 0, 0));
            if (selectedBuilding.CanAfford(BuildingType.Storage))
            {
                Vector3 position = new Vector3(0, 0, 0);
                Instantiate(buildingStoragePrefab, position, Quaternion.identity);
                Debug.Log("Building instantiated: " + selectedBuildingType);
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
