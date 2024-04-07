using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskMenuTask : MonoBehaviour
{
    public TextMeshProUGUI TaskTitleText;
    public TextMeshProUGUI TaskDescriptionText;
    public TextMeshProUGUI NotCompleteText;
    public GameObject ClaimRewardButton;
    public GameObject completeTaskCheckGO;
    public GameObject RewardDisplayParent;
    public GameObject WorkersRewardGO;
    public GameObject HoneyRewardGO;
    public GameObject PropolisRewardGO;
    public GameObject RoyalJellyRewardGO;
    public GameObject BudsRewardGO;
    public GameObject WaterRewardGO;
    public GameObject NectarRewardGO;
    public GameObject PollenRewardGO;
    public GameObject rewardsViewPortGO;
    public GameObject rewardsContentsGO;
    public TaskScriptableObject Tasks;
    public InventoryScriptableObject UserInventory;

    private Dictionary<RewardType, GameObject> RewardDisplays= new();

    void Awake()
    {   
        if (Tasks != null) {
            Tasks.OnTaskStatusChange += TasksUpdated;
        }
        if (UserInventory != null) {
            UserInventory.OnInventoryChanged += CheckTaskRequirements;
        }
    }
    
    /// <summary>
    /// Method subscribed to OnInventoryChanged EventHandler. 
    /// Calls method to check if task requirements have been met.
    /// </summary>
    private void CheckTaskRequirements(object sender, System.EventArgs e) {
        Tasks.CheckRequirements();
    }

    /// <summary>
    /// Method subscribed to OnTaskStatusChange EventHandler.
    /// Calls AssignValues method if task menu is open to be updated.
    /// </summary>
    private void TasksUpdated(object sender, System.EventArgs e) {
        // check if menu/task GO is open in order to be able to update it
        if (TaskTitleText != null)
        {
            // get task
            Task task = Tasks.GetTask(TaskTitleText.text.ToString());
            //update assigned display values
            AssignValues(task);
        }
    }

    /// <summary>
    /// Assigns task instance values to task game object UI components
    /// </summary>
    /// <param name="task"> task instance </param>
    public void AssignValues(Task task)
    {
        TaskTitleText.text = task.Title;
        TaskDescriptionText.text = task.Description;
 
        // if dictionary isn't already set up then set it up
        if (RewardDisplays.Count == 0)
        {
            RewardDisplays.Add(RewardType.Water, WaterRewardGO);
            RewardDisplays.Add(RewardType.Pollen, PollenRewardGO);
            RewardDisplays.Add(RewardType.Nectar,NectarRewardGO);
            RewardDisplays.Add(RewardType.Buds,BudsRewardGO);
            RewardDisplays.Add(RewardType.Honey,HoneyRewardGO);
            RewardDisplays.Add(RewardType.Propolis,PropolisRewardGO);
            RewardDisplays.Add(RewardType.RoyalJelly,RoyalJellyRewardGO);
            RewardDisplays.Add(RewardType.Workers,WorkersRewardGO);
        }

        // only display rewards and amounts for task and disable all other reward type displays
        foreach(RewardType reward in Enum.GetValues(typeof(RewardType)))
        {
            // check if reward dict contains reward type
            if(task.Rewards.ContainsKey(reward))
            {
                RewardDisplays[reward].SetActive(true);
                RewardDisplays[reward].GetComponentInChildren<TextMeshProUGUI>().text = task.Rewards[reward].ToString();
            } 
            else{
                RewardDisplays[reward].SetActive(false);
            }
        }

        //Set up rewards section to dynamically resize rewards so they all fit within the viewport
        float viewWidth = rewardsViewPortGO.GetComponent<RectTransform>().rect.width;
        float viewHeight = rewardsViewPortGO.GetComponent<RectTransform>().rect.height;
        float maxRewardsPerRow = 3;
        float padding = 30;
        Vector2 originalSize = new(viewWidth, viewHeight);
        float rewardCount = task.Rewards.Count;
        float numRows = (float) Math.Ceiling(rewardCount / maxRewardsPerRow);
        float actualRewardsPerRow; 
        if (numRows == 1)
        {
            actualRewardsPerRow = rewardCount;
        }
        else
        {
            actualRewardsPerRow = maxRewardsPerRow;
        }
        Vector2 newSize = new Vector2(viewWidth / actualRewardsPerRow - padding, (viewHeight / numRows) - padding);
        // case where there is a single full row height scale is off so fixed here
        if ((rewardCount % maxRewardsPerRow  == 0 || rewardCount == 2) && numRows == 1)
        {
            newSize.y -= 75;
        } 
        rewardsContentsGO.transform.localScale =  newSize / originalSize;

        // What to display if task is/ is not completed and/or claimed
        if (!task.IsComplete) 
        {
            NotCompleteText.enabled = true;
            ClaimRewardButton.SetActive(false);
            completeTaskCheckGO.SetActive(false);
        } 
        else if (task.IsComplete && !task.IsClaimed)
        {
            NotCompleteText.enabled = false;
            ClaimRewardButton.SetActive(true);
            ClaimRewardButton.GetComponent<Button>().interactable = true;
            ClaimRewardButton.GetComponentInChildren<TextMeshProUGUI>().text = "Claim Reward";
            completeTaskCheckGO.SetActive(false);
        } 
        else if (task.IsComplete && task.IsClaimed)
        {
            NotCompleteText.enabled = false;
            ClaimRewardButton.SetActive(false);
            completeTaskCheckGO.SetActive(true);
        }
        
    }
    /// <summary>
    /// Called on claim reward button click. 
    /// Calls TaskScriptableObjectMethod ClaimReward method.
    /// </summary>
   public void ClaimRewardClick()
    {
        // match task to tasks in SO
        Task task = Tasks.GetTask(TaskTitleText.text.ToString());
        Tasks.ClaimReward(task);
    }
}