using UnityEngine;
using UnityEngine.UI;

public class StartMenuSoundManager : MonoBehaviour
{
    public static StartMenuSoundManager instance;
    [SerializeField] private AudioSource startMenuBackgroundMusic;
    [SerializeField] private GameObject startMenuButtonGO;
    [SerializeField] private AudioSource buttonClickFX;
    private UnityEngine.UI.Image musicIconImage;

    private void Awake()
    {
       if (instance == null)
        {
            instance = this;
        } 
        startMenuButtonGO.GetComponent<Button>().onClick.AddListener(this.SoundClickButton);
        musicIconImage = startMenuButtonGO.GetComponent<UnityEngine.UI.Image>();
    }

    private void Start()
    {
        startMenuButtonGO.SetActive(true);
        AudioSource[] audioSources = {startMenuBackgroundMusic, buttonClickFX};
        AudioUtility.OnSceneAudioStart(audioSources, musicIconImage);
    }

    /// <summary>
    ///  On sound button click turns all sounds in scene on or off.
    /// </summary>
    public void SoundClickButton()
    {
        AudioSource[] audioSources = {startMenuBackgroundMusic, buttonClickFX};
        AudioUtility.AudioButtonClick(audioSources, musicIconImage);
    }

    /// <summary>
    /// Play the button click sound.
    /// </summary>
    public void PlayButtonClickFX()
    {
        if (!buttonClickFX.isPlaying && buttonClickFX.enabled)
        {
            buttonClickFX.Play();
        }
    }

    private void OnDestroy()
    {
         startMenuButtonGO.GetComponent<Button>().onClick.RemoveListener(this.SoundClickButton);
    }
}