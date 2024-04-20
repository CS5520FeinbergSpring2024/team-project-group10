using UnityEngine;
using TMPro;

public class SnackbarScript : MonoBehaviour {
    [SerializeField] TextMeshProUGUI text;
    
    // Start is called before the first frame update
    void Start() {
        SetClose();
    }

    // Make the Snackbar appear
    public void SetOpen() {
        gameObject.SetActive(true);
    }
    
    // Make the Snackbar disappear
    public void SetClose() {
        gameObject.SetActive(false);
    }

    // Displays the given string on the snackbar (overwriting original text)
    // and closing it after a certain amount of time
    public void SetText(string text, int delay=1) {
        this.text.text = text;
        SetOpen();
        Invoke(nameof(SetClose), delay);
    }
}