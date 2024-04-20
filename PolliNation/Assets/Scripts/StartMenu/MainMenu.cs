using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() 
    {
        StartMenuSoundManager.instance.PlayButtonClickFX();
        string hiveScene = "Hive";
        SceneManager.LoadScene(hiveScene);
    }

}
