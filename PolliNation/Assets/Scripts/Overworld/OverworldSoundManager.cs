using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OverworldSoundManager : MonoBehaviour
{
    public static OverworldSoundManager instance;
    private AudioSource overworldSceneAudio;
    [SerializeField] private GameObject overworldSoundLayer;
    [SerializeField] private GameObject overworldButtonGO;
    private UnityEngine.UI.Image musicIconImage;
    [SerializeField] private AudioSource waspFX;
    [SerializeField] private AudioSource flowerFX;
    [SerializeField] private AudioSource deathScreenFX;

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
        overworldButtonGO.SetActive(true);
        AudioSource[] audioSources = {overworldSceneAudio, waspFX, flowerFX, deathScreenFX};
        AudioUtility.OnSceneAudioStart(audioSources, musicIconImage);

    }

    /// <summary>
    ///  On sound button click turns all sounds in scene on or off.
    /// </summary>
    public void SoundClickButton()
    {
        AudioSource[] audioSources = {overworldSceneAudio, waspFX, flowerFX, deathScreenFX};
        AudioUtility.AudioButtonClick(audioSources, musicIconImage);
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

    /// <summary>
    ///  Plays death screen soundFX.
    /// </summary>
    public void PlayDeathScreenFX(float duration)
    {
        if (!deathScreenFX.isPlaying && deathScreenFX.enabled)
        {
            StopAllMeadowSoundsOnDeath();
            StartCoroutine(AudioUtility.AudioFadeOut(deathScreenFX, duration));
        }
    }

    /// <summary>
    ///  Stops death screen soundFX.
    /// </summary>
    public void StopDeathScreenSoundFX()
    {
        if (deathScreenFX.isPlaying && deathScreenFX.enabled)
        {
            deathScreenFX.Stop();
        }
    }

    public void StopAllMeadowSoundsOnDeath()
    {
        AudioSource[] audioSources = {overworldSceneAudio, waspFX, flowerFX};
        foreach(AudioSource audioSource in audioSources)
        {
            if(audioSource != null)
            {
                audioSource.Stop();
                audioSource.enabled = false;
            }
        }
    }



    private void OnDestroy()
    {
         overworldButtonGO.GetComponent<Button>().onClick.RemoveListener(this.SoundClickButton);
    }
}