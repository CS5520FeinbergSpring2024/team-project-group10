using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

/// <summary>
/// Facilitates saving to and loading from persistent data storage.
/// 
/// Contains methods to load and save:
///     - Dictionary<ResourceType, int>
///     - Dictionary<ResourceType, bool>
/// </summary>
internal static class DataStorageFacilitator
{
  /// Dict<ResourceType, int>

  /// <summary>
  /// Serializes the given dictionary and writes it to the given file.
  /// </summary>
  /// <param name="dict">The dictionary to serialize.</param>
  /// <param name="path">The Json file in which to store the serialized data.</param>
  internal static void SaveResourceIntDict(Dictionary<ResourceType, int> dict, string path)
  {
    try
    {
      dict ??= new();
      string jsonString = JsonConvert.SerializeObject(dict);
      Debug.Log("Writing: " + jsonString);
      File.WriteAllText(path, jsonString);
    }
    // Swallow any exceptions.
    catch (Exception ex)
    {
      Debug.LogException(ex);
    }
  }

  /// <summary>
  /// Deserializes the Json string stored in the give file and returns the
  /// resulting Dictionary<ResourceType, int>.
  /// </summary>
  /// <param name="path">The path to the Json file to deserialize.</param>
  /// <returns>The Dictionary<ResourceType, int> stored in the file.
  /// or an empty dictionary.</returns>
  internal static Dictionary<ResourceType, int> LoadResourceIntDict(string path)
  {
    Dictionary<ResourceType, int> dict = new();
    try
    {
      if (File.Exists(path))
      {
        string jsonString = File.ReadAllText(path);
        Debug.Log("Read: " + jsonString);
        dict = JsonConvert.DeserializeObject<Dictionary<ResourceType, int>>(jsonString);
      }
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
    }
    return dict;
  }

  /// Dict<ResourceType, bool>

  /// <summary>
  /// Serializes the given dictionary and writes it to the given file.
  /// </summary>
  /// <param name="dict">The dictionary to serialize.</param>
  /// <param name="path">The Json file in which to store the serialized data.</param>
  internal static void SaveResourceBoolDict(Dictionary<ResourceType, bool> dict, string path)
  {
    try
    {
      dict ??= new();
      string jsonString = JsonConvert.SerializeObject(dict);
      Debug.Log("Writing: " + jsonString);
      File.WriteAllText(path, jsonString);
    }
    // Swallow any exceptions.
    catch (Exception ex)
    {
      Debug.LogException(ex);
    }
  }

  /// <summary>
  /// Deserializes the Json string stored in the give file and returns the
  /// resulting Dictionary<ResourceType, bool>.
  /// </summary>
  /// <param name="path">The path to the Json file to deserialize.</param>
  /// <returns>The Dictionary<ResourceType, bool> stored in the file.
  /// or an empty dictionary.</returns>
  internal static Dictionary<ResourceType, bool> LoadResourceBoolDict(string path)
  {
    Dictionary<ResourceType, bool> dict = new();
    try
    {
      if (File.Exists(path))
      {
        string jsonString = File.ReadAllText(path);
        Debug.Log("Read: " + jsonString);
        dict = JsonConvert.DeserializeObject<Dictionary<ResourceType, bool>>(jsonString);
      }
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
    }
    return dict;
  }
}
