
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores inventory data. Inclues update methods for the inventory.
/// </summary>
public class InventoryDataSingleton
{
  // For singleton.
  private static InventoryDataSingleton _instance;
  public static InventoryDataSingleton Instance
  {
    get { return _instance; }
  }

  // Instance variables. Should only be accessed through Instance.<variable>
  // Properties give appearance of accessing instance variables so the interface
  // is unchanged from InventoryScriptableObject.

  // set initial values to zero for now 
  // need to add method to get/set values from/to file with save system upon game start and end
  private Dictionary<ResourceType, int> _resourceCounts = new();
  internal Dictionary<ResourceType, int> ResourceCounts
  {
    get { return Instance._resourceCounts; }
    set { Instance._resourceCounts = value; }
  }
  private Dictionary<ResourceType, int> _resourceCarryLimits = new();
  internal Dictionary<ResourceType, int> ResourceCarryLimits
  {
    get { return Instance._resourceCarryLimits; }
    set { Instance._resourceCarryLimits = value; }
  }
  private Dictionary<ResourceType, bool> _inventoryFull = new();
  internal Dictionary<ResourceType, bool> InventoryFullDict
  {
    get { return Instance._inventoryFull; }
    set { Instance._inventoryFull = value; }
  }
  private event EventHandler _onInventoryChanged;
  public event EventHandler OnInventoryChanged
  {
    add { lock(this) Instance._onInventoryChanged += value; }
    remove { lock(this) Instance._onInventoryChanged -= value; }
  }
  private HiveDataSingleton _hive;
  private HiveDataSingleton Hive
  {
    get { return Instance._hive; }
    set { Instance._hive = value; }
  }
  private int _storagePerLevel = 1000;
  private int StoragePerLevel
  {
    get { return Instance._storagePerLevel; }
    set { Instance._storagePerLevel = value; }
  }

  public InventoryDataSingleton()
  {
    // Make a singleton.
    if (_instance != null)
    {
      return;
    }
    _instance = this;

    Hive = new HiveDataSingleton();

    foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType))) {
        Instance._resourceCounts.Add(resource, 0);
        Instance._inventoryFull.Add(resource, false);

        // set initial user inventory carry only (what the user can "carry" wihtout storage buildings) limits 
        if (resource == ResourceType.Pollen || resource == ResourceType.Nectar 
        || resource == ResourceType.Water || resource == ResourceType.Buds)
        {
            Instance._resourceCarryLimits.Add(resource,100);
        }
        else
        {
            Instance._resourceCarryLimits.Add(resource,10);
        }
    }
  }

  /// <summary>
  /// Returns the inventory amount for the <c>resource</c>
  /// </summary>
  /// <param name="resource">resource type</param>
  /// <returns> amount of resource in inventory </returns>
  public int GetResourceCount(ResourceType resource){
      return Instance._resourceCounts[resource];
  }

  /// <summary>
  /// Returns the storage limit for the <c>resource</c>
  /// </summary>
  /// <param name="resource"> resource type</param>
  /// <returns> storage limit for resource </returns>
  public int GetStorageLimit(ResourceType resource)
  {
    int storageCap = Math.Max(ResourceCarryLimits[resource],
                              Hive.GetStationLevels(resource).storageLevel * StoragePerLevel);
    return storageCap;
  }

  /// <summary>
  /// Updates the inventory count for the <c>resource</c> by <c>amount</c> 
  /// </summary>
  /// <param name="resource"> resource type</param>
  /// <param name="amount"> amount to update resource by </param>
  public void UpdateInventory(ResourceType resource, int amount) 
  {  
      int newInventoryValue = Instance._resourceCounts[resource] + amount;
      if (newInventoryValue >= 0 &&  (newInventoryValue <= GetStorageLimit(resource))) 
      {
          Instance._resourceCounts[resource] = newInventoryValue;
          Instance._inventoryFull[resource] = false;
      }
      else if (newInventoryValue < 0)
      {
          Debug.Log("Not enough resource in inventory");
          Instance._inventoryFull[resource] = false;
      }
      else
      {
          // set count to max allowed for resource 
          Instance._resourceCounts[resource] = GetStorageLimit(resource);
          Instance._inventoryFull[resource] = true;
          TutorialStatic.ResourceStorageLimitReached(resource);
          Debug.Log("Inventory full for resource, cannot add more to inventory");
      }
      Instance._onInventoryChanged?.Invoke(Instance, EventArgs.Empty); 
  }

  public bool InventoryFull(ResourceType resource)
  {
      return Instance._inventoryFull[resource];
  }
}
