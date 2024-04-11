using UnityEngine;

public class CancelBuild : MonoBehaviour
{
    public HiveGameManager hiveGameManager;

    // Start is called before the first frame update
    void Start()
    {
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

    // Exits the building mode designated by the hive game manager without opening the build menu
    void OnMouseDown() {
        if (hiveGameManager.building) {
            hiveGameManager.building = false;

            BuildButtonScript buildButton = FindObjectOfType<BuildButtonScript>(true);
            if (buildButton != null) 
            {
                buildButton.ReappearButton();
            }
        }
    }
}
