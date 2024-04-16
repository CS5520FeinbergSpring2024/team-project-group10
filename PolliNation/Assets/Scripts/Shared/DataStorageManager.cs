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
  public DataStorageManager Instance
  {
    get
    {
      return _instance;
    }
  }

  // Storage filepaths.
  // Inventory.
  private static string _inventoryResourceCountsPath;
  private static string _inventoryResourceFullPath;
  private static string _inventoryResourceCarryLimitsPath;
  // Hive.
  private static string _hiveBuildingDataPath;
  private static string _hiveAssignedWorkersPath;
  // It's inefficient to store these as separate files, however
  // it was faster to code it this way than to create another
  // object to store both types of worker data and serialize that.
  private static string _hiveTotalWorkersPath;
  private static string _hiveResourceLevelsPath;

  // Singleton objects whose data to store.
  // These must only be accessed through Instance.<field>.
  private InventoryDataSingleton _inventory;
  private HiveDataSingleton _hive;

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
    _hiveBuildingDataPath = 
        Application.persistentDataPath + "/hiveBuildingDataPath.json";
    _hiveAssignedWorkersPath = 
        Application.persistentDataPath + "/hiveAssignedWorkersPath.json";
    _hiveTotalWorkersPath = 
        Application.persistentDataPath + "/hiveTotalWorkersPath.json";   
    _hiveResourceLevelsPath = 
        Application.persistentDataPath + "/hiveResourceLevelsPath.json";

    _inventory = new InventoryDataSingleton();
    _hive = new HiveDataSingleton();

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
    LoadHiveData();
  }

  public void SaveData()
  {
    SaveInventoryData();
    SaveHiveData();
  }

  // ScriptableObject-specific loading and storage.

  /// <summary>
  /// Loads the data from the storage files into the Inventory's dictionaries.
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
        Debug.Log("LoadInventoryData" + " loaded dict(s) are null or empty.");
      }
    }
    else
    {
      Debug.LogError("_inventory is null");
    }
  }

  /// <summary>
  /// Saves the data from the Inventory's dictionaries into the storage files.
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
      Debug.LogError("Inventory is null");
    }
  }

  /// <summary>
  /// Loads the data from the storage files into the Hive.
  /// </summary>
  public void LoadHiveData()
  {
    if (Instance._hive != null)
    {
      List<BuildingData> buildingData = 
          DataStorageFacilitator.LoadBuildingDataList(_hiveBuildingDataPath);
      Dictionary<ResourceType, int> assignedWorkers = 
          DataStorageFacilitator.LoadResourceIntDict(_hiveAssignedWorkersPath);
      int totalWorkers = 
          DataStorageFacilitator.LoadInt(_hiveTotalWorkersPath);
      Dictionary<ResourceType, (int storageLevel, int productionLevel)> resourceLevels =
          DataStorageFacilitator.LoadResourceIntTupleDict(_hiveResourceLevelsPath);
      if (buildingData != null && assignedWorkers != null 
          && totalWorkers >= 0 && resourceLevels != null
          && buildingData.Count != 0 || assignedWorkers.Count != 0 && resourceLevels.Count != 0)
          {
            Instance._hive.BuildingData = buildingData;
            Instance._hive.TotalWorkers = totalWorkers;
            Instance._hive.AssignedWorkers = assignedWorkers;
            Instance._hive.ResourceLevels = resourceLevels;
          }
      else
      {
        Debug.Log("LoadHiveData" + " loaded dict(s) or List is null or empty.");
      }
    }
    else
    {
      Debug.LogError("Hive is null");
    }
  }

  /// <summary>
  /// Saves the data from the Hive into the storage files.
  /// </summary>
  public void SaveHiveData()
  {
    if (Instance._hive != null)
    {
      DataStorageFacilitator.SaveBuildingDataList(
          Instance._hive.BuildingData, _hiveBuildingDataPath);
      DataStorageFacilitator.SaveResourceIntDict(
          Instance._hive.AssignedWorkers, _hiveAssignedWorkersPath);
      DataStorageFacilitator.SaveInt(
          Instance._hive.TotalWorkers, _hiveTotalWorkersPath);
      DataStorageFacilitator.SaveResourceIntTupleDict(
          Instance._hive.ResourceLevels, _hiveResourceLevelsPath);
    }
    else
    {
      Debug.LogError("Hive is null");
    }
  }
}
