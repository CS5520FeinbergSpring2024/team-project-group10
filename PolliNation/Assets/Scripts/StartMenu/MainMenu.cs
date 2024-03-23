using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() 
    {
        string hiveScene = "Hive";
        SceneManager.LoadScene(hiveScene);
    }

}
