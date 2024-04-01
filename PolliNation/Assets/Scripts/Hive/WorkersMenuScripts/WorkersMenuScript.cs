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
    //private static int availableWorkers; //make it static prevent to be destoryed when GameObject deactived.
    //private int totalworkerCount;
    //private DataClass dataClass;
    private Formula formula;
    private Dictionary<ResourceType, int> resourceQuantities;
    private Dictionary<ResourceType, int> finishGoodQuantities;
    private float timeProduceNectar = 10; //10 seconds
    private float timeProducePollen = 15;
    private float timeProduceWater = 20;
    private float timeProduceBuds = 25;
    private float timeConvertHoney = 60;
    private float timeConvertPropolis = 70;
    private float timeConvertRoyalJelly = 80;


    //used for test only
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
            availableWorkers = resource.GetNumWorkers();
            availableWorkersText.text = availableWorkers.ToString();
            totalworkerCount = availableWorkers;

            resourceQuantities = new Dictionary<ResourceType, int>();
            finishGoodQuantities = new Dictionary<ResourceType, int>();
            resourceQuantities.Add(ResourceType.Nectar, userInventory.GetResourceCount(ResourceType.Nectar));
            resourceQuantities.Add(ResourceType.Pollen, userInventory.GetResourceCount(ResourceType.Pollen));
            resourceQuantities.Add(ResourceType.Water, userInventory.GetResourceCount(ResourceType.Water));
            resourceQuantities.Add(ResourceType.Buds, userInventory.GetResourceCount(ResourceType.Buds));
            finishGoodQuantities.Add(ResourceType.Honey, 0);
            finishGoodQuantities.Add(ResourceType.Propolis, 0);
            finishGoodQuantities.Add(ResourceType.RoyalJelly, 0);
            formula = new Formula(resourceQuantities);
        }*/

        //used for test only
        resourceQuantities = new Dictionary<ResourceType, int>();
        finishGoodQuantities = new Dictionary<ResourceType, int>();
        resourceQuantities.Add(ResourceType.Nectar, 5);
        resourceQuantities.Add(ResourceType.Pollen, 5);
        resourceQuantities.Add(ResourceType.Water, 5);
        resourceQuantities.Add(ResourceType.Buds, 5);
        finishGoodQuantities.Add(ResourceType.Honey, 0);
        finishGoodQuantities.Add(ResourceType.Propolis, 0);
        finishGoodQuantities.Add(ResourceType.RoyalJelly, 0);
        formula = new Formula(resourceQuantities);
        availableWorkersText.text = availableWorkers.ToString();
        totalworkerCount = availableWorkers;
        //////////////////////////////////
    }

    public void ClickPlus(ResourceType resourceType)
    {
        if (availableWorkers > 0 && availableWorkers <= totalworkerCount)
        {
            int assignedValue;
            switch (resourceType) 
            {
                
                case ResourceType.Nectar:
                    //check if the input text is correct format, below are same
                    if (int.TryParse(NectarAssignedText.text, out assignedValue))
                    {
                        NectarAssignedText.text = (assignedValue + 1).ToString();
                        availableWorkers--;
                    }   
                    break;
                case ResourceType.Pollen:
                    if (int.TryParse(PollenAssignedText.text, out assignedValue))
                    {
                        PollenAssignedText.text = (assignedValue + 1).ToString();
                        availableWorkers--;
                    }
                    break;
                case ResourceType.Buds:
                    if (int.TryParse(BudsAssignedText.text, out assignedValue))
                    {
                        BudsAssignedText.text = (assignedValue + 1).ToString();
                        availableWorkers--;
                    }
                    break;
                case ResourceType.Water:
                    if (int.TryParse(WaterAssignedText.text, out assignedValue))
                    {
                        WaterAssignedText.text = (assignedValue + 1).ToString();
                        availableWorkers--;
                    }
                    break;
                case ResourceType.Honey:
                    if (int.TryParse(HoneyAssignedText.text, out assignedValue))
                    {
                        if (formula.Honey(assignedValue + 1))
                        {
                            HoneyAssignedText.text = (assignedValue+ 1).ToString();
                            finishGoodQuantities[ResourceType.Honey] = assignedValue + 1;
                            availableWorkers--;
                        }
                        else
                        {
                            Debug.Log("You don't have enough resource to convert");
                        }
                    }
                    break;
                case ResourceType.Propolis:
                    if (int.TryParse(PropolisAssignedText.text, out assignedValue))
                    {
                        if (formula.Propolis(assignedValue + 1))
                        {
                            PropolisAssignedText.text = (assignedValue + 1).ToString();
                            finishGoodQuantities[ResourceType.Propolis] = assignedValue + 1;
                            availableWorkers--;
                        }
                        else
                        {
                            Debug.Log("You don't have enough resource to convert");
                        }
                    }
                    break;
                case ResourceType.RoyalJelly:
                    if (int.TryParse(RoyalJellyAssignedText.text, out assignedValue))
                    {
                        if (formula.RoyalJelly(assignedValue + 1))
                        {
                            RoyalJellyAssignedText.text = (assignedValue + 1).ToString();
                            finishGoodQuantities[ResourceType.RoyalJelly] = assignedValue + 1;
                            availableWorkers--;
                        }
                        else
                        {
                            Debug.Log("You don't have enough resource to convert");
                        }
                    }
                    break;
            }
            availableWorkersText.text = availableWorkers.ToString();
        }
    }

    public void ClickMinus(ResourceType resourceType)
    {
        int assignedValue;
        switch (resourceType)
        {
            case ResourceType.Nectar:
                //check if the input text is correct format, below are same
                if (int.TryParse(NectarAssignedText.text, out assignedValue))
                {   
                    if (assignedValue > 0) //only do logic after worker assigned, below are same
                    {
                        NectarAssignedText.text = (assignedValue - 1).ToString();
                        availableWorkers++;
                    }
                }
                break;
            case ResourceType.Pollen:
                if (int.TryParse(PollenAssignedText.text, out assignedValue))
                {
                    if (assignedValue > 0)
                    {
                        PollenAssignedText.text = (assignedValue - 1).ToString();
                        availableWorkers++;
                    }
                }
                break;
            case ResourceType.Buds:
                if (int.TryParse(BudsAssignedText.text, out assignedValue))
                {
                    if (assignedValue > 0)
                    {
                        BudsAssignedText.text = (assignedValue - 1).ToString();
                        availableWorkers++;
                    }
                }
                break;
            case ResourceType.Water:
                if (int.TryParse(WaterAssignedText.text, out assignedValue))
                {
                    if (assignedValue > 0)
                    {
                        WaterAssignedText.text = (assignedValue - 1).ToString();
                        availableWorkers++;
                    }
                }
                break;
            case ResourceType.Honey:
                if (int.TryParse(HoneyAssignedText.text, out assignedValue))
                {
                    if (assignedValue > 0)
                    {
                        HoneyAssignedText.text = (assignedValue - 1).ToString();
                        finishGoodQuantities[ResourceType.Honey] = assignedValue - 1;
                        availableWorkers++;
                    }
                }
                break;
            case ResourceType.Propolis:
                if (int.TryParse(PropolisAssignedText.text, out assignedValue))
                {
                    if (assignedValue > 0)
                    {
                        PropolisAssignedText.text = (assignedValue - 1).ToString();
                        finishGoodQuantities[ResourceType.Propolis] = assignedValue - 1;
                        availableWorkers++;
                    }
                }
                break;
            case ResourceType.RoyalJelly:
                if (int.TryParse(RoyalJellyAssignedText.text, out assignedValue))
                {
                    if (assignedValue > 0)
                    {
                        RoyalJellyAssignedText.text = (assignedValue - 1).ToString();
                        finishGoodQuantities[ResourceType.RoyalJelly] = assignedValue - 1;
                        availableWorkers++;
                    }
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

    private void saveChanges()
    {
        //store finish goods changes to the database
        foreach (var good in finishGoodQuantities)
        {
            ResourceType key = good.Key;
            int value = good.Value;
            userInventory.UpdateInventory(key, value);

            //dataClass.SaveResource(key, userInventory.GetResourceCount(key) + value);
        }


        //store raw material changes to the database
        foreach (var res in resourceQuantities)
        {
            ResourceType key = res.Key;
            int value = res.Value;
            userInventory.UpdateInventory(key, value);

            //dataClass.SaveResource(key, value);
        }
    }


    public void Assign()
    {   
        int nectarAssignedValue;
        if (int.TryParse(NectarAssignedText.text, out nectarAssignedValue) && nectarAssignedValue != 0)
        {
            //record how many workers assigned which will be add back when assignment finished.
            int workersAssigned = nectarAssignedValue;
            CoroutineManager.Instance.StartCoroutine(TimerManager.StartCountdown(timeProduceNectar * nectarAssignedValue, remainingTime =>
            {
                //update time, call when update
                NectarAssignedText.text = TimerManager.FormatTime(remainingTime);

            }, () => 
            {   
                //add resource to local storage when finish assignment. call when count down finished
                resourceQuantities[ResourceType.Nectar] = resourceQuantities[ResourceType.Nectar] + nectarAssignedValue;
                NectarAssignedText.text = "Finished";
                availableWorkers += workersAssigned;
                availableWorkersText.text = availableWorkers.ToString(); // worker add one when it finished last assignment
            }));
        }

        int pollenAssignedValue;
        if (int.TryParse(PollenAssignedText.text, out pollenAssignedValue) && pollenAssignedValue != 0)
        {
            int workersAssigned = pollenAssignedValue;
            CoroutineManager.Instance.StartCoroutine(TimerManager.StartCountdown(timeProducePollen * pollenAssignedValue, remainingTime =>
            {
                PollenAssignedText.text = TimerManager.FormatTime(remainingTime);
            }, () =>
            {
                resourceQuantities[ResourceType.Pollen] = resourceQuantities[ResourceType.Pollen] + pollenAssignedValue;
                PollenAssignedText.text = "Finished";
                availableWorkers += workersAssigned;
                availableWorkersText.text = availableWorkers.ToString();
            }));
        }

        int waterAssignedValue;
        if (int.TryParse(WaterAssignedText.text, out waterAssignedValue) && waterAssignedValue != 0)
        {
            int workersAssigned = waterAssignedValue;
            CoroutineManager.Instance.StartCoroutine(TimerManager.StartCountdown(timeProduceWater * waterAssignedValue, remainingTime =>
            {
                WaterAssignedText.text = TimerManager.FormatTime(remainingTime);
            }, () =>
            {
                resourceQuantities[ResourceType.Water] = resourceQuantities[ResourceType.Water] + waterAssignedValue;
                WaterAssignedText.text = "Finished";
                availableWorkers += workersAssigned;
                availableWorkersText.text = availableWorkers.ToString();
            }));
        }

        int budsAssignedValue;
        if (int.TryParse(BudsAssignedText.text, out budsAssignedValue) && budsAssignedValue != 0)
        {
            int workersAssigned = budsAssignedValue;
            CoroutineManager.Instance.StartCoroutine(TimerManager.StartCountdown(timeProduceBuds * budsAssignedValue, remainingTime =>
            {
                BudsAssignedText.text = TimerManager.FormatTime(remainingTime);
            }, () =>
            {
                resourceQuantities[ResourceType.Buds] = resourceQuantities[ResourceType.Buds] + budsAssignedValue;
                BudsAssignedText.text = "Finished";
                availableWorkers += workersAssigned;
                availableWorkersText.text = availableWorkers.ToString();
            }));
        }

        int honeyAssignedValue;
        if (int.TryParse(HoneyAssignedText.text, out honeyAssignedValue) && honeyAssignedValue != 0)
        {   
            int workersAssigned = honeyAssignedValue;
            CoroutineManager.Instance.StartCoroutine(TimerManager.StartCountdown(timeConvertHoney * honeyAssignedValue, remainingTime =>
            {
                HoneyAssignedText.text = TimerManager.FormatTime(remainingTime);
            }, () =>
            {
                finishGoodQuantities[ResourceType.Honey] = finishGoodQuantities[ResourceType.Honey] + honeyAssignedValue;
                HoneyAssignedText.text = "Finished";
                availableWorkers += workersAssigned;
                availableWorkersText.text = availableWorkers.ToString();
            }));
        }

        int propolisAssignedValue;
        if (int.TryParse(PropolisAssignedText.text, out propolisAssignedValue) && propolisAssignedValue != 0)
        {
            int workersAssigned = propolisAssignedValue;
            CoroutineManager.Instance.StartCoroutine(TimerManager.StartCountdown(timeConvertPropolis * propolisAssignedValue, remainingTime =>
            {
                PropolisAssignedText.text = TimerManager.FormatTime(remainingTime);
            }, () =>
            {
                finishGoodQuantities[ResourceType.Propolis] = finishGoodQuantities[ResourceType.Propolis] + propolisAssignedValue;
                PropolisAssignedText.text = "Finished";
                availableWorkers += workersAssigned;
                availableWorkersText.text = availableWorkers.ToString();
            }));
        }

        int royalJellyAssignedValue;
        if (int.TryParse(RoyalJellyAssignedText.text, out royalJellyAssignedValue) && royalJellyAssignedValue != 0)
        {
            int workersAssigned = royalJellyAssignedValue;
            CoroutineManager.Instance.StartCoroutine(TimerManager.StartCountdown(timeConvertRoyalJelly * royalJellyAssignedValue, remainingTime =>
            {
                RoyalJellyAssignedText.text = TimerManager.FormatTime(remainingTime);
            }, () =>
            {
                finishGoodQuantities[ResourceType.RoyalJelly] = finishGoodQuantities[ResourceType.RoyalJelly] + royalJellyAssignedValue;
                RoyalJellyAssignedText.text = "Finished";
                availableWorkers += royalJellyAssignedValue;
                availableWorkersText.text = availableWorkers.ToString();
            }));
        }


    }

    public void ClaimResources()
    {
        string zero = "0";
        string finished = "Finished";
        //set timer back to show number of worker need to be assigned.
        if (NectarAssignedText.text.Equals(finished))
        {
            NectarAssignedText.text = zero;
            saveChanges();
        }
       
        if (PollenAssignedText.text.Equals(finished))
        {
            PollenAssignedText.text = zero;
            saveChanges();
        }
        
        if (WaterAssignedText.text.Equals(finished))
        {
            WaterAssignedText.text = zero;
            saveChanges();
        }
        
        if (BudsAssignedText.text.Equals(finished))
        {
            BudsAssignedText.text = zero;
            saveChanges();
        }
        
        if (HoneyAssignedText.text.Equals(finished))
        {
            HoneyAssignedText.text = zero;
            saveChanges();
        }

        if(PropolisAssignedText.text.Equals(finished))
        {
            PropolisAssignedText.text = zero;
            saveChanges();
        }

        if(RoyalJellyAssignedText.text.Equals(finished))
        {
            RoyalJellyAssignedText.text = zero;
            saveChanges();
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


