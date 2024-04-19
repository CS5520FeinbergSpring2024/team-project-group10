using UnityEngine;

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
    public Vector2 tileID;
    private bool occupied = false; // used to reduce usage of IsOccupied()

    // Fields related to changing tile color
    private GameObject hexagon;
    private bool isYellow = true; // used to track color and reduce usage of IsOccupied()
    [SerializeField] private Material yellowMat;
    [SerializeField] private Material greenMat;
    [SerializeField] private Material redMat;

    // Start is called before the first frame update
    void Start() {
        //  Reference to hexagon object to change its material
        hexagon = gameObject.transform.GetChild(0).gameObject;

        // Set tileID as this tile's x and z coordinates as a Vector2
        tileID = new Vector2(transform.position.x, transform.position.z);
        
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
            if (hiveGameManager.IsOccupied(tileID)) {
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

    public void OpenBuildMenu(Vector2 tileID) {
        if (buildMenu != null) {
            buildMenu.OpenMenuForTile(tileID);
        }
       
    }

    public void OpenDestroyMenu(Vector2 tileID) {
        if (destroyMenu != null) {
            destroyMenu.OpenMenuForTile(tileID);
        }
    }

    // Opens build menu when in building mode and tile is available
    void OnMouseDown() {
        if (hiveGameManager.building && !isYellow) {
            Debug.Log(tileID);
            hiveGameManager.building = false;

            if (occupied) {
                OpenDestroyMenu(tileID);
            } else {
                OpenBuildMenu(tileID);
            }
        }
    }
}
