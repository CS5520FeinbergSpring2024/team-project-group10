using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores and manages Tasks.
/// </summary>
public class TaskDataSingleton
{
  private static TaskDataSingleton _instance;
  public static TaskDataSingleton Instance
  {
    get { return _instance; }
  }

  private List<Task> _tasks;
  private List<Task> Tasks
  {
    get { return Instance._tasks; }
    set { Instance._tasks = value; }
  }
  private InventoryDataSingleton _userInventory;
  private InventoryDataSingleton UserInventory
  {
    get { return Instance._userInventory; }
    set { Instance._userInventory = value; }
  }
  private HiveDataSingleton _hive;
  private HiveDataSingleton Hive
  {
    get { return Instance._hive; }
    set { Instance._hive = value; }
  }
  private event EventHandler _onTaskStatusChange;
  public event EventHandler OnTaskStatusChange
  {
    add { Instance._onTaskStatusChange += value; }
    remove { Instance._onTaskStatusChange -= value; }
  }

  public TaskDataSingleton()
  {
    // Make a singleton.
    if (_instance != null)
    {
      return;
    }
    _instance = this;

    UserInventory = new InventoryDataSingleton();
    Hive = new HiveDataSingleton();
    Tasks = new List<Task>();

    // starting tasks
    Dictionary<ResourceType, int> task1ResourceRequirements = new()
        {
            { ResourceType.Pollen, 100 },
            { ResourceType.Nectar, 100 }
        };
    Dictionary<RewardType, int> task1ResourceRewards = new()
        {
            { RewardType.Water, 100 },
            {RewardType.Workers, 10}
        };
    Task task1 = new("First Harvest", "Gather 100 pollen and 100 nectar from outside", task1ResourceRequirements, task1ResourceRewards);

    Dictionary<ResourceType, int> task2ResourceRequirements = new()
        {
            { ResourceType.Pollen, 1000 },
            { ResourceType.Nectar, 1000 }
        };
    Dictionary<RewardType, int> task2ResourceRewards = new()
        {
            { RewardType.Buds, 50},
            { RewardType.Workers, 10 }
        };
    Task task2 = new("Store and More", "Store 1000 pollen and 1000 nectar", task2ResourceRequirements, task2ResourceRewards);

    Dictionary<ResourceType, int> task3ResourceRequirements = new()
        {
            { ResourceType.Honey, 100},
        };
    Dictionary<RewardType, int> task3ResourceRewards = new()
        {
            { RewardType.Water, 100},
            { RewardType.Buds, 100},
            { RewardType.Workers, 10}
        };
    Task task3 = new("Production Underway", "Produce 100 Honey", task3ResourceRequirements, task3ResourceRewards);

    Dictionary<ResourceType, int> task4ResourceRequirements = new()
        {
            { ResourceType.Propolis, 10},
        };
    Dictionary<RewardType, int> task4ResourceRewards = new()
        {
            { RewardType.Water, 100},
            { RewardType.Buds, 100},
            { RewardType.Honey, 10},
            { RewardType.Propolis, 10},
            { RewardType.Workers, 20}
        };
    Task task4 = new("Manage The Hive", "Produce 10 Propolis", task4ResourceRequirements, task4ResourceRewards);

    Dictionary<ResourceType, int> task5ResourceRequirements = new()
        {
            { ResourceType.Propolis, 50},
        };
    Dictionary<RewardType, int> task5ResourceRewards = new()
        {
            { RewardType.RoyalJelly, 1},
        };
    Task task5 = new("Build, Buzz, Bee!", "Store 50 Propolis", task5ResourceRequirements, task5ResourceRewards);

    // add created tasks to SO list
    Tasks.Add(task2);
    Tasks.Add(task3);
    Tasks.Add(task4);
    Tasks.Add(task5);
    // adding last because scroll group is displaying last first for Task Menu
    Tasks.Add(task1);
  }

  /// <summary>
  /// Get all of the task instances.
  /// </summary>
  /// <returns> List of tasks </returns>
  public List<Task> GetTasks()
  {
    return Tasks;
  }

  /// <summary>
  /// Get specific task by title.
  /// </summary>
  /// <param name="taskTitle"> String title of task </param>
  /// <returns> task </returns>
  public Task GetTask(String taskTitle)
  {
    return Tasks.Find(task => task.Title == taskTitle);
  }

  /// <summary>
  /// Add a task to the tasks be stored.
  /// </summary>
  public void AddTask(Task task)
  {
    Tasks.Add(task);
    Instance._onTaskStatusChange?.Invoke(Instance, EventArgs.Empty);
  }

  /// <summary>
  /// Remove a stored task.
  /// </summary>
  public void RemoveTask(Task task)
  {
    Tasks.Remove(task);
    Instance._onTaskStatusChange?.Invoke(Instance, EventArgs.Empty);
  }

  /// <summary>
  /// Checks if requirements for tasks that have not been completed
  /// have been met upon changes to inventory values. If requirements are met 
  /// task is marked as completed. 
  /// </summary>
  public void CheckRequirements()
  {
    foreach (Task task in Tasks)
    {
      // is task is not already completed 
      if (!task.IsComplete)
      {
        Boolean checkComplete = true;
        foreach (KeyValuePair<ResourceType, int> requirement in task.Requirements)
        {
          // if resource count is greater or equal to requirement resource count
          if (UserInventory.GetResourceCount(requirement.Key) < requirement.Value)
          {
            // if any resource requirements are not met turn flag false
            checkComplete = false;
            // don't need to finish checking rest of requirements
            break;
          }
        }
        // if all requirements were met
        if (checkComplete)
        {
          task.IsComplete = true;
          Instance._onTaskStatusChange?.Invoke(Instance, EventArgs.Empty);
        }
      }
    }
  }

  /// <summary>
  /// Claim reward associated with task if requirements are met.
  /// </summary>
  /// <param name="task"> the task to claim the reward of </param>
  public void ClaimReward(Task task)
  {
    if (task.IsComplete && !task.IsClaimed)
    {
      foreach (KeyValuePair<RewardType, int> entry in task.Rewards)
      {
        if (!entry.Key.Equals(RewardType.Workers) && entry.Value != 0)
        {
          if (Enum.TryParse(entry.Key.ToString(), out ResourceType rewardResource)
          && Enum.IsDefined(typeof(ResourceType), rewardResource))
          {
            UserInventory.UpdateInventory(rewardResource, entry.Value);
          }
        }
        else if (entry.Key.Equals(RewardType.Workers) && entry.Value != 0)
        {
          Hive.AddWorkers(entry.Value);
        }
      }

      // update to set as claimed and notify any listeners of event
      task.IsClaimed = true;
      Instance._onTaskStatusChange?.Invoke(Instance, EventArgs.Empty);
    }
    else
    {
      Debug.Log("Cannot claim task reward");
    }
  }
}
