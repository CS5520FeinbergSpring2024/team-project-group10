using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages data storage and loading for the different ScriptableObjects.
/// </summary>
public class DataStorageManager : MonoBehaviour
{
  // Approximate a static class while still descending
  // from MonoBehavior to allow calls to Awake(), Update(), etc.
  private static DataStorageManager _instance;
  public static DataStorageManager Instance
  {
    get
    {
      return _instance;
    }
  }

  // Storage filepaths.
  private static string _inventoryResourceCountsPath;
  private static string _inventoryResourceFullPath;
  private static string _inventoryResourceCarryLimitsPath;

  // ScriptableObjects whose data to store.
  [SerializeField]
  private InventoryDataSingleton _inventory;
  
  private void Awake()
  {
    Debug.Log("AWAKE");
    // Make DataStorageManagerScript a singleton.
    if (Instance != null)
    {
      Destroy(gameObject);
      return;
    }

    _instance = this;
    // So the gameobject persists between scenes.
    DontDestroyOnLoad(gameObject);

    // Filepath init
    _inventoryResourceCountsPath =
        Application.persistentDataPath + "/inventoryResourceCounts.json";
    _inventoryResourceFullPath =
        Application.persistentDataPath + "/inventoryResourceFull.json";
    _inventoryResourceCarryLimitsPath =
        Application.persistentDataPath + "/inventoryResourceCarryLimits.json";

    _inventory = new InventoryDataSingleton();

    LoadData();
  }

  private void OnDestroy()
  {
    SaveData();
  }

  // OnApplicationPause is guaranteed to be called on Android,
  // while OnApplicationQuit is not.
  private void OnApplicationPause(bool paused)
  {
    if (paused)
    {
      SaveData();
    }
  }

  public void LoadData()
  {
    LoadInventoryData();
  }

  public void SaveData()
  {
    SaveInventoryData();
  }

  // ScriptableObject-specific loading and storage.

  /// <summary>
  /// Loads the data from the storage files into the InventorySO's dictionaries.
  /// </summary>
  public void LoadInventoryData()
  {
    if (Instance._inventory != null)
    {
      Dictionary<ResourceType, int> resourceCounts = 
          DataStorageFacilitator.LoadResourceIntDict(_inventoryResourceCountsPath);
      Dictionary<ResourceType, bool> inventoryFull = 
          DataStorageFacilitator.LoadResourceBoolDict(_inventoryResourceFullPath);
      Dictionary<ResourceType, int> carryLimits = 
          DataStorageFacilitator.LoadResourceIntDict(_inventoryResourceCarryLimitsPath);
      if (resourceCounts != null && inventoryFull != null && carryLimits != null
          && resourceCounts.Count != 0 && inventoryFull.Count != 0 && carryLimits.Count != 0)
      {
        Instance._inventory.ResourceCounts = resourceCounts;
        Instance._inventory.InventoryFullDict = inventoryFull;
        Instance._inventory.ResourceCarryLimits = carryLimits;
      }
      else
      {
        Debug.Log("LoadInventorySOData" + " loaded dict(s) are null or empty.");
      }
    }
    else
    {
      Debug.LogError("_inventory is null");
    }
  }

  /// <summary>
  /// Saves the data from the InventorySO dictionaries into the storage files.
  /// </summary>
  public void SaveInventoryData()
  {
    if (Instance._inventory != null)
    {
      DataStorageFacilitator.SaveResourceIntDict(
              Instance._inventory.ResourceCounts, _inventoryResourceCountsPath);
      DataStorageFacilitator.SaveResourceBoolDict(
              Instance._inventory.InventoryFullDict, _inventoryResourceFullPath);
      DataStorageFacilitator.SaveResourceIntDict(
              Instance._inventory.ResourceCarryLimits, _inventoryResourceCarryLimitsPath);
    }
    else
    {
      Debug.LogError("_inventory is null");
    }
  }
}
