using UnityEngine;

public class OverworldMeandowGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TutorialStatic.Snackbar = FindObjectOfType<SnackbarScript>(true);
        TutorialStatic.EnteredOutside();
    }
}
