using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InventoryMenuScript : MonoBehaviour
{   
    public InventoryDataSingleton UserInventory;
    public HiveDataSingleton hive;
    public TextMeshProUGUI HoneyAmountText;
    public TextMeshProUGUI PropolisAmountText;
    public TextMeshProUGUI RoyalJellyAmountText;
    public TextMeshProUGUI PollenAmountText;
    public TextMeshProUGUI NectarAmountText;
    public TextMeshProUGUI WaterAmountText;
    public TextMeshProUGUI BudsAmountText;
    public TextMeshProUGUI HoneyLimitText;
    public TextMeshProUGUI PropolisLimitText;
    public TextMeshProUGUI RoyalJellyLimitText;
    public TextMeshProUGUI PollenLimitText;
    public TextMeshProUGUI NectarLimitText;
    public TextMeshProUGUI WaterLimitText;
    public TextMeshProUGUI BudsLimitText;
    public TextMeshProUGUI HoneyProductionRateText;
    public TextMeshProUGUI PropolisProductionRateText;
    public TextMeshProUGUI RoyalJellyProductionRateText;
    public TextMeshProUGUI PollenProductionRateText;
    public TextMeshProUGUI NectarProductionRateText;
    public TextMeshProUGUI WaterProductionRateText;
    public TextMeshProUGUI BudsProductionRateText;

    private Dictionary<ResourceType, int> rates = new();

    void Awake()
    {
        UserInventory = new InventoryDataSingleton();
        // if rate dictionary isn't already set up then set it up
        if (rates.Count == 0)
        {
            foreach(ResourceType resource in Enum.GetValues(typeof(ResourceType)))
            {
                rates.Add(resource, 0);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {   
        // make the menu not visible upon scene start
        gameObject.SetActive(false);

        hive = new();
        
        // on start do intial load of data and add event subscribers
        if (UserInventory != null) {
            LoadData();
            UserInventory.OnInventoryChanged += InventoryUpdated;
            LoadStorageLimits();
            LoadProductionRates();
            hive.OnStationLevelChanged += InventoryStorageAndProductionUpdated;
        }
    }

    // called on inventory count update
    private void InventoryUpdated(object sender, System.EventArgs e) {
        LoadData();
        // since rates sometimes depend on if inventory is full or empty also update with inventory count changes
        LoadProductionRates();
    }

    // called on inventory storage or production rate update
    private void InventoryStorageAndProductionUpdated(object sender, System.EventArgs e) {
        LoadStorageLimits();
        LoadProductionRates();
    }

    public void LoadData()
    {
        if (UserInventory != null)
        {
            // Load in inventory counts and limits data
            PollenAmountText.text = UserInventory.GetResourceCount(ResourceType.Pollen).ToString();
            NectarAmountText.text = UserInventory.GetResourceCount(ResourceType.Nectar).ToString();
            WaterAmountText.text = UserInventory.GetResourceCount(ResourceType.Water).ToString();
            BudsAmountText.text = UserInventory.GetResourceCount(ResourceType.Buds).ToString();
            HoneyAmountText.text = UserInventory.GetResourceCount(ResourceType.Honey).ToString();
            PropolisAmountText.text = UserInventory.GetResourceCount(ResourceType.Propolis).ToString();
            RoyalJellyAmountText.text = UserInventory.GetResourceCount(ResourceType.RoyalJelly).ToString();
        }
        else
        {
            Debug.LogWarning("Inventory Scriptable Object asset is null");
        }
    }

    public void LoadStorageLimits()
    {
        if (UserInventory != null)
        {
            PollenLimitText.text = UserInventory.GetStorageLimit(ResourceType.Pollen).ToString();
            NectarLimitText.text = UserInventory.GetStorageLimit(ResourceType.Nectar).ToString();
            WaterLimitText.text = UserInventory.GetStorageLimit(ResourceType.Water).ToString();
            BudsLimitText.text = UserInventory.GetStorageLimit(ResourceType.Buds).ToString();
            HoneyLimitText.text = UserInventory.GetStorageLimit(ResourceType.Honey).ToString();
            PropolisLimitText.text = UserInventory.GetStorageLimit(ResourceType.Propolis).ToString();
            RoyalJellyLimitText.text = UserInventory.GetStorageLimit(ResourceType.RoyalJelly).ToString();
        }
        else
        {
            Debug.LogWarning("Inventory Scriptable Object asset is null");
        }
    }

    public void LoadProductionRates()
    {
        // update calculated production rates stored in dictionary
        CalculateProductionRates();
        PollenProductionRateText.text = rates[ResourceType.Pollen].ToString();
        NectarProductionRateText.text = rates[ResourceType.Nectar].ToString();
        WaterProductionRateText.text = rates[ResourceType.Water].ToString();
        BudsProductionRateText.text = rates[ResourceType.Buds].ToString();
        HoneyProductionRateText.text = rates[ResourceType.Honey].ToString();
        PropolisProductionRateText.text = rates[ResourceType.Propolis].ToString();
        RoyalJellyProductionRateText.text = rates[ResourceType.RoyalJelly].ToString();
    
    }

    /// <summary>
    /// Calculates the net production rate of a resource based on hive station levels 
    /// and if resource is being consumed to produce other finished good resources
    /// stores results in rates dictionary.
    /// </summary>
    private void CalculateProductionRates()
    {
        // get conversion formulas from Formula class
        Dictionary<ResourceType, Dictionary<ResourceType, int>> formulas = Formula.conversionFormulas;
         List<ResourceType> notBeingProduced = new();

        // iterate over all resource types
        foreach(ResourceType resource in Enum.GetValues(typeof(ResourceType)))
        {
            // reset values
            rates[resource] = 0;

            // add resources being gathered / produced
            // assuming rate is 1 per second per worker
            if (hive.GetStationLevels(resource).productionLevel >= 1)
            {
            rates[resource] += hive.GetAssignedWorkers(resource);
            }

            // check if resource is being consumed to produce other finished good resources
            foreach (KeyValuePair<ResourceType, Dictionary<ResourceType, int>> formula in formulas)
            {
                if (formula.Value.ContainsKey(resource) && hive.GetAssignedWorkers(formula.Key) >= 1)
                {
                    // if produced resource is not able to meet formula demands it won't be produced and the ingredients wont be consumed
                    bool canProduce = true;
                    foreach(KeyValuePair<ResourceType, int> formulaIngredient in formula.Value)
                    {
                        // if not enough of resource in inventory to use in recipe for finsihed good
                        if (formulaIngredient.Value * hive.GetAssignedWorkers(formula.Key) 
                        > UserInventory.GetResourceCount(formulaIngredient.Key))
                        {
                            Debug.Log("RSRSRS Cannot produce: " + formula.Key + " not enough " + formulaIngredient + " need " + formulaIngredient.Value);
                            canProduce = false;
                            // produced resource is not being produced bc lack of supplies
                            notBeingProduced.Add(formula.Key);
                        }
                    }
                    // if finished good is being produced then take away contribution from ingredient
                    if (canProduce && hive.GetStationLevels(formula.Key).productionLevel >= 1)
                    {
                        // take away amount used to make produced resource based on formula 
                        // and assigned workers for producing finished good that resource is used to make
                        rates[resource] -= formula.Value[resource] * hive.GetAssignedWorkers(formula.Key);
                        
                    }
                }
            }

        }

        // go back through and set all finished goods that aren't being produced to 0
        if (notBeingProduced.Count > 0)
        {
            foreach (ResourceType notProducingResource in notBeingProduced)
            {
                rates[notProducingResource] = 0;
            }
        }
    }
    
}