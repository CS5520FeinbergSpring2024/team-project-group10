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
  [SerializeField] private static bool _builtGatheringStation;
  [SerializeField] private static bool _builtConversionStation;

  private static ResourceType _toldToBuildStorageForResourceType;
  private static bool _receivedWorkers;

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
        _builtStorageStation = true;
        if (_toldToBuildStorageForResourceType == resourceType)
        {
          _builtStorageForMoreCapacity = true;
        }
        break;
      case BuildingType.Gathering:
        _builtGatheringStation = true;
        break;
      case BuildingType.Production:
        _builtConversionStation = true;
        break;
      default:
        Debug.Log(buildingType + " is not recognized.");
        break;
    }
  }

  public static void ResourceStorageLimitReached(ResourceType resource)
  {
    if (!_builtStorageForMoreCapacity)
    {
      string resourceString = resource == ResourceType.RoyalJelly ? "Royal Jelly" : resource.ToString();
      _toldToBuildStorageForResourceType = resource;
      Snackbar.SetText($"{resourceString} limit reached. Build more {resourceString} storage stations to store more.", 3);
    }
    if (_receivedWorkers) 
    {
      SuggestBuildingGatheringAndConversion();
    }
  }

  public static void ReceivedWorkers()
  {
    _receivedWorkers = true;
    SuggestBuildingGatheringAndConversion();
  }

  private static void SuggestBuildingGatheringAndConversion()
  {
    if (!_builtGatheringStation)
    {
      Snackbar.SetText("Build a gathering station to assign worker bees to collect resources for you.", 3);
    }
    // Forces an order, but is prefereable to bombardng the user with messages.
    else if (!_builtConversionStation)
    {
      Snackbar.SetText("Build a conversion station to assign workers to produce resources like honey.");
    }
  }

}