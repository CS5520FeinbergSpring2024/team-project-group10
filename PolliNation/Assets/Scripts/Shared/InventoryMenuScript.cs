using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryMenuScript : MonoBehaviour
{   
    public InventoryScriptableObject UserInventory;
    public TextMeshProUGUI HoneyAmountText;
    public TextMeshProUGUI PropolisAmountText;
    public TextMeshProUGUI RoyalJellyAmountText;
    public TextMeshProUGUI PollenAmountText;
    public TextMeshProUGUI NectarAmountText;
    public TextMeshProUGUI WaterAmountText;
    public TextMeshProUGUI BudsAmountText;

    /*
    PLACEHOLDER FOR FUTURE RESOURCE LIMITS
    public TextMeshProUGUI HoneyLimitText;
    public TextMeshProUGUI PropolisLimitText;
    public TextMeshProUGUI RoyalJellyLimitText;
    public TextMeshProUGUI PollenLimitText;
    public TextMeshProUGUI NectarLimitText;
    public TextMeshProUGUI WaterLimitText;
    public TextMeshProUGUI BudsLimitText;
    */

    // Start is called before the first frame update
    void Start()
    {   
        // make the menu not visible upon scene start
        gameObject.SetActive(false);

        // on start do intial load of data and add listener
        if (UserInventory != null) {
            LoadData();
            UserInventory.OnInventoryChanged += InventoryUpdated;
            //future listener for changes in inventory limits
            // UserInventory.OnInventoryLimitsChanged += InventoryLimitsUpdated;
        }
    }

    // method to open menu 
    public void SetOpen()
    {
        gameObject.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(true);
    }

    // called on inventory count update
    private void InventoryUpdated(object sender, System.EventArgs e) {
        LoadData();
    }

    public void LoadData()
    {
        if (UserInventory != null)
        {
            // Load in inventory data
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

    /*

    // called on inventory limit update (user obtains more storage etc etc)

    private void InventoryLimitsUpdated(object sender, System.EventArgs e) {
        if (UserInventory != null)
        {
            // Load in inventory data
            PollenLimitText.text = UserInventory.GetResourceLimit(ResourceType.Pollen).ToString();
            NectarLimitText.text = UserInventory.GetResourceLimit(ResourceType.Nectar).ToString();
            WaterLimitText.text = UserInventory.GetResourceLimit(ResourceType.Water).ToString();
            BudsLimitText.text = UserInventory.GetResourceLimit(ResourceType.Buds).ToString();
            HoneyLimitText.text = UserInventory.GetResourceLimit(ResourceType.Honey).ToString();
            PropolisLimitText.text = UserInventory.GetResourceLimit(ResourceType.Propolis).ToString();
            RoyalJellyLimitText.text = UserInventory.GetResourceLimit(ResourceType.RoyalJelly).ToString();

        }
        else
        {
            Debug.LogWarning("Inventory Scriptable Object asset is null");
        }
    */

    
    /*
    public void ExitMenu()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        canvas.gameObject.SetActive(false);
        Debug.Log("Inventory Menu Exit button was clicked");
    }
    */
}