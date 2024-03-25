using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//public class MenuOpenEvent : UnityEvent { }

public class BuildButtonScript : MonoBehaviour
{
    public UnityEvent onButtonClick;
    //public BuildMenuScriptableObject menuState;
    BuildMenuScript buildMenuScript;

    private void Start()
    {
        buildMenuScript = GameObject.FindObjectOfType<BuildMenuScript>(true);
    }

    public void ClickButton()
    {
        onButtonClick.Invoke();
        //menuState.isOpen = true;
        //buildMenuScript.setVisibility();
        buildMenuScript.setOpen();
        Debug.Log("Build button was clicked");
    }



    // Attempting to use a Unity Event 

    //public static MenuOpenEvent onMenuOpen = new MenuOpenEvent();

    //public void OnButtonClick()
    //{
    //    onMenuOpen.Invoke();
    //    Debug.Log("Build button was clicked");
    //}


    // Attempting to create a reference from build button child to build menu child
    //public GameObject BuildMenuScreen;

    // Attempting to create a reference from build button prefab to build menu prefab
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

    // Using the child object to child object idea
    //public void onButtonClick()
    //{
    //    if (BuildMenuScreen != null)
    //    {
    //        BuildMenuScreen.SetActive(true);
    //        Debug.Log("Build button was clicked");
    //    }
    //}


    // USing the prefab reference idea:
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
