using UnityEngine;
using TMPro;

public class DestroyMenuScript : MonoBehaviour
{
    private ILaunchMenuButton launchMenuButton;
    private HiveGameManager hiveGameManager;
    [SerializeField] TextMeshProUGUI question;
    private Vector2 tileId;

    // Start is called before the first frame update
    void Start() {
        // Finding the Build Button object object in the scene
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

        SetClose();
    }

    // Open Destroy menu and update contents with tileId's associated building
    public void OpenMenuForTile(Vector2 tileId) {
        Debug.Log("Menu set to open");
        this.tileId = tileId;
        gameObject.SetActive(true);

        foreach (Building b in hiveGameManager.hiveSingleton.GetBuildings()) {
            if (b.TileID == tileId) {
                // RoyalJelly should be spaced out
                string resourceType = b.ResourceType == ResourceType.RoyalJelly ? "Royal Jelly" : b.ResourceType.ToString();
                // Other menus use "Conversion" instead of "Production"
                string buildingType = b.Type == BuildingType.Production ? "Conversion" : b.Type.ToString();
                question.text = $"Destroy {resourceType} {buildingType} Station?";
                break;
            }
        }
    }

    // Close the Destroy menu and make build button reappear
    public void SetClose() {
        launchMenuButton.ReappearButton();
        gameObject.SetActive(false);
    }

    // Destroy building then close the menu
    public void YesPressed() {
        hiveGameManager.DestroyBuilding(tileId);
        SetClose();
    }
    
    // Close menu without any other action
    public void NoPressed() {
        SetClose();
    }
}
