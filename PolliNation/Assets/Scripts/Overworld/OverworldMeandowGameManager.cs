using UnityEngine;

public class OverworldMeandowGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TutorialSingleton.Snackbar = FindObjectOfType<SnackbarScript>(true);
        TutorialSingleton.EnteredOutside();
    }
}
