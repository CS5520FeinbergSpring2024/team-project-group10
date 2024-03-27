using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenuScript : MonoBehaviour
{
    private List<Building> buildingsList;
    private List<ResourceType> resourceList;
    private Tile currentTile;
    public Building selectedBuilding;
    public Resource selectedResource;
    private Canvas canvas;
    // Reference to the future data class 
    //public DataClass buildingData; 

    // Start is called before the first frame update
    void Start()
    {
        loadData();
        canvas = GetComponentInParent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {

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
        Debug.Log("Selected building: " + building.name);
    }

    // Function to handle selecting a resource
    public void SelectResource(Resource resource)
    {
        selectedResource = resource;
        Debug.Log("Selected resource: " + resource);
    }

    public void Build()
    {
        if (selectedBuilding != null)
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
        canvas.gameObject.SetActive(true);
    }


    public void exitMenu()
    {
        //Canvas canvas = GetComponentInParent<Canvas>();
        canvas.gameObject.SetActive(false);
        Debug.Log("Exit button was clicked");

        saveChanges();
    }
}
