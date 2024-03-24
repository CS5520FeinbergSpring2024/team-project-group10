using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuOpenEvent : UnityEvent { }
public class BuildButtonScript : MonoBehaviour
{
    public static MenuOpenEvent onMenuOpen = new MenuOpenEvent();

    public void OnButtonClick()
    {
        onMenuOpen.Invoke();
        Debug.Log("Build button was clicked");
    }
    //public GameObject BuildMenuScreen;
    //public GameObject buildmenuPrefab;
    //private GameObject buildmenuInstance;
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    //public void onButtonClick()
    //{
    //    if (BuildMenuScreen != null)
    //    {
    //        BuildMenuScreen.SetActive(true);
    //        Debug.Log("Build button was clicked");
    //    }
    //}

    ////public void onButtonClick()
    ////{
    ////    if (buildmenuPrefab != null)
    ////    {
    ////        if (buildmenuInstance == null) // Check if an instance already exists
    ////        {
    ////            buildmenuInstance = Instantiate(buildmenuPrefab);
    ////        }
    ////        else
    ////        {
    ////            buildmenuInstance.SetActive(true); // Activate the existing instance
    ////        }

    ////        Debug.Log("Build button was clicked");
    ////    }
    ////    else
    ////    {
    ////        Debug.LogWarning("No reference to menu prefab");
    ////    }
    ////}
}
