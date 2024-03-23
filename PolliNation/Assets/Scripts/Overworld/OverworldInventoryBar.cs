using UnityEngine;
using TMPro;

public class OverworldInventoryBar : MonoBehaviour
{
    public InventoryScriptableObject UserInventory;
    public TextMeshProUGUI PollenAmountText;
    public TextMeshProUGUI NectarAmountText;
    public TextMeshProUGUI WaterAmountText;
    public TextMeshProUGUI BudsAmountText;

    // Start is called before the first frame update
    void Start()
    {   // set initial values and add listener
         if (UserInventory != null) {
            UserInventory.OnInventoryChanged += InventoryUpdated;
            PollenAmountText.text = UserInventory.GetPollenCount.ToString();
            NectarAmountText.text = UserInventory.GetNectarCount.ToString();
            WaterAmountText.text = UserInventory.GetWaterCount.ToString();
            BudsAmountText.text = UserInventory.GetBudsCount.ToString();
        }
        
    }

    // called on inventory count update
    private void InventoryUpdated(object sender, System.EventArgs e) {
         if (UserInventory != null) {
            PollenAmountText.text = UserInventory.GetPollenCount.ToString();
            NectarAmountText.text = UserInventory.GetNectarCount.ToString();
            WaterAmountText.text = UserInventory.GetWaterCount.ToString();
            BudsAmountText.text = UserInventory.GetBudsCount.ToString();
        }
    }

}
