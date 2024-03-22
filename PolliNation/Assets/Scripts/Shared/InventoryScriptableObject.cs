using System;
using System.Collections.Generic;
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

    public event EventHandler OnInventoryChanged;

    //get accessors 
    public int GetNectarCount => _nectarCount;
    public int GetPollenCount => _pollenCount;
    public int GetWaterCount => _waterCount;
    public int GetBudsCount => _budsCount;
    public int GetHoneyCount => _honeyCount;
    public int GetPropolisCount => _propolisCount;
    public int GetRoyalJellyCount => _royalJellyCount;

    // update methods
    public void  UpdatePollenCount(int amount) 
    {
        if (_pollenCount + amount < 0) {
            _pollenCount = 0;
        } else {
            _pollenCount += amount;
        }
        // check if there are subscribers to event and if so trigger;
        OnInventoryChanged?.Invoke(this, EventArgs.Empty);

    }

     public void  UpdateNectarCount(int amount) 
     {
        if (_nectarCount + amount < 0) {
            _nectarCount = 0;
        } else {
            _nectarCount += amount;
        }
        // check if there are subscribers to event and if so trigger;
        OnInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

    public void  UpdateWaterCount(int amount) 
    {
        if (_waterCount + amount < 0) {
            _waterCount = 0;
        } else {
            _waterCount += amount;
        }
        // check if there are subscribers to event and if so trigger;
        OnInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

    public void  UpdateBudsCount(int amount) 
    {
        if (_budsCount + amount < 0) {
            _budsCount = 0;
        } else {
            _budsCount += amount;
        }
        // check if there are subscribers to event and if so trigger;
        OnInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

    public void  UpdateHoneyCount(int amount) 
    {
        if (_honeyCount + amount < 0) {
            _honeyCount = 0;
        } else {
            _honeyCount += amount;
        }
        // check if there are subscribers to event and if so trigger;
        OnInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

    public void  UpdatePropolisCount(int amount) 
    {
        if (_propolisCount + amount < 0) {
            _propolisCount = 0;
        } else {
            _propolisCount += amount;
        }
        // check if there are subscribers to event and if so trigger;
        OnInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

    public void  UpdateRoyalJellyCount(int amount) 
    {
        if (_royalJellyCount + amount < 0) {
            _royalJellyCount = 0;
        } else {
            _royalJellyCount += amount;
        }
        // check if there are subscribers to event and if so trigger;
        OnInventoryChanged?.Invoke(this, EventArgs.Empty);
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
