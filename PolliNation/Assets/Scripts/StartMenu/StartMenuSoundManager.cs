using UnityEngine;
using UnityEngine.UI;

public class StartMenuSoundManager : MonoBehaviour
{
    public static StartMenuSoundManager instance;
    [SerializeField] private Sprite musicIcon;
    [SerializeField] private Sprite muteMusicIcon;
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
        if (SoundOn.soundOn)
        {
            startMenuSceneAudio.enabled = true;
            musicIconImage.sprite = musicIcon;
        } else
        {
            startMenuSceneAudio.enabled = false;
            musicIconImage.sprite = muteMusicIcon;
        }
    }

    /// <summary>
    ///  On sound button click turns all sounds in scene on or off.
    /// </summary>
    public void SoundClickButton()
    {
        if (startMenuSceneAudio != null)
        {
            Debug.Log("RSRSRS start menu sound button clicked");
            startMenuSceneAudio.enabled = !startMenuSceneAudio.enabled;
            SoundOn.soundOn = startMenuSceneAudio.enabled;
            if (startMenuSceneAudio.enabled)
            {
                musicIconImage.sprite = musicIcon;
            }
            else
            {
                musicIconImage.sprite = muteMusicIcon;
            }
        } 
    }

    private void OnDestroy()
    {
         startMenuButtonGO.GetComponent<Button>().onClick.RemoveListener(this.SoundClickButton);
    }
}