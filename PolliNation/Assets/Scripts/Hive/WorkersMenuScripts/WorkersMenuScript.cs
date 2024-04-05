using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System;
using TMPro;
using UnityEngine;
using System.Reflection;

public class WorkersMenuScript : MonoBehaviour
{
    public InventoryScriptableObject userInventory;
    //public HiveScriptableObject hiveScriptableObject;
    //public WokerScriptableObject wokerScriptableObject;
    public TextMeshProUGUI NectarAssignedText;
    public TextMeshProUGUI PollenAssignedText;
    public TextMeshProUGUI BudsAssignedText;
    public TextMeshProUGUI WaterAssignedText;
    public TextMeshProUGUI HoneyAssignedText;
    public TextMeshProUGUI PropolisAssignedText;
    public TextMeshProUGUI RoyalJellyAssignedText;
    public TextMeshProUGUI availableWorkersText;
    public TextMeshProUGUI NectarProducedText;
    public TextMeshProUGUI PollenProducedText;
    public TextMeshProUGUI WaterProducedText;
    public TextMeshProUGUI BudsProducedText;
    public TextMeshProUGUI HoneyCountDownText;
    public TextMeshProUGUI PropolisCountDownText;
    public TextMeshProUGUI RoyalJellyCountDownText;
    private Resource resource;
    private GameObject menuButtonObject;
    private ILaunchMenuButton launchMenuButton;
    //private static int availableWorkers; //make it static prevent to be destoryed when GameObject deactived.
    //private int totalworkerCount;
    //private DataClass dataClass;
    private Formula formula;
    private Dictionary<ResourceType, TextMeshProUGUI> assignedTextMap;
    private Dictionary<ResourceType, TextMeshProUGUI> producedTextMap;
    private Dictionary<ResourceType, TextMeshProUGUI> countDownTextMap;
    private Dictionary<ResourceType, int> resourceQuantities;
    private Dictionary<ResourceType, int> finishGoodQuantities;
    private Dictionary<ResourceType, int> resourceLimitMap;
    private Dictionary<ResourceType, int> resourceCollectingRateMap;
    private Dictionary<ResourceType, Coroutine> resourceCoroutines;
    // use for situation that player click plus or minus button but didn't assign workers
    private Dictionary<ResourceType, int> finishGoodQuantitiesTemp;
    private Dictionary<ResourceType, int> assignedWorkers;
    private List<ResourceType> haventBuiltList;
    private List<Building> buildings;
    //private float timeConvertHoney;
    //private float timeConvertPropolis;
    //private float timeConvertRoyalJelly;

    // use for test only
    private float timeConvertHoney = 10;
    private float timeConvertPropolis = 10;
    private float timeConvertRoyalJelly = 10;
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
            
            assignedTextMap = new Dictionary<ResourceType, TextMeshProUGUI>();
            producedTextMap = new Dictionary<ResourceType, TextMeshProUGUI>();
            resourceQuantities = new Dictionary<ResourceType, int>();
            finishGoodQuantities = new Dictionary<ResourceType, int>();
            finishGoodQuantitiesTemp = new Dictionary<ResourceType, int>();
            resourceLimitMap = new Dictionary<ResourceType, int>();
            resourceCollectingRateMap = new Dictionary<ResourceType, int>();
            resourceCoroutines = new Dictionary<ResourceType, Coroutine>();

            assignedTextMap.Add(ResourceType.Nectar, NectarAssignedText);
            assignedTextMap.Add(ResourceType.Pollen, PollenAssignedText);
            assignedTextMap.Add(ResourceType.Buds, BudsAssignedText);
            assignedTextMap.Add(ResourceType.Water, WaterAssignedText);
            assignedTextMap.Add(ResourceType.Honey, HoneyAssignedText);
            assignedTextMap.Add(ResourceType.Propolis, PropolisAssignedText);
            assignedTextMap.Add(ResourceType.RoyalJelly, RoyalJellyAssignedText);

            producedTextMap.Add(ResourceType.Nectar, NectarProduced);
            producedTextMap.Add(ResourceType.Pollen, PollenProduced);
            producedTextMap.Add(ResourceType.Water, WaterProduced);
            producedTextMap.Add(ResourceType.Buds, BudsProduced);

            countDownTextMap.Add(ResourceType.Honey, HoneyCountDownText);
            countDownTextMap.Add(ResourceType.Propolis, PropolisCountDownText);
            countDownTextMap.Add(ResourceType.RoyalJelly, RoyalJellyCountDownText);
            

            resourceQuantities.Add(ResourceType.Nectar, userInventory.GetResourceCount(ResourceType.Nectar));
            resourceQuantities.Add(ResourceType.Pollen, userInventory.GetResourceCount(ResourceType.Pollen));
            resourceQuantities.Add(ResourceType.Water, userInventory.GetResourceCount(ResourceType.Water));
            resourceQuantities.Add(ResourceType.Buds, userInventory.GetResourceCount(ResourceType.Buds));

            finishGoodQuantities.Add(ResourceType.Honey, userInventory.GetResourceCount(ResourceType.Honey));
            finishGoodQuantities.Add(ResourceType.Propolis, userInventory.GetResourceCount(ResourceType.Propolis));
            finishGoodQuantities.Add(ResourceType.RoyalJelly, userInventory.GetResourceCount(ResourceType.RoyalJelly));

            resourceLimitMap.Add(ResourceType.Nectar, userInventory.GetResourceLimit(ResourceType.Nectar));
            resourceLimitMap.Add(ResourceType.Pollen, userInventory.GetResourceLimit(ResourceType.Pollen));
            resourceLimitMap.Add(ResourceType.Water, userInventory.GetResourceLimit(ResourceType.Water));
            resourceLimitMap.Add(ResourceType.Buds, userInventory.GetResourceLimit(ResourceType.Buds));
            resourceLimitMap.Add(ResourceType.Honey, userInventory.GetResourceLimit(ResourceType.Honey));
            resourceLimitMap.Add(ResourceType.Propolis, userInventory.GetResourceLimit(ResourceType.Propolis));
            resourceLimitMap.Add(ResourceType.RoyalJelly, userInventory.GetResourceLimit(ResourceType.RoyalJelly));
  
            resourceCollectingRateMap.Add(ResourceType.Nectar, userInventory.GetResourceCollectingRate(ResourceType.Nectar));
            resourceCollectingRateMap.Add(ResourceType.Nectar, userInventory.GetResourceCollectingRate(ResourceType.Pollen));
            resourceCollectingRateMap.Add(ResourceType.Nectar, userInventory.GetResourceCollectingRate(ResourceType.Water));
            resourceCollectingRateMap.Add(ResourceType.Nectar, userInventory.GetResourceCollectingRate(ResourceType.Buds));

            
            
        }*/



        //used for test only
        assignedTextMap = new Dictionary<ResourceType, TextMeshProUGUI>();
        producedTextMap = new Dictionary<ResourceType, TextMeshProUGUI>();
        countDownTextMap = new Dictionary<ResourceType, TextMeshProUGUI>();
        resourceQuantities = new Dictionary<ResourceType, int>();
        finishGoodQuantities = new Dictionary<ResourceType, int>();
        finishGoodQuantitiesTemp = new Dictionary<ResourceType, int>();
        resourceLimitMap = new Dictionary<ResourceType, int>();
        resourceCollectingRateMap = new Dictionary<ResourceType, int>();
        resourceCoroutines = new Dictionary<ResourceType, Coroutine>();
        haventBuiltList = new List<ResourceType>();

        resourceQuantities.Add(ResourceType.Nectar, 5);
        resourceQuantities.Add(ResourceType.Pollen, 5);
        resourceQuantities.Add(ResourceType.Water, 5);
        resourceQuantities.Add(ResourceType.Buds, 5);
        finishGoodQuantities.Add(ResourceType.Honey, 0);
        finishGoodQuantities.Add(ResourceType.Propolis, 0);
        finishGoodQuantities.Add(ResourceType.RoyalJelly, 0);

        assignedTextMap.Add(ResourceType.Nectar, NectarAssignedText);
        assignedTextMap.Add(ResourceType.Pollen, PollenAssignedText);
        assignedTextMap.Add(ResourceType.Buds, BudsAssignedText);
        assignedTextMap.Add(ResourceType.Water, WaterAssignedText);
        assignedTextMap.Add(ResourceType.Honey, HoneyAssignedText);
        assignedTextMap.Add(ResourceType.Propolis, PropolisAssignedText);
        assignedTextMap.Add(ResourceType.RoyalJelly, RoyalJellyAssignedText);

        producedTextMap.Add(ResourceType.Nectar, NectarProducedText);
        producedTextMap.Add(ResourceType.Pollen, PollenProducedText);
        producedTextMap.Add(ResourceType.Water, WaterProducedText);
        producedTextMap.Add(ResourceType.Buds, BudsProducedText);

        countDownTextMap.Add(ResourceType.Honey, HoneyCountDownText);
        countDownTextMap.Add(ResourceType.Propolis, PropolisCountDownText);
        countDownTextMap.Add(ResourceType.RoyalJelly, RoyalJellyCountDownText);

        resourceLimitMap.Add(ResourceType.Nectar, 100);
        resourceLimitMap.Add(ResourceType.Pollen, 100);
        resourceLimitMap.Add(ResourceType.Water, 100);
        resourceLimitMap.Add(ResourceType.Buds, 100);

        resourceCollectingRateMap.Add(ResourceType.Nectar, 1);
        resourceCollectingRateMap.Add(ResourceType.Pollen, 1);
        resourceCollectingRateMap.Add(ResourceType.Water, 1);
        resourceCollectingRateMap.Add(ResourceType.Buds, 1);


        formula = new Formula(resourceQuantities);
        availableWorkersText.text = availableWorkers.ToString();
        totalworkerCount = availableWorkers;
        //////////////////////////////////
        ///
        


        //1. check what resource building is available
        LoadBuildingResource();
        
        
        //2.
    }

    private void LoadBuildingResource()
    {
        /*buildings = hiveScriptableObject.GetBuildings();
        List<ResourceType> builtList = new List<ResourceType>();
        foreach (Building building in buildings)
        {
            builtList.Add(building.GetResourceType());
        }

        foreach (ResourceType rt in Enum.GetValues(typeof(ResourceType)))
        {
            if (!builtList.Contains(rt))
            {
                haventBuiltList.Add(rt);
            }
        }

        UpdateUI();*/

    }

    private void UpdateUI()
    {
        GreyOut(haventBuiltList);
    }

    private void GreyOut(List<ResourceType> haventBuiltList)
    {
        
    }

   /* public void ClickPlus(ResourceType resourceType)
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
                        Assign();
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
*/
    /*public void ClickMinus(ResourceType resourceType)
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
                        availableWorkers++;
                    }
                }
                break;
        }
        availableWorkersText.text = availableWorkers.ToString();
        
    }*/


    public void ClickPlus(ResourceType resourceType)
    {
        if (availableWorkers > 0 && availableWorkers <= totalworkerCount)
        {
            switch (resourceType)
            {
                case ResourceType.Nectar:
                    IncrementAssignedGatheringWorkers(NectarAssignedText, ResourceType.Nectar);
                    break;
                case ResourceType.Pollen:
                    IncrementAssignedGatheringWorkers(PollenAssignedText, ResourceType.Pollen);
                    break;
                case ResourceType.Buds:
                    IncrementAssignedGatheringWorkers(BudsAssignedText, ResourceType.Buds);
                    break;
                case ResourceType.Water:
                    IncrementAssignedGatheringWorkers(WaterAssignedText, ResourceType.Water);
                    break;
                case ResourceType.Honey:
                    IncrementAssignedConvertingWorkers(HoneyAssignedText, ResourceType.Honey);
                    break;
                case ResourceType.Propolis:
                    IncrementAssignedConvertingWorkers(PropolisAssignedText, ResourceType.Propolis);
                    break;
                case ResourceType.RoyalJelly:
                    IncrementAssignedConvertingWorkers(RoyalJellyAssignedText, ResourceType.RoyalJelly);
                    break;
            }
        }
    }

    public void ClickMinus(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Nectar:
                DecrementAssignedGatheringWorkers(NectarAssignedText, ResourceType.Nectar);
                break;
            case ResourceType.Pollen:
                DecrementAssignedGatheringWorkers(PollenAssignedText, ResourceType.Pollen);
                break;
            case ResourceType.Buds:
                DecrementAssignedGatheringWorkers(BudsAssignedText, ResourceType.Buds);
                break;
            case ResourceType.Water:
                DecrementAssignedGatheringWorkers(WaterAssignedText, ResourceType.Water);
                break;
            case ResourceType.Honey:
                DecrementAssignedConvertingWorkers(HoneyAssignedText, ResourceType.Honey);
                break;
            case ResourceType.Propolis:
                DecrementAssignedConvertingWorkers(PropolisAssignedText, ResourceType.Propolis);
                break;
            case ResourceType.RoyalJelly:
                DecrementAssignedConvertingWorkers(RoyalJellyAssignedText, ResourceType.RoyalJelly);
                break;
        }
    }

    private void IncrementAssignedGatheringWorkers(TextMeshProUGUI assignedText, ResourceType resourceType)
    {
        int assignedValue;
        // Check if the text can be parsed into an integer
        if (int.TryParse(assignedText.text, out assignedValue))
        {
            assignedText.text = (assignedValue + 1).ToString();
            availableWorkers--;
            availableWorkersText.text = availableWorkers.ToString();
            // Assign a worker to collect the resource
            AssignGatheringWorkers(resourceType); 
        }
    }

    private void IncrementAssignedConvertingWorkers(TextMeshProUGUI assignedText, ResourceType resourceType)
    {
        int assignedValue;
        // Check if the text can be parsed into an integer
        if (int.TryParse(assignedText.text, out assignedValue))
        {
            if (formula.resourceEnough(resourceType, assignedValue + 1))
            {
                assignedText.text = (assignedValue + 1).ToString();
                availableWorkers--;
                availableWorkersText.text = availableWorkers.ToString();
                // Assign a worker to collect the resource
                AssignConvertingWorkers(resourceType);
            }
            else 
            {
                Debug.Log("No enough resource");
            }
        }
    }

    private void DecrementAssignedGatheringWorkers(TextMeshProUGUI assignedText, ResourceType resourceType)
    {
        int assignedValue;
        // Check if the text can be parsed into an integer
        if (int.TryParse(assignedText.text, out assignedValue))
        {
            if (assignedValue > 0) //only do logic after worker assigned
            {
                assignedText.text = (assignedValue - 1).ToString();
                availableWorkers++;
                availableWorkersText.text = availableWorkers.ToString();
                // Assign a worker to collect the resource
                AssignGatheringWorkers(resourceType);
            }
        }
    }

    private void DecrementAssignedConvertingWorkers(TextMeshProUGUI assignedText, ResourceType resourceType)
    {
        int assignedValue;
        // Check if the text can be parsed into an integer
        if (int.TryParse(assignedText.text, out assignedValue))
        {
            if (assignedValue > 0)
            {
                formula.Cancel(resourceType);
                assignedText.text = (assignedValue - 1).ToString();
                availableWorkers++;
                availableWorkersText.text = availableWorkers.ToString();
                // Assign a worker to collect the resource
                AssignConvertingWorkers(resourceType);
            }
        }
    }



    private void AssignGatheringWorkers(ResourceType resourceType)
    {
        string zero = "0";
        int assignedValue;
        if (int.TryParse(assignedTextMap[resourceType].text, out assignedValue))
        {
            int workersAssigned = assignedValue;

            if (resourceCoroutines.TryGetValue(resourceType, out Coroutine existingCoroutine))
            {
                if (existingCoroutine != null)
                {
                    CoroutineManager.Instance.StopCoroutine(existingCoroutine);
                    resourceCoroutines.Remove(resourceType);
                }
            }

            //when assignedValue == 0 means no worker, so no need do below logic
            if (assignedValue == 0)
            {
                return;
            }

            Coroutine newCoroutine = CoroutineManager.Instance.StartCoroutine(StartCollectingResource(workersAssigned, resourceType,  () => 
            {
                availableWorkers += workersAssigned;
                availableWorkersText.text = availableWorkers.ToString();
                assignedTextMap[resourceType].text = zero;
                saveChanges();
            }));
            resourceCoroutines[resourceType] = newCoroutine;
        }
    }

    private void AssignConvertingWorkers(ResourceType resourceType) 
    {
        string zero = "0";
        int assignedValue;
        if (int.TryParse(assignedTextMap[resourceType].text, out assignedValue))
        {
            int workersAssigned = assignedValue;

            if (resourceCoroutines.TryGetValue(resourceType, out Coroutine existingCoroutine))
            {
                if (existingCoroutine != null)
                {
                    CoroutineManager.Instance.StopCoroutine(existingCoroutine);
                    resourceCoroutines.Remove(resourceType);
                }
            }

            //when assignedValue == 0 means no worker, so no need do below logic
            if (assignedValue == 0)
            {
                return;
            }

            Coroutine newCoroutine = CoroutineManager.Instance.StartCoroutine(StartConvertingResource(timeConvertHoney * workersAssigned, remainingTime =>
            {
                //update time, call when update
                countDownTextMap[resourceType].text = FormatTime(remainingTime);
            }, () =>
            {
                finishGoodQuantities[resourceType] += assignedValue;
                availableWorkers += workersAssigned;
                availableWorkersText.text = availableWorkers.ToString();
                assignedTextMap[resourceType].text = zero;
                Debug.Log(123132);
                saveChanges();
            }));
            resourceCoroutines[resourceType] = newCoroutine;
        }
    }


    private IEnumerator StartConvertingResource(float duration, System.Action<float> onUpdate, System.Action onFinished)
    {
        float startTime = Time.realtimeSinceStartup;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            onUpdate?.Invoke(duration - elapsedTime);
            elapsedTime = Time.realtimeSinceStartup - startTime;
            yield return new WaitForSeconds(1f);
        }

        onFinished?.Invoke();
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private IEnumerator StartCollectingResource(int workersAssigned, ResourceType resourceType, System.Action onFinished)
    {
        //int limit = resourceLimitMap[resourceType] * hiveScriptableObject[resourceType].storageLevel;
        
        //use for test only
        int limit = 100;
        int updateFrequency = 1;
        ////

        int resourceProduced = 0;
        while (resourceQuantities[resourceType] <= limit && workersAssigned > 0)
        {

            resourceProduced = resourceCollectingRateMap[resourceType] * workersAssigned;
            resourceQuantities[resourceType] += resourceProduced;
            resourceQuantities[resourceType] = Mathf.Min(resourceQuantities[resourceType], limit);
            
            string format = string.Format("{0} / {1}", resourceQuantities[resourceType], limit);
            producedTextMap[resourceType].text = $"{resourceQuantities[resourceType]} / {limit}";
            saveChanges();
            yield return new WaitForSeconds(updateFrequency);
        }

        onFinished?.Invoke();
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


    


    /*public void Assign()
    {   
        string zero = "0";

        int nectarAssignedValue;
        if (int.TryParse(NectarAssignedText.text, out nectarAssignedValue) && nectarAssignedValue != 0)
        {
            //record how many workers assigned which will be add back when assignment finished.
            int workersAssigned = nectarAssignedValue;
            CoroutineManager.Instance.StartCoroutine(TimerManager.UpdateInventoryEveryMinute(rateProduce, storageLimit, this, hiveScriptableObject, workersAssigned, percentageToCapacity =>
            {
                //update information, call when update
                NectarAssignedText.text = TimerManager.FormatGathering(workersAssigned, ResourceType.Nectar, percentageToCapacity);

            }, () => 
            {   
                //add resource to local storage when finish assignment. call when count down finished
                resourceQuantities[ResourceType.Nectar] = resourceQuantities[ResourceType.Nectar] + nectarAssignedValue;
                NectarAssignedText.text = zero;
                availableWorkers += workersAssigned;
                availableWorkersText.text = availableWorkers.ToString(); // worker add one when it finished last assignment
                saveChanges(); // save changes whenever it finished collecting or converting(no need to click claim).
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
                PollenAssignedText.text = zero;
                availableWorkers += workersAssigned;
                availableWorkersText.text = availableWorkers.ToString();
                saveChanges();
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
                WaterAssignedText.text = zero;
                availableWorkers += workersAssigned;
                availableWorkersText.text = availableWorkers.ToString();
                saveChanges();
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
                BudsAssignedText.text = zero;
                availableWorkers += workersAssigned;
                availableWorkersText.text = availableWorkers.ToString();
                saveChanges();
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
                HoneyAssignedText.text = zero;
                availableWorkers += workersAssigned;
                availableWorkersText.text = availableWorkers.ToString();
                saveChanges();
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
                PropolisAssignedText.text = zero;
                availableWorkers += workersAssigned;
                availableWorkersText.text = availableWorkers.ToString();
                saveChanges();
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
                RoyalJellyAssignedText.text = zero;
                availableWorkers += royalJellyAssignedValue;
                availableWorkersText.text = availableWorkers.ToString();
                saveChanges();
            }));
        }

    }
*/



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


