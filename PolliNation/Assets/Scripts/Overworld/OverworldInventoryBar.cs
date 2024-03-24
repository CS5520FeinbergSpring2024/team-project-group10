using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OverworldInventoryBar : MonoBehaviour
{
    public TextMeshProUGUI PollenAmountText;
    public TextMeshProUGUI NectarAmountText;
    public TextMeshProUGUI WaterAmountText;
    public TextMeshProUGUI BudsAmountText;
    public InventoryScriptableObject UserInventory;

    // Start is called before the first frame update
    void Start()
    {
      // set initial values and add listener
         if (UserInventory != null) {
            UserInventory.OnInventoryChanged += InventoryUpdated;
            PollenAmountText.text = UserInventory.GetResourceCount(ResourceType.Pollen).ToString();
            NectarAmountText.text = UserInventory.GetResourceCount(ResourceType.Nectar).ToString();
            WaterAmountText.text = UserInventory.GetResourceCount(ResourceType.Water).ToString();
            BudsAmountText.text = UserInventory.GetResourceCount(ResourceType.Buds).ToString();
            UserInventory.UpdateInventory(ResourceType.Water,5555);
         }
    }

    // called on inventory count update
    private void InventoryUpdated(object sender, System.EventArgs e) {
         if (UserInventory != null) {
            PollenAmountText.text = UserInventory.GetResourceCount(ResourceType.Pollen).ToString();
            NectarAmountText.text = UserInventory.GetResourceCount(ResourceType.Nectar).ToString();
            WaterAmountText.text = UserInventory.GetResourceCount(ResourceType.Water).ToString();
            BudsAmountText.text = UserInventory.GetResourceCount(ResourceType.Buds).ToString();
        }
    }

}
