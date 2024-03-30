using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceButtonHandler : MonoBehaviour
{
    private Button myButton;
    private ResourceType resourceType;
    private WorkersMenuScript workersMenu;

    // Start is called before the first frame update
    void Start()
    {
        myButton = GetComponent<Button>();
        resourceType = (ResourceType)Enum.Parse(typeof(ResourceType), this.name);
        workersMenu = FindObjectOfType<WorkersMenuScript>();

    }


    public void ClickPlus()
    {
        workersMenu.ClickPlus(resourceType);
    }

    public void ClickMinus()
    { 
        workersMenu.ClickMinus(resourceType);
    }
}
