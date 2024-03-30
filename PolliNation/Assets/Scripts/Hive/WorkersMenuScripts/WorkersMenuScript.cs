using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class WorkersMenuScript : MonoBehaviour
{
    public InventoryScriptableObject userInventory;
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
    //private int availableWorkers;
    //private int totalworkerCount;
    //private DataClass dataClass;
    private Formula formula;
    private Dictionary<ResourceType, int> resourceQuantities;

    //used for test only
    private int availableWorkers = 5;
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
            availableWorkers = resource.GetNumWorkers();
            availableWorkersText.text = availableWorkers.ToString();
            totalworkerCount = availableWorkers;

            resourceQuantities = new Dictionary<string, int>();
            resourceQuantities.Add("Nectar", userInventory.GetResourceCount(ResourceType.Nectar));
            resourceQuantities.Add("Pollen", userInventory.GetResourceCount(ResourceType.Pollen));
            resourceQuantities.Add("Water", userInventory.GetResourceCount(ResourceType.Water));
            resourceQuantities.Add("Buds", userInventory.GetResourceCount(ResourceType.Buds))
            formula = new Formula(resourceQuantities);
        }*/

        //used for test only
        resourceQuantities = new Dictionary<ResourceType, int>();
        formula = new Formula(resourceQuantities);
        availableWorkersText.text = availableWorkers.ToString();
        totalworkerCount = availableWorkers;
        //////////////////////////////////
    }

    public void ClickPlus(ResourceType resourceType)
    {
        if (availableWorkers > 0 && availableWorkers <= totalworkerCount)
        {
            switch (resourceType) 
            {

                case ResourceType.Nectar:
                    NectarAssignedText.text = (int.Parse(NectarAssignedText.text) + 1).ToString();
                    availableWorkers--;
                    break;
                case ResourceType.Pollen:
                    PollenAssignedText.text = (int.Parse(PollenAssignedText.text) + 1).ToString();
                    availableWorkers--;
                    break;
                case ResourceType.Buds:
                    BudsAssignedText.text = (int.Parse(BudsAssignedText.text) + 1).ToString();
                    availableWorkers--;
                    break;
                case ResourceType.Water:
                    WaterAssignedText.text = (int.Parse(WaterAssignedText.text) + 1).ToString();
                    availableWorkers--;
                    break;
                case ResourceType.Honey:
                    if (formula.Honey(int.Parse(HoneyAssignedText.text) + 1))
                    {
                        HoneyAssignedText.text = (int.Parse(HoneyAssignedText.text) + 1).ToString();
                        availableWorkers--;
                    }
                    else 
                    {
                        Debug.Log("You don't have enough resource to convert");
                    }
                    break;
                case ResourceType.Propolis:
                    if (formula.Propolis(int.Parse(PropolisAssignedText.text) + 1)) 
                    {
                        PropolisAssignedText.text = (int.Parse(PropolisAssignedText.text) + 1).ToString();
                        availableWorkers--;
                    }
                    else
                    {
                        Debug.Log("You don't have enough resource to convert");
                    }
                    break;
                case ResourceType.RoyalJelly:
                    if (formula.RoyalJelly(int.Parse(RoyalJellyAssignedText.text) + 1)) 
                    {
                        RoyalJellyAssignedText.text = (int.Parse(RoyalJellyAssignedText.text) + 1).ToString();
                        availableWorkers--;
                    }
                    else
                    {
                        Debug.Log("You don't have enough resource to convert");
                    }
                    break;
            }
            availableWorkersText.text = availableWorkers.ToString();
        }
    }

    public void ClickMinus(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Nectar:
                if (int.Parse(NectarAssignedText.text) > 0)
                {
                    NectarAssignedText.text = (int.Parse(NectarAssignedText.text) - 1).ToString();
                    availableWorkers++;
                }
                break;
            case ResourceType.Pollen:
                if (int.Parse(PollenAssignedText.text) > 0)
                {
                    PollenAssignedText.text = (int.Parse(PollenAssignedText.text) - 1).ToString();
                    availableWorkers++;
                }
                break;
            case ResourceType.Buds:
                if (int.Parse(BudsAssignedText.text) > 0)
                {
                    BudsAssignedText.text = (int.Parse(BudsAssignedText.text) - 1).ToString();
                    availableWorkers++;
                }
                break;
            case ResourceType.Water:
                if (int.Parse(WaterAssignedText.text) > 0)
                {
                    WaterAssignedText.text = (int.Parse(WaterAssignedText.text) - 1).ToString();
                    availableWorkers++;
                }
                break;
            case ResourceType.Honey:
                if (int.Parse(HoneyAssignedText.text) > 0)
                {
                    HoneyAssignedText.text = (int.Parse(HoneyAssignedText.text) - 1).ToString();
                    availableWorkers++;
                }
                break;
            case ResourceType.Propolis:
                if (int.Parse(PropolisAssignedText.text) > 0)
                {
                    PropolisAssignedText.text = (int.Parse(PropolisAssignedText.text) - 1).ToString();
                    availableWorkers++;
                }
                break;
            case ResourceType.RoyalJelly:
                if (int.Parse(RoyalJellyAssignedText.text) > 0)
                {
                    RoyalJellyAssignedText.text = (int.Parse(RoyalJellyAssignedText.text) - 1).ToString();
                    availableWorkers++;
                }
                break;
        }
        availableWorkersText.text = availableWorkers.ToString();
        
    }

    public void SetOpen()
    {
        gameObject.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(true);
    }

    public void SetClose()
    {
        gameObject.SetActive(false);
    }

    public void Assign()
    {
        foreach (var kvp in resourceQuantities)
        {
            ResourceType key = kvp.Key;
            int value = kvp.Value;
            userInventory.UpdateInventory(key, value);
        }
    }

    public void ExitMenu()
    {
        Canvas canvas = GetComponentInChildren<Canvas>();
        canvas.gameObject.SetActive(false);

        if (launchMenuButton != null)
        {
            launchMenuButton.ReappearButton();
        }
    }

}


