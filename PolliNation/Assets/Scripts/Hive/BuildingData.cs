using System;
using UnityEngine;

/// <summary>
/// Stores core, serializeable, data for a Building that is not tied
/// to a Unity GameObect's lifecycle.
/// </summary>
[Serializable]
public class BuildingData
{
  public BuildingType Type;
  public ResourceType ResourceType;
  public Vector2 TileID;
  public Vector3 Position;

  public BuildingData(BuildingType type, ResourceType resourceType, 
                      Vector2 tileID, Vector3 position)
    {
        Type = type;
        ResourceType = resourceType;
        TileID = tileID;
        Position = position;
    }
}
