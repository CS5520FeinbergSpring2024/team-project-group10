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
    public TextMeshProUGUI HealthAmountText;
    public InventoryScriptableObject UserInventory;
    private GameObject bee;

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
         }

         bee = GameObject.FindWithTag("Player");
         if (bee.GetComponent<BeeHealth>() != null)
         {
            bee.GetComponent<BeeHealth>().OnHealthChanged += HealthUpdated;
            HealthAmountText.text = bee.GetComponent<BeeHealth>().Health.ToString();
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

        // called on inventory count update
    private void HealthUpdated(object sender, System.EventArgs e) {
         if (bee.GetComponent<BeeHealth>() != null) {
            HealthAmountText.text = bee.GetComponent<BeeHealth>().Health.ToString();
        }
    }

}
