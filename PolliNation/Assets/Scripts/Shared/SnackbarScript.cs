using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SnackbarScript : MonoBehaviour {
    [SerializeField] TextMeshProUGUI text;
    private List<float> delays = new();
    
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

    // Closes snackbar if no messages are in the queue
    private void AttemptClose() {
        delays.RemoveAt(0);
        if (delays.Count == 0) {
            SetClose();
        }
    }

    // Displays the given string on the snackbar (overwriting original text)
    // and closing it after a certain amount of time
    public void SetText(string text, float delay=1.5f) {
        this.text.text = text;
        delays.Add(delay);
        SetOpen();
        Invoke(nameof(AttemptClose), delay);
    }
}