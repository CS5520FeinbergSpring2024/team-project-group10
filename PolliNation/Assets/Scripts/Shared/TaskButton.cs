using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class TaskButton : MonoBehaviour
{
    public TaskScriptableObject Tasks;
    public GameObject button;

    void Start()
    {
        CheckAnimateButton();
        if (Tasks != null) {
            Tasks.OnTaskStatusChange += TasksUpdatedButton;
        }
    }

    /// <summary>
    /// Method subscribed to OnTaskStatusChange EventHandler.
    /// On a task status change calls CheckAnimateButton method.
    /// </summary>
    private void TasksUpdatedButton(object sender, System.EventArgs e) {
        CheckAnimateButton();
    }

    /// <summary>
    /// Plays button animation if there is an unclaimed task reward.
    /// </summary>
    private void CheckAnimateButton()
    {
        bool checkUnclaimedRewards = false;
        if (button != null)
        {
        // check if any tasks are completed but reward not claimed
        foreach(Task task in Tasks.GetTasks())
        {
            if (task.IsComplete && !task.IsClaimed)
            {
                checkUnclaimedRewards = true;
                break;
            } 
        }
        if (checkUnclaimedRewards == true)
        {
            button.GetComponent<Animator>().enabled = true;
        } 
        else
        {
            button.GetComponent<Animator>().enabled = false;
            // reset roation back to default
            button.transform.rotation = Quaternion.identity;
        }
        }
    }

}