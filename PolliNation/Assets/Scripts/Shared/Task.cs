using System;
using System.Collections.Generic;

public class Task
{
    private readonly string _title;
    private readonly string _description;
    private readonly Dictionary<ResourceType, int> _requirements;
    private readonly Dictionary<RewardType, int> _rewards;
    // for if tasks are later going to associated or only shown by player level
    private readonly int _level;
    private bool _isComplete = false;
    private bool _isClaimed = false;

    public Task(string title, string description, 
    Dictionary<ResourceType, int> requirements, Dictionary<RewardType, int> rewards, int level = 1)
    {
        _title = title;
        _description = description;
        _requirements = requirements;
        _rewards = rewards;
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
    public Dictionary<RewardType, int> Rewards
    {
        get => _rewards;
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
