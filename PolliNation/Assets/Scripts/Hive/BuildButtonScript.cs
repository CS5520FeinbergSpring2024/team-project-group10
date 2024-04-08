using UnityEngine;
using UnityEngine.UI;


public class BuildButtonScript : MonoBehaviour, ILaunchMenuButton
{
    BuildMenuScript buildMenuScript;
    public HiveGameManager hiveGameManager;
    Image buildButtonImage;

    void Start()
    {
        buildMenuScript = GameObject.FindObjectOfType<BuildMenuScript>(true);
        buildButtonImage = GetComponentInChildren<Image>();

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

    public void ClickButton()
    {
        Debug.Log("Build button was clicked");
        hiveGameManager.building = true;

        // Make the BuildButtonImage invisible
        if (buildButtonImage != null)
        {
            buildButtonImage.enabled = false;
        }
    }

    public void ReappearButton()
    {
        // Make the BuildButtonImage visible again
        if (buildButtonImage != null)
        {
            buildButtonImage.enabled = true;
        }
    }

}
