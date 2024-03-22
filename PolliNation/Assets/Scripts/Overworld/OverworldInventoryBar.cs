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
    {
         if (UserInventory != null) {
            PollenAmountText.text = UserInventory.GetPollenCount.ToString();
            NectarAmountText.text = UserInventory.GetNectarCount.ToString();
            WaterAmountText.text = UserInventory.GetWaterCount.ToString();
            BudsAmountText.text = UserInventory.GetBudsCount.ToString();
        }
        //add listener 
        UserInventory.OnInventoryChanged += InventoryUpdated;
    }

    private void InventoryUpdated(object sender, System.EventArgs e) {
         if (UserInventory != null) {
            PollenAmountText.text = UserInventory.GetPollenCount.ToString();
            NectarAmountText.text = UserInventory.GetNectarCount.ToString();
            WaterAmountText.text = UserInventory.GetWaterCount.ToString();
            BudsAmountText.text = UserInventory.GetBudsCount.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
