using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Tasks", menuName = "Data/TaskScriptableObject")]
public class TaskScriptableObject : ScriptableObject
{
    private List<Task> tasks = new();
    public InventoryScriptableObject UserInventory;

    public TaskScriptableObject() 
    {
        // starting tasks
        Dictionary<ResourceType, int> task1ResourceRequirements= new()
        {
            { ResourceType.Pollen, 100 },
            { ResourceType.Nectar, 100 }
        };
        Dictionary<ResourceType, int> task1ResourceRewards= new()
        {
            { ResourceType.Water, 100 },
        };
        Task task1 = new("Quest 1", "Gather 100 pollen and 100 nectar from outside", task1ResourceRequirements, task1ResourceRewards, 5);

        Dictionary<ResourceType, int> task2ResourceRequirements= new()
        {
            { ResourceType.Pollen, 1000 },
            { ResourceType.Nectar, 1000 }
        };
        Dictionary<ResourceType, int> task2ResourceRewards= new()
        {
            { ResourceType.Buds, 50},
        };
        Task task2 = new("Quest 2", "Store 1000 pollen and 1000 nectar", task2ResourceRequirements, task2ResourceRewards, 5);

        Dictionary<ResourceType, int> task3ResourceRequirements= new()
        {
            { ResourceType.Honey, 100},
        };
        Dictionary<ResourceType, int> task3ResourceRewards= new()
        {
            { ResourceType.Buds, 100},
        };
        Task task3 = new("Quest 3", "Produce 100 Honey", task3ResourceRequirements, task3ResourceRewards, 10);

        Dictionary<ResourceType, int> task4ResourceRequirements= new()
        {
            { ResourceType.Propolis, 10},
        };
        Dictionary<ResourceType, int> task4ResourceRewards= new();
        Task task4 = new("Quest 4", "Produce 10 Propolis", task4ResourceRequirements, task4ResourceRewards, 15);

        Dictionary<ResourceType, int> task5ResourceRequirements= new()
        {
            { ResourceType.Propolis, 50},
        };
        Dictionary<ResourceType, int> task5ResourceRewards= new()
        {
            { ResourceType.RoyalJelly, 1},
        };
        Task task5 = new("Quest 5", "Store 50 Propolis", task5ResourceRequirements, task5ResourceRewards, 0);

        // add created tasks to SO list
        tasks.Add(task1);
        tasks.Add(task2);
        tasks.Add(task3);
        tasks.Add(task4);
        tasks.Add(task5);

        // add listener on UserInventory Event Handler
        if (UserInventory != null) {
            UserInventory.OnInventoryChanged += CheckRequirements;
        }
    }
    
    /// <summary>
    /// Get all of the task instances.
    /// </summary>
    /// <returns> List of tasks </returns>
    public List<Task> GetTasks(){
        return tasks;
    }

    /// <summary>
    /// Get specific task by title.
    /// </summary>
    /// <param name="taskTitle"> String title of task </param>
    /// <returns> task </returns>
    public Task GetTask(String taskTitle){
        return tasks.Find( task => task.Title == taskTitle);
    }

    /// <summary>
    /// Add a task to the tasks be stored.
    /// </summary>
    public void AddTask(Task task){
        tasks.Add(task);
    }

    /// <summary>
    /// Remove a stored task.
    /// </summary>
    public void RemoveTask(Task task){
        tasks.Remove(task);
    }

    /// <summary>
    /// Checks if requirements for tasks that have not been completed
    /// have been met upon changes to inventory values. If requirements are met 
    /// task is marked as completed. 
    /// </summary>
    private void CheckRequirements(object sender, System.EventArgs e) {
        foreach(Task task in tasks)
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
        if(task.IsComplete && !task.IsClaimed)
        {
            foreach(KeyValuePair<ResourceType, int> entry in task.ResourceRewards)
            {
                UserInventory.UpdateInventory(entry.Key, entry.Value);
            }
            // when HiveScriptableObject is finished add to update workers
            if (task.WorkerReward != 0) 
            {
                // HiveManager.AddWorkers(_workerReward)
            }
            // update to set as claimed
            task.IsClaimed = true;
        } else
        {
            Debug.Log("Cannot claim task reward");
        }
    }

}