using UnityEngine;
using System;

/// <summary>
/// Provide:
/// 
/// public method OpenBuildMenu() for click button 
/// allows open buildMenu.
/// 
/// </summary>
public class Tile : MonoBehaviour
{
    private BuildMenuScript buildMenu;
    private DestroyMenuScript destroyMenu;
    public HiveGameManager hiveGameManager;
    public Vector3 tilePosition;
    private bool occupied = false; // used to reduce usage of IsOccupied()

    // Fields related to changing tile color
    private GameObject hexagon;
    private bool isYellow = true; // used to track color and reduce usage of IsOccupied()
    [SerializeField] private Material yellowMat;
    [SerializeField] private Material greenMat;
    [SerializeField] private Material redMat;

    // To play sound on click.
    internal Action playSoundOnClick;

    // Start is called before the first frame update
    void Start() {
        //  Reference to hexagon object to change its material
        hexagon = gameObject.transform.GetChild(0).gameObject;

        // Set tilePosition as this tile's position with the y-value set to make buildings appear on top of tile
        tilePosition = new Vector3(transform.position.x, 2f, transform.position.z);
        
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
        buildMenu = FindObjectOfType<BuildMenuScript>(true);
        destroyMenu = FindObjectOfType<DestroyMenuScript>(true);
    }

    void Update() {
        // Update tile color based on whether it is available for building
        if (hiveGameManager.building && !occupied && isYellow) {
            isYellow = false;
            if (hiveGameManager.IsOccupied(tilePosition)) {
                occupied = true;
                hexagon.GetComponent<Renderer>().material = redMat;
                return;
            }
            hexagon.GetComponent<Renderer>().material = greenMat;
        } else if (!hiveGameManager.building && !isYellow) {
            isYellow = true;
            occupied = false; // reset to ensure tile turns red
            hexagon.GetComponent<Renderer>().material = yellowMat;
        }
    }

    public void OpenBuildMenu() {
        if (buildMenu != null) {
            buildMenu.OpenMenuForTile(tilePosition);
        }
       
    }

    public void OpenDestroyMenu() {
        if (destroyMenu != null) {
            destroyMenu.OpenMenuForTile(tilePosition);
        }
    }

    // Opens build menu when in building mode and tile is available
    void OnMouseDown() {
        if (hiveGameManager.building && !isYellow) {
            hiveGameManager.building = false;

            if (playSoundOnClick != null)
            {
                playSoundOnClick();
            }

            if (occupied) {
                OpenDestroyMenu();
            } else {
                OpenBuildMenu();
            }
        }
    }
}
