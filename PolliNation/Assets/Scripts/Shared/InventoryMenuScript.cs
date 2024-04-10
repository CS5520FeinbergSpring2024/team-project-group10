using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryMenuScript : MonoBehaviour
{   
    public InventoryScriptableObject UserInventory;
    public HiveScriptable hive;
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

    // Start is called before the first frame update
    void Start()
    {   
        // make the menu not visible upon scene start
        gameObject.SetActive(false);
        
        // on start do intial load of data and add listener
        if (UserInventory != null) {
            LoadData();
            LoadStorageLimits();
            LoadProductionRates();
            UserInventory.OnInventoryChanged += InventoryUpdated;
            UserInventory.OnInventoryStorageLimitsChanged += InventoryStorageUpdated;
        }
    }

    // called on inventory count update
    private void InventoryUpdated(object sender, System.EventArgs e) {
        LoadStorageLimits();
    }

    // called on inventory storage limit update
    private void InventoryStorageUpdated(object sender, System.EventArgs e) {
        LoadData();
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
        if (UserInventory != null)
        {
            PollenProductionRateText.text = hive.GetStationLevels(ResourceType.Pollen).productionLevel.ToString();
            NectarProductionRateText.text = hive.GetStationLevels(ResourceType.Nectar).productionLevel.ToString();
            WaterProductionRateText.text = hive.GetStationLevels(ResourceType.Water).productionLevel.ToString();
            BudsProductionRateText.text = hive.GetStationLevels(ResourceType.Buds).productionLevel.ToString();
            HoneyProductionRateText.text = hive.GetStationLevels(ResourceType.Honey).productionLevel.ToString();
            PropolisProductionRateText.text = hive.GetStationLevels(ResourceType.Propolis).productionLevel.ToString();
            RoyalJellyProductionRateText.text = hive.GetStationLevels(ResourceType.RoyalJelly).productionLevel.ToString();
        }
        else
        {
            Debug.LogWarning("Inventory Scriptable Object asset is null");
        }
    }
    
}