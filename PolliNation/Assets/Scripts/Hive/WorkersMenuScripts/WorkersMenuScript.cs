using System.Collections.Generic;
using TMPro;
using UnityEngine;



/// <summary>
/// class to update wokersMenu UI and assigned workers to each resource.
/// </summary>
public class WorkersMenuScript : MonoBehaviour
{
    //public HiveScriptable hiveScriptableObject;
    public TextMeshProUGUI NectarAssignedText;
    public TextMeshProUGUI PollenAssignedText;
    public TextMeshProUGUI BudsAssignedText;
    public TextMeshProUGUI WaterAssignedText;
    public TextMeshProUGUI HoneyAssignedText;
    public TextMeshProUGUI PropolisAssignedText;
    public TextMeshProUGUI RoyalJellyAssignedText;
    public TextMeshProUGUI availableWorkersText;

    private Resource resource;
    private GameObject menuButtonObject;
    private ILaunchMenuButton launchMenuButton;
    //private static int availableWorkers; //make it static prevent to be destoryed when GameObject deactived.
    //private int totalworkerCount;
    //private DataClass dataClass;

    private Dictionary<ResourceType, TextMeshProUGUI> assignedTextMap;
    private Dictionary<ResourceType, int> workersAssigned;
    private HashSet<ResourceType> buildingSet;

    // use for test only
    private static int availableWorkers = 5;
    private int totalworkerCount;
    /// ///////////////////////////////


    void Start()
    {
        //SetClose();
        LoadData();

        /*menuButtonObject = GameObject.Find("WorkersMenuButton");

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
        }*/
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadData()
    {
        /*if (dataClass != null)
        {   
            resource = dataClass.LoadResource();
            totalWorkersCount = resource.GetNumWorkers();
            availableWorkersText.text = totalWorkersCount.ToString();
            
        }*/

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

        buildingSet = new HashSet<ResourceType>();
        workersAssigned = new Dictionary<ResourceType, int>();

        availableWorkersText.text = availableWorkers.ToString();
        totalworkerCount = availableWorkers;

        // check what resource building is available
        LoadBuildingResource();
        

    }


    private void LoadBuildingResource()
    {
        /*List<Building> buildings = hiveScriptableObject.GetBuildings();

        foreach (Building building in buildings)
        {   
            ResourceType rt = building.GetResourceType();
            buildingSet.Add(rt);
            workersAssigned.Add(rt, hiveScriptableObject.GetAssignedWorker(rt));
        }*/

    }


    public void ClickPlus(ResourceType resourceType)
    {
        if (availableWorkers > 0 && availableWorkers <= totalworkerCount)
        {
            IncrementAssignedWorkers(assignedTextMap[resourceType], resourceType);
        }
    }


    public void ClickMinus(ResourceType resourceType)
    {   
        DecrementAssignedWorkers(assignedTextMap[resourceType], resourceType);
    }


    private void IncrementAssignedWorkers(TextMeshProUGUI assignedText, ResourceType resourceType)
    {
        if (buildingSet.Contains(resourceType))
        {
            int assignedValue;
            // Check if the text can be parsed into an integer
            if (int.TryParse(assignedText.text, out assignedValue))
            {
                assignedText.text = (assignedValue + 1).ToString();
                availableWorkers--;
                availableWorkersText.text = availableWorkers.ToString();
                //int numOfWorkersAssigned = hiveScriptableObject.GetAssignedWorkers[resourceType] + 1;
                //workersAssigned[resourceType] = numOfWorkersAssigned;
  
            }
        }
        else
        {
            Debug.Log(string.Format("No {0} building exist, please build it first", resourceType));
        }

        
    }


    private void DecrementAssignedWorkers(TextMeshProUGUI assignedText, ResourceType resourceType)
    {
        if (buildingSet.Contains(resourceType))
        {
            int assignedValue;
            // Check if the text can be parsed into an integer
            if (int.TryParse(assignedText.text, out assignedValue) && assignedValue > 0)
            {
                assignedText.text = (assignedValue - 1).ToString();
                availableWorkers++;
                availableWorkersText.text = availableWorkers.ToString();
                //int numOfWorkersAssigned = hiveScriptableObject.GetAssignedWorkers[resourceType] - 1;
                //workersAssigned[resourceType] = numOfWorkersAssigned;
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
        gameObject.transform.parent.gameObject.SetActive(true);
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
            //hiveScriptableObject.AssignWorkers(pair.Key, pair.Value);
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


