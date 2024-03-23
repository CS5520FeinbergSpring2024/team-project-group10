using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName ="UserInventory", menuName = "Data/UserInventory")]
public class InventoryScriptableObject : ScriptableObject
{
    // set initial values to zero for now 
    // need to add method to get/set values from/to file with save system upon game start and end
    [SerializeField] private int _nectarCount = 0;
    [SerializeField] private int _pollenCount = 0;
    [SerializeField] private int _waterCount = 0;
    [SerializeField] private int _budsCount = 0;
    [SerializeField] private int _honeyCount = 0;
    [SerializeField] private int _propolisCount = 0;
    [SerializeField] private int _royalJellyCount = 0;
    private int resourceCopy;


    /* // need to make all fields static for dict
    Dictionary<string, int> inventoryFields = new Dictionary<string, int>() 
    {
        {"Nectar", _nectarCount},
        {"Pollen", _pollenCount},
        {"Water", _waterCount},
        {"Buds", _budsCount},
        {"Honey",_honeyCount},
        {"Propolis", _propolisCount},
        {"RoyalJelly", _royalJellyCount}
    };
    */

    public event EventHandler OnInventoryChanged;

    //get accessors 
    public int GetNectarCount => _nectarCount;
    public int GetPollenCount => _pollenCount;
    public int GetWaterCount => _waterCount;
    public int GetBudsCount => _budsCount;
    public int GetHoneyCount => _honeyCount;
    public int GetPropolisCount => _propolisCount;
    public int GetRoyalJellyCount => _royalJellyCount;

    // update method
    public void UpdateInventory(ResourceType resourceType, int amount) 
    {  
        switch(resourceType.ToString().ToLower())
            {
                case "pollen":
                    if (_pollenCount + amount < 0) {
                        _pollenCount = 0;
                    } else {
                        _pollenCount += amount;
                    }
                    break;
                case "nectar":
                    if (_nectarCount + amount < 0) {
                        _nectarCount = 0;
                    } else {
                        _nectarCount += amount;
                    }
                    break;
                case "water":
                    if (_waterCount + amount < 0) {
                        _waterCount = 0;
                    } else {
                        _waterCount += amount;
                    }
                    break;
                case "buds":
                    if (_budsCount + amount < 0) {
                        _budsCount = 0;
                    } else {
                        _budsCount += amount;
                    }
                    break;
                case "honey":
                    if (_honeyCount + amount < 0) {
                        _honeyCount = 0;
                    } else {
                        _honeyCount += amount;
                    }
                    break;
                case "propolis":
                    if (_propolisCount + amount < 0) {
                        _propolisCount = 0;
                    } else {
                        _propolisCount += amount;
                    }
                    break;
                case "royaljelly":
                    if (_royalJellyCount + amount < 0) {
                        _royalJellyCount = 0;
                    } else {
                        _royalJellyCount += amount;
                    }
                    break;
                default:
                    Debug.Log("No matching case");
                    break;
            }
        OnInventoryChanged?.Invoke(this, EventArgs.Empty); 


        // attempt to use dictionary, must make fields static, not updating values in asset??
        /*
        string resource = resourceType.ToString();
        if (inventoryFields.ContainsKey(resource)) {
        Debug.Log("HELP in first if");
            if (inventoryFields[resource] + amount < 0) {
                inventoryFields[resource] = 0;
            } else {
                Debug.Log("HELP should be adding");
                inventoryFields[resource] += amount;
            }
            Debug.Log("update: " + inventoryFields[resource]);
            // check if there are subscribers to event and if so trigger;
            OnInventoryChanged?.Invoke(this, EventArgs.Empty);
        
        } */
    }

    public void ResetInventory() {
        _nectarCount = 0;
        _pollenCount = 0;
        _waterCount = 0;
        _budsCount = 0;
        _honeyCount = 0;
        _propolisCount = 0;
        _royalJellyCount = 0;
        // check if there are subscribers to event and if so trigger;
        OnInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

}
