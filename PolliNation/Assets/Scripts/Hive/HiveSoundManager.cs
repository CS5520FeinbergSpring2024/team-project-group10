using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HiveSoundManager : MonoBehaviour
{
    public static HiveSoundManager instance;
    private AudioSource[] hiveSceneAudio;
    [SerializeField] private GameObject hiveSoundLayer;
    [SerializeField] private GameObject hiveButtonGO;
    private UnityEngine.UI.Image musicIconImage;

    private void Awake()
    {
       if (instance == null)
        {
            instance = this;
        } 
        hiveButtonGO.GetComponent<Button>().onClick.AddListener(this.SoundClickButton);
        musicIconImage = hiveButtonGO.GetComponent<UnityEngine.UI.Image>();
        hiveSceneAudio = hiveSoundLayer.GetComponents<AudioSource>();
    }

    private void Start()
    {
        hiveButtonGO.SetActive(true);
        AudioUtility.OnSceneAudioStart(hiveSceneAudio, musicIconImage);
    }

    /// <summary>
    ///  On sound button click turns all sounds in scene on or off.
    /// </summary>
    public void SoundClickButton()
    {
        AudioUtility.AudioButtonClick(hiveSceneAudio, musicIconImage);
    }

    private void OnDestroy()
    {
         hiveButtonGO.GetComponent<Button>().onClick.RemoveListener(this.SoundClickButton);
    }
}