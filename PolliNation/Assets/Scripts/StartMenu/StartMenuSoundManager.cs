using UnityEngine;
using UnityEngine.UI;

public class StartMenuSoundManager : MonoBehaviour
{
    public static StartMenuSoundManager instance;
    private AudioSource startMenuSceneAudio;
    [SerializeField] private GameObject startMenuSoundLayer;
    [SerializeField] private GameObject startMenuButtonGO;
    private UnityEngine.UI.Image musicIconImage;

    private void Awake()
    {
       if (instance == null)
        {
            instance = this;
        } 
        startMenuButtonGO.GetComponent<Button>().onClick.AddListener(this.SoundClickButton);
        musicIconImage = startMenuButtonGO.GetComponent<UnityEngine.UI.Image>();
        startMenuSceneAudio = startMenuSoundLayer.GetComponent<AudioSource>();
    }

    private void Start()
    {
        startMenuButtonGO.SetActive(true);
        AudioSource[] audioSources = {startMenuSceneAudio};
        AudioUtility.OnSceneAudioStart(audioSources, musicIconImage);
    }

    /// <summary>
    ///  On sound button click turns all sounds in scene on or off.
    /// </summary>
    public void SoundClickButton()
    {
        AudioSource[] audioSources = {startMenuSceneAudio};
        AudioUtility.AudioButtonClick(audioSources, musicIconImage);
    }

    private void OnDestroy()
    {
         startMenuButtonGO.GetComponent<Button>().onClick.RemoveListener(this.SoundClickButton);
    }
}