using System;
using UnityEngine;

/// <summary>
/// Stores core, serializeable, data for a Building that is not tied
/// to a Unity GameObect's lifecycle.
/// </summary>
[Serializable]
public class BuildingData {
  public BuildingType BuildingType;
  public ResourceType ResourceType;
  public Vector3 Position;

  public BuildingData(BuildingType buildingType, ResourceType resourceType, Vector3 position) {
        BuildingType = buildingType;
        ResourceType = resourceType;
        Position = position;
    }
}
