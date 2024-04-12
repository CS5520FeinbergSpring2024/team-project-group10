using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



/// <summary>
/// class to update wokersMenu UI and assigned workers to each resource.
/// </summary>
public class WorkersMenuScript : MonoBehaviour
{
    public HiveGameManager gameManager;
    public TextMeshProUGUI NectarAssignedText;
    public TextMeshProUGUI PollenAssignedText;
    public TextMeshProUGUI BudsAssignedText;
    public TextMeshProUGUI WaterAssignedText;
    public TextMeshProUGUI HoneyAssignedText;
    public TextMeshProUGUI PropolisAssignedText;
    public TextMeshProUGUI RoyalJellyAssignedText;
    public TextMeshProUGUI availableWorkersText;

    private HiveScriptable hiveScriptable;
    private Resource resource;
    private GameObject menuButtonObject;
    private ILaunchMenuButton launchMenuButton;
    private static int availableWorkers; //make it static prevent to be destoryed when GameObject deactived.
    private int totalWorkersCount;
    private Dictionary<ResourceType, TextMeshProUGUI> assignedTextMap;
    private Dictionary<ResourceType, int> workersAssigned;


    void Start()
    {

        SetClose();
        LoadData();

        menuButtonObject = GameObject.Find("WorkersMenuObject");
        if (menuButtonObject != null)
        {
            launchMenuButton = menuButtonObject.GetComponent<ILaunchMenuButton>();
            if (launchMenuButton == null)
            {
                Debug.LogError("Button does not implement ILaunchMenuButton interface.");
            }
        }
        else
        {
            Debug.LogError("Menu button object reference not set.");
        }
    }


    private void LoadData()
    {   
        //init game manager
        GameObject GameManagerObject = GameObject.Find("Hive_GameManager");
        if (GameManagerObject != null)
        {
            gameManager = GameManagerObject.GetComponent<HiveGameManager>();
            if (gameManager == null)
            {
                Debug.LogError("HiveGameManager component not found!");
            }
        }

        hiveScriptable = gameManager.hiveScriptable;
        totalWorkersCount = hiveScriptable.GetAvailableWorkers();
        availableWorkersText.text = totalWorkersCount.ToString();
        
        
        //initilize dictionary
        assignedTextMap = new Dictionary<ResourceType, TextMeshProUGUI>
        {
            { ResourceType.Nectar, NectarAssignedText },
            { ResourceType.Pollen, PollenAssignedText },
            { ResourceType.Buds, BudsAssignedText },
            { ResourceType.Water, WaterAssignedText },
            { ResourceType.Honey, HoneyAssignedText },
            { ResourceType.Propolis, PropolisAssignedText },
            { ResourceType.RoyalJelly, RoyalJellyAssignedText }
        };

        workersAssigned = new Dictionary<ResourceType, int>();

        //init local workerAssigned Dictionary avoid frequent updating with HiveScriptable 
        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            workersAssigned.Add(resourceType, 0);
        }

        // check what resource building is available
        LoadBuildingResource();
        

    }


    private void LoadBuildingResource()
    {
        List<Building> buildings = hiveScriptable.GetBuildings();

        foreach (Building building in buildings)
        {
            ResourceType rt = building.ResourceType;
            workersAssigned.Add(rt, hiveScriptable.GetAssignedWorkers(rt));
        }

    }


    public void ClickPlus(ResourceType resourceType)
    {
        if (availableWorkers > 0 && availableWorkers <= totalWorkersCount)
        {
            IncrementAssignedWorkers(assignedTextMap[resourceType], resourceType);
        }
        else
        {
            Debug.Log("No worker available.");
        }
    }


    public void ClickMinus(ResourceType resourceType)
    {
        if (workersAssigned[resourceType] > 0)
        {
            DecrementAssignedWorkers(assignedTextMap[resourceType], resourceType);
        }
        else 
        {
            Debug.Log("No worker available.");
        }
        
    }


    private void IncrementAssignedWorkers(TextMeshProUGUI assignedText, ResourceType resourceType)
    {
        if (hiveScriptable.GetStationLevels(resourceType).productionLevel > 0)
        {
            int assignedValue;
            // Check if the text can be parsed into an integer
            if (int.TryParse(assignedText.text, out assignedValue))
            {
                assignedText.text = (assignedValue + 1).ToString();
                availableWorkers--;
                availableWorkersText.text = availableWorkers.ToString();
                workersAssigned[resourceType]++;

  
            }
        }
        else
        {
            Debug.Log(string.Format("No {0} building exist, please build it first", resourceType));
        }

        
    }


    private void DecrementAssignedWorkers(TextMeshProUGUI assignedText, ResourceType resourceType)
    {
        if (hiveScriptable.GetStationLevels(resourceType).productionLevel > 0)
        {
            int assignedValue;
            // Check if the text can be parsed into an integer
            if (int.TryParse(assignedText.text, out assignedValue) && assignedValue > 0)
            {
                assignedText.text = (assignedValue - 1).ToString();
                availableWorkers++;
                availableWorkersText.text = availableWorkers.ToString();
                workersAssigned[resourceType]--;
            }
        }
        else
        {
            Debug.Log(string.Format("No {0} building exist, please build it first", resourceType));
        }
    }


    public void Assign()
    {
        saveChanges();
        Debug.Log("Workers Assigned");
    }


    public void SetOpen()
    {
        gameObject.SetActive(true);
        Canvas canvas = GetComponentInChildren<Canvas>(true);
        if (canvas != null)
        {
            canvas.gameObject.SetActive(true);
        }
        //update building reousrce everytime it opens
        LoadBuildingResource();
    }


    public void SetClose()
    {
        gameObject.SetActive(false);
    }


    public void saveChanges()
    {
        foreach (var pair in workersAssigned)
        {
            hiveScriptable.AssignWorkers(pair.Key, pair.Value);
        }
       
    }


    public void ExitMenu()
    {   
        saveChanges();
        Canvas canvas = GetComponentInChildren<Canvas>();
        canvas.gameObject.SetActive(false);

        if (launchMenuButton != null)
        {
            launchMenuButton.ReappearButton();
        }
    }

}


