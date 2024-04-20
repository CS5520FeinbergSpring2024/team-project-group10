using UnityEngine;

public class TaskMenuScript : MonoBehaviour
{
    public TaskDataSingleton Tasks;
    public GameObject TaskGameObject;
    public GameObject ScrollContainer;
    private RewardClickSoundPlayerScript _rewardClickSoundPlayer;

    void Start()
    {   
        // make the menu not visible upon scene start
        gameObject.SetActive(false);
    }

    void Awake()
    {
        // intial load of tasks into menu
        Tasks = new TaskDataSingleton();
        _rewardClickSoundPlayer = FindObjectOfType<RewardClickSoundPlayerScript>();
        LoadTasks();
    }

    /// <summary>
    /// Loads all tasks in TaskDataSingleton into Task Game Objects in menu.
    /// </summary>
    public void LoadTasks()
    {
        if (ScrollContainer != null) 
        {
            // instantiate each task as a Task GameObject and add to menu
            foreach(Task task in Tasks.GetTasks())
            {
                // instantiate a new task game object under the scroll container
                Instantiate(TaskGameObject, ScrollContainer.transform);
                
                // call method to assign values to task game object 
                TaskMenuTask taskMenuTask = TaskGameObject.GetComponent<TaskMenuTask>();
                taskMenuTask.AssignValues(task);
                if (_rewardClickSoundPlayer != null)
                {
                    Debug.Log("DEBUG assigned play reward sound");
                    taskMenuTask.PlayRewardSound = RewardClickSoundPlayerScript.PlayClaimRewardFX;
                }
            }
        }
    }

}