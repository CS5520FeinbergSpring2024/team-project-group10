using System;
using System.Collections.Generic;
using System.Linq;
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
    private GameObject menuButtonObject;
    private ILaunchMenuButton launchMenuButton;
    private int availableWorkers;
    private Dictionary<ResourceType, TextMeshProUGUI> assignedTextMap;
    private Dictionary<ResourceType, int> workersAssigned;


    void Start()
    {
        SetClose();

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

        // init local workerAssigned Dictionary avoid frequent updating with HiveScriptable 
        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            workersAssigned.Add(resourceType, 0);
        }

        LoadData();
    }


    private void LoadData()
    {
        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            workersAssigned[resourceType] = hiveScriptable.GetAssignedWorkers(resourceType);
            assignedTextMap[resourceType].text = workersAssigned[resourceType].ToString();
        }
        availableWorkers = hiveScriptable.GetTotalWorkers() - workersAssigned.Sum(x => x.Value);
        availableWorkersText.text = availableWorkers.ToString();
    }


    public void ClickPlus(ResourceType resourceType)
    {
        if (availableWorkers > 0)
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
            workersAssigned[resourceType]++;
            assignedText.text = workersAssigned[resourceType].ToString();
            availableWorkers--;
            availableWorkersText.text = availableWorkers.ToString();
        }
        else
        {
            Debug.Log(string.Format("No {0} building exist, please build it first", resourceType));
        }
    }


    private void DecrementAssignedWorkers(TextMeshProUGUI assignedText, ResourceType resourceType)
    {
            workersAssigned[resourceType]--;
            assignedText.text = workersAssigned[resourceType].ToString();
            availableWorkers++;
            availableWorkersText.text = availableWorkers.ToString();
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
        LoadData();
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


