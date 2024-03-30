using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Receive buildMenuScript class, load building has
/// already been set up when start. 
/// 
/// Provide:
/// 
/// public method OpenBuildMenu() for click button 
/// allows open buildMenu.
/// 
/// public method SetCurrentBuilding() allows BuildMenuScript
/// class access and bind building to the tile.
/// 
/// </summary>
public class Tile : MonoBehaviour
{
    private BuildMenuScript buildMenu;
    private Building currentBuilding;
    //private DataClass buildingData;

    // Start is called before the first frame update
    void Start()
    {
        LoadBuilding();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //logic to load building data create building prefab
    void LoadBuilding()
    {
        /*if (buildingData != null)
        {
            //load current building binded to this tile
            currentBuilding = buildingData.GetBuildingForTile(tileId);

            if (currentBuilding != null)
            {
                BuildCurrentBuilding();
            }

        }*/
    }

    public void OpenBuildMenu() 
    {
        buildMenu = GameObject.FindObjectOfType<BuildMenuScript>(true);
        if (buildMenu != null) 
        {   
            //pass current tile to the buildMenu and open the buildMenu screen
            buildMenu.OpenMenuForTile(this);
        }
       
    }

    //bind specific building to the tile
    public void SetCurrentBuilding(Building building)
    {
        currentBuilding = building;
    }


    //instantiating a building prefab at the position of the tile in the game world
    private void BuildCurrentBuilding()
    {
        if (currentBuilding != null)
        { 
            //Instantiate(currentBuilding, transform.position, Quaternion.identity);
        }
    }



}
