using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used setActive method to make build menu show or not,
/// which is faster but cost more memory.
/// </summary>
public class Tile : MonoBehaviour
{
    public GameObject buildMenuCanvas;
    public GameObject self;
    // Start is called before the first frame update
    void Start()
    {   
       //hide the buildMenu at first
       buildMenuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenBuildMenu() 
    {
        if (buildMenuCanvas != null) 
        {
            //show 
            buildMenuCanvas.SetActive(true);
        }
    }
}
