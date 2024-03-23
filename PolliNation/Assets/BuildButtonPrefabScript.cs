using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildButtonPrefabScript : MonoBehaviour
{
    public GameObject buildmenuPrefab;
    private GameObject buildmenuInstance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onButtonClick()
    {
        if (buildmenuPrefab != null)
        {
            if (buildmenuInstance == null) // Check if an instance already exists
            {
                buildmenuInstance = Instantiate(buildmenuPrefab);
            }
            else
            {
                buildmenuInstance.SetActive(true); // Activate the existing instance
            }

            Debug.Log("Build button was clicked");
        }
        else
        {
            Debug.LogWarning("No reference to menu prefab");
        }
    }
}
