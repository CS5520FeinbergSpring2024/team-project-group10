using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// I used setActive method to make build menu show or not,
/// which is faster but cost more memory.
/// If use setActive method to hide buildMenu at start, should we hide it in anywhere else ? 
/// </summary>
public class Tile : MonoBehaviour
{
    public GameObject buildMenu;
    public GameObject self;
    // Start is called before the first frame update
    void Start()
    {   
       //hide the buildMenu at first
       buildMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenBuildMenu() 
    {
        if (buildMenu != null) 
        {   
            //show 
            buildMenu.SetActive (true);
        }
    }
}
