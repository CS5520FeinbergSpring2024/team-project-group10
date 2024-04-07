using UnityEngine;

public class TaskMenuScript : MonoBehaviour
{
    public TaskScriptableObject Tasks;
    public GameObject TaskGameObject;
    public GameObject ScrollContainer;

    void Start()
    {   
        // make the menu not visible upon scene start
        gameObject.SetActive(false);
    }

    void Awake()
    {
         // intial load of tasks into menu
        if (Tasks != null) {
            LoadTasks();
        }
    }

    /// <summary>
    /// Loads all tasks in TaskScriptableObject into Task Game Objects in menu.
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
                TaskGameObject.GetComponent<TaskMenuTask>().AssignValues(task);
            }
        }
    }

}