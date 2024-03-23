using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HiveInventoryBar : MonoBehaviour
{
    public TextMeshProUGUI HoneyAmountText;
    public TextMeshProUGUI PropolisAmountText;
    public TextMeshProUGUI RoyalJellyAmountText;

    public InventoryScriptableObject UserInventory;

    // Start is called before the first frame update
    void Start()
    {
        // set initial values and add listener
        if (UserInventory != null) {
            UserInventory.OnInventoryChanged += InventoryUpdated;
            HoneyAmountText.text = UserInventory.GetHoneyCount.ToString();
            PropolisAmountText.text = UserInventory.GetPropolisCount.ToString();
            RoyalJellyAmountText.text = UserInventory.GetRoyalJellyCount.ToString();
        }
    }

        // called on inventory count update
    private void InventoryUpdated(object sender, System.EventArgs e) {
        if (UserInventory != null) {
            HoneyAmountText.text = UserInventory.GetHoneyCount.ToString();
            PropolisAmountText.text = UserInventory.GetPropolisCount.ToString();
            RoyalJellyAmountText.text = UserInventory.GetRoyalJellyCount.ToString();
        }
    }

    
}
