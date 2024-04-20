using System.Collections.Generic;
using UnityEngine;

public class TutorialSingleton
{
  private static TutorialSingleton _instance;
  public static TutorialSingleton Instance
  {
    get { return _instance; }
  }
  public static SnackbarScript Snackbar;

  // Milestone suggestions.
  [SerializeField] private static bool _wentOutside;
  // Whether they've built a storage station when they're reached 
  // a resource collection limit.
  [SerializeField] private static bool _builtStorageForMoreCapacity;
  [SerializeField] private static bool _builtStorageStation;


  private static ResourceType _toldToBuildStorageForResourceType;

  public TutorialSingleton()
  {
    if (_instance != null)
    {
      return;
    }
    _instance = this;
  }

  public static void EnteredHive()
  {
    if (!_wentOutside)
    {
      Snackbar.SetText("Head outside to collect resources like pollen and nectar.", 3);
    }
  }

  public static void EnteredOutside()
  {
    _wentOutside = true;
  }

  public static void BuiltStation(BuildingType buildingType, ResourceType resourceType)
  {
    switch (buildingType)
    {
      case BuildingType.Storage:
        BuiltStorageStation(resourceType);
        break;
      default:
        Debug.Log(buildingType + " is not recognized.");
        break;
    }
  }

  private static void BuiltStorageStation(ResourceType resource)
  {
    _builtStorageStation = true;
    if (_toldToBuildStorageForResourceType == resource)
    {
      BuiltStorageForMoreCapacity();
    }
  }

  private static void BuiltStorageForMoreCapacity()
  {
    _builtStorageForMoreCapacity = true;
  }

  public static void ResourceStorageLimitReached(ResourceType resource)
  {
    if (!_builtStorageForMoreCapacity)
    {
      string resourceString = resource == ResourceType.RoyalJelly ? "Royal Jelly" : resource.ToString();
      _toldToBuildStorageForResourceType = resource;
      Snackbar.SetText($"{resourceString} limit reached. Build more {resourceString} storage stations to store more.", 3);
    }
  }

  

}