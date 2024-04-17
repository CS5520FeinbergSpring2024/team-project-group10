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
  /// <returns>The Dictionary<ResourceType, int> stored in the file
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
  /// <returns>The Dictionary<ResourceType, bool> stored in the file
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

  /// Dict<ResourceType, (int, int)>

  /// <summary>
  /// Serializes the given dictionary and writes it to the given file.
  /// </summary>
  /// <param name="dict">The dictionary to serialize.</param>
  /// <param name="path">The Json file in which to store the serialized data.</param>
  internal static void SaveResourceIntTupleDict(Dictionary<ResourceType, (int, int)> dict, string path)
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
  /// resulting Dictionary<ResourceType, (int, int)>.
  /// </summary>
  /// <param name="path">The path to the Json file to deserialize.</param>
  /// <returns>The Dictionary<ResourceType, (int, int)> stored in the file
  /// or an empty dictionary.</returns>
  internal static Dictionary<ResourceType, (int, int)> LoadResourceIntTupleDict(string path)
  {
    Dictionary<ResourceType, (int, int)> dict = new();
    try
    {
      if (File.Exists(path))
      {
        string jsonString = File.ReadAllText(path);
        Debug.Log("Read: " + jsonString);
        dict = JsonConvert.DeserializeObject<Dictionary<ResourceType, (int, int)>>(jsonString);
      }
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
    }
    return dict;
  }

  /// List<Building>

  /// <summary>
  /// Serializes the given list and writes it to the given file.
  /// </summary>
  /// <param name="list">The list to serialize.</param>
  /// <param name="path">The Json file in which to store the serialized data.</param>
  internal static void SaveBuildingDataList(List<BuildingData> list, string path)
  {
    try
    {
      list ??= new();
      // JsonConvert can't convert Vectors so get a list of serialized
      // objects with JsonUtility.
      List<string> jsonList = new();
      foreach (BuildingData bd in list)
      {
        if (bd != null)
        {
          jsonList.Add(JsonUtility.ToJson(bd));
        }
      }

      // JsonUtility can't serialize a list.
      string jsonString = JsonConvert.SerializeObject(jsonList);
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
  /// resulting List<BuildingData>.
  /// </summary>
  /// <param name="path">The path to the Json file to deserialize.</param>
  /// <returns>The List<BuildingData> stored in the file or an empty list.</returns>
  internal static List<BuildingData> LoadBuildingDataList(string path)
  {
    List<BuildingData> list = new();
    try
    {
      if (File.Exists(path))
      {
        string jsonString = File.ReadAllText(path);
        Debug.Log("Read: " + jsonString);

        List<string> jsonList = JsonConvert.DeserializeObject<List<string>>(jsonString);
        foreach (string bdString in jsonList)
        {
          BuildingData bd = JsonUtility.FromJson<BuildingData>(bdString);
          if (bd != null)
          {
            list.Add(bd);
          }
        }
      }
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
    }

    return list;
  }

  /// List<Task>

  /// <summary>
  /// Serializes the given list and writes it to the given file.
  /// </summary>
  /// <param name="list">The list to serialize.</param>
  /// <param name="path">The Json file in which to store the serialized data.</param>
  internal static void SaveTaskList(List<Task> list, string path)
  {
    try
    {
      list ??= new();
      string jsonString = JsonConvert.SerializeObject(list);
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
  /// resulting List<Task>.
  /// </summary>
  /// <param name="path">The path to the Json file to deserialize.</param>
  /// <returns>The List<Task> stored in the file or an empty list.</returns>
  internal static List<Task> LoadTaskList(string path)
  {
    List<Task> list = new();
    try
    {
      if (File.Exists(path))
      {
        string jsonString = File.ReadAllText(path);
        Debug.Log("Read: " + jsonString);
        list = JsonConvert.DeserializeObject<List<Task>>(jsonString);
      }
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
    }
    return list;
  }

  /// int.
  /// Yes, this is silly, but it was faster to code this than creat a new structure.

  /// <summary>
  /// Serializes int and writes it to the given file.
  /// </summary>
  /// <param name="int">The int to serialize.</param>
  /// <param name="path">The Json file in which to store the serialized data.</param>
  internal static void SaveInt(int num, string path)
  {
    try
    {
      // JsonUtility can't serialize a list.
      string jsonString = JsonConvert.SerializeObject(num);
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
  /// resulting int.
  /// </summary>
  /// <param name="path">The path to the Json file to deserialize.</param>
  /// <returns>The int stored in the file. </returns>
  internal static int LoadInt(string path)
  {
    int num = 0;
    try
    {
      if (File.Exists(path))
      {
        string jsonString = File.ReadAllText(path);
        Debug.Log("Read: " + jsonString);
        num = JsonConvert.DeserializeObject<int>(jsonString);
      }
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
    }
    return num;
  }
}
