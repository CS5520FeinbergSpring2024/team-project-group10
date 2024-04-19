using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OverworldSoundManager : MonoBehaviour
{
    public static OverworldSoundManager instance;
    [SerializeField] private Sprite musicIcon;
    [SerializeField] private Sprite muteMusicIcon;
    private AudioSource overworldSceneAudio;
    [SerializeField] private GameObject overworldSoundLayer;
    [SerializeField] private GameObject overworldButtonGO;
    private UnityEngine.UI.Image musicIconImage;
    [SerializeField] private AudioSource waspFX;
    [SerializeField] private AudioSource flowerFX;

    private void Awake()
    {
       if (instance == null)
        {
            instance = this;
        } 
        overworldButtonGO.GetComponent<Button>().onClick.AddListener(this.SoundClickButton);
        musicIconImage = overworldButtonGO.GetComponent<UnityEngine.UI.Image>();
        overworldSceneAudio = overworldSoundLayer.GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "OverworldMeadow")
        {
            overworldButtonGO.SetActive(true);
            if (SoundOn.soundOn)
            {
                overworldSceneAudio.enabled = true;
                waspFX.enabled = true;
                flowerFX.enabled = true;
                musicIconImage.sprite = musicIcon;
            } else
            {
                overworldSceneAudio.enabled = false;
                waspFX.enabled = false;
                flowerFX.enabled = false;
                musicIconImage.sprite = muteMusicIcon;
            }
            
        } 
        else
        {
            overworldSceneAudio.enabled = false;
            waspFX.enabled = false;
            flowerFX.enabled = false;
            overworldButtonGO.SetActive(false);
        }
        
    }

    /// <summary>
    ///  On sound button click turns all sounds in scene on or off.
    /// </summary>
    public void SoundClickButton()
    {
        if (SceneManager.GetActiveScene().name == "OverworldMeadow")
        {
            if (overworldSceneAudio != null)
            {
                overworldSceneAudio.enabled = !overworldSceneAudio.enabled;
                waspFX.enabled = !waspFX.enabled;
                flowerFX.enabled= !flowerFX.enabled;
                SoundOn.soundOn = overworldSceneAudio.enabled;
                if (overworldSceneAudio.enabled)
                {
                    musicIconImage.sprite = musicIcon;
                }
                else
                {
                    musicIconImage.sprite = muteMusicIcon;
                }
            }
        }
    }

    /// <summary>
    ///  Plays Wasp soundFX.
    /// </summary>
    public void PlayWaspSoundFX()
    {
        if (!waspFX.isPlaying && waspFX.enabled)
        {
            waspFX.Play();
        }
    }

    /// <summary>
    ///  Stops Wasp soundFX.
    /// </summary>
    public void StopWaspSoundFX()
    {
        if (waspFX.isPlaying && waspFX.enabled)
        {
            waspFX.Stop();
        }
    }

    /// <summary>
    ///  Plays ResourceCollection soundFX.
    /// </summary>
    public void PlayResoureCollectionSoundFX()
    {
        if (!flowerFX.isPlaying && flowerFX.enabled)
        {
            flowerFX.Play();
        }
    }

    /// <summary>
    ///  Stops ResourceCollection soundFX.
    /// </summary>
    public void StopResourceCollectionSoundFX()
    {
        if (flowerFX.isPlaying && flowerFX.enabled)
        {
            flowerFX.Stop();
        }
    }

    private void OnDestroy()
    {
         overworldButtonGO.GetComponent<Button>().onClick.RemoveListener(this.SoundClickButton);
    }
}

public class SoundOn
{
    public static bool soundOn = true;
}
