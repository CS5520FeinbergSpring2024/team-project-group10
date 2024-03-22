using UnityEngine;
using TMPro;

public class HiveInventoryBar : MonoBehaviour
{
    public InventoryScriptableObject UserInventory;
    public TextMeshProUGUI HoneyAmountText;
    public TextMeshProUGUI PropolisAmountText;
    public TextMeshProUGUI RoyalJellyAmountText;
    // Start is called before the first frame update
    void Start()
    {
        if (UserInventory != null) {
            HoneyAmountText.text = UserInventory.GetHoneyCount.ToString();
            PropolisAmountText.text = UserInventory.GetPropolisCount.ToString();
            RoyalJellyAmountText.text = UserInventory.GetRoyalJellyCount.ToString();
        }
        // add listener 
        UserInventory.OnInventoryChanged += InventoryUpdated;
    }

    private void InventoryUpdated(object sender, System.EventArgs e) {
        if (UserInventory != null) {
            HoneyAmountText.text = UserInventory.GetHoneyCount.ToString();
            PropolisAmountText.text = UserInventory.GetPropolisCount.ToString();
            RoyalJellyAmountText.text = UserInventory.GetRoyalJellyCount.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
