using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages data storage and loading for the different ScriptableObjects.
/// 
/// This approach carries some risks since it means that a class (this one) 
/// other than the ScriptableObject (SO) itself is directly manipulating the
/// SO's data. It would be preferable to have the SOs manage their data
/// loading and storage within themselves, however the lifecycle and nature
/// of SOs seems to prevent that. Given that all of the access that breaks
/// object encapsulation is happening within this assembly, this risk has
/// been deemed acceptable.
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
  private InventoryScriptableObject _inventorySO;
  
  private void Awake()
  {
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

    LoadData();
  }

  private void OnDestroy()
  {
    SaveData();
  }

  private void OnApplicationQuit()
  {
    SaveData();
  }

  public void LoadData()
  {
    Debug.Log("LOADDATA");
    LoadInventorySOData();
  }

  public void SaveData()
  {
    Debug.Log("SAVEDATA");
    SaveInventorySOData();
  }

  // ScriptableObject-specific loading and storage.

  /// <summary>
  /// Loads the data from the storage files into the InventorySO's dictionaries.
  /// </summary>
  public void LoadInventorySOData()
  {
    if (Instance._inventorySO != null)
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
        Instance._inventorySO.resourceCounts = resourceCounts;
        Instance._inventorySO.inventoryFull = inventoryFull;
        Instance._inventorySO.resourceCarryLimits = carryLimits;
      }
      else
      {
        Debug.Log("LoadInventorySOData" + " loaded dict(s) are null or empty.");
      }
    }
    else
    {
      Debug.LogError("_inventorySO is null");
    }
  }

  /// <summary>
  /// Saves the data from the InventorySO dictionaries into the storage files.
  /// </summary>
  public void SaveInventorySOData()
  {
    if (Instance._inventorySO != null)
    {
      DataStorageFacilitator.SaveResourceIntDict(
              Instance._inventorySO.resourceCounts, _inventoryResourceCountsPath);
      DataStorageFacilitator.SaveResourceBoolDict(
              Instance._inventorySO.inventoryFull, _inventoryResourceFullPath);
      DataStorageFacilitator.SaveResourceIntDict(
              Instance._inventorySO.resourceCarryLimits, _inventoryResourceCarryLimitsPath);
    }
    else
    {
      Debug.LogError("_inventorySO is null");
    }
  }
}
