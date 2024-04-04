using System;
using System.Collections.Generic;

public class Task
{
    private readonly String _title;
    private readonly String _description;
    private readonly Dictionary<ResourceType, int> _requirements;
    private readonly Dictionary<ResourceType, int> _resourceRewards;
    private readonly int _workerReward;
    // for if tasks are later going to associated or only shown by player level
    private readonly int _level;
    private bool _isComplete = false;
    private bool _isClaimed = false;

    public Task(String title, String description, 
    Dictionary<ResourceType, int> requirements, Dictionary<ResourceType, int> resourceRewards, int workerReward = 0, int level = 1)
    {
        _title = title;
        _description = description;
        _requirements = requirements;
        _resourceRewards = resourceRewards;
        _workerReward = workerReward;
        _level = level;
    }

    public string Title
    {
        get => _title;
    }
    public string Description
    {
        get => _description;
    }
    public Dictionary<ResourceType, int> Requirements
    {
        get => _requirements;
    }
    public Dictionary<ResourceType, int> ResourceRewards
    {
        get => _resourceRewards;
    }
    public int WorkerReward
    {
        get => _workerReward;
    }
    public bool IsComplete
    {
        get => _isComplete;
        set => _isComplete = value;
    }
    public bool IsClaimed
    {
        get => _isClaimed;
        set => _isClaimed = value;
    }
    public int Level
    {
        get => _level;
    }

}
