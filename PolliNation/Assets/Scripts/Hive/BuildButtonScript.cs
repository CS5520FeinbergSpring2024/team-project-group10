using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class BuildButtonScript : MonoBehaviour, ILaunchMenuButton
{
    BuildMenuScript buildMenuScript;
    Image buildButtonImage;

    void Start()
    {
        buildMenuScript = GameObject.FindObjectOfType<BuildMenuScript>(true);
        buildButtonImage = GetComponentInChildren<Image>();
    }

    public void ClickButton()
    {
        Debug.Log("ClickButton method called");
        buildMenuScript.setOpen();
        Debug.Log("Build button was clicked");

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


    // Update is called once per frame
    void Update()
    {

    }

}