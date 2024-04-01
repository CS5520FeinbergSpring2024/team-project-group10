using UnityEngine;
using UnityEngine.UI;


public class WorkersMenuButton : MonoBehaviour, ILaunchMenuButton
{
    //WorkersMenuScript workersMenuScript;
    //Button workersMenuButton;

    void Start()
    {
        //workersMenuScript = GameObject.FindObjectOfType<WorkersMenuScript>(true);
        //workersMenuButton = GetComponentInChildren<Button>();
    }

    public void ClickButton()
    {
        /*Debug.Log("ClickButton method called");
        workersMenuScript.setOpen();
        Debug.Log("Build button was clicked");

        // Make the BuildButtonImage invisible
        if (workersMenuButton != null)
        {
            workersMenuButton.enabled = false;
        }*/
    }

    public void ReappearButton()
    {
        /*// Make the BuildButtonImage visible again
        if (workersMenuButton != null)
        {
            workersMenuButton.enabled = true;
        }*/
    }


    // Update is called once per frame
    void Update()
    {

    }

}
