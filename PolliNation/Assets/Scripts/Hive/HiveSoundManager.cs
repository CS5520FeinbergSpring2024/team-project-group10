using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HiveSoundManager : MonoBehaviour
{
    public static HiveSoundManager instance;
    [SerializeField] private Sprite musicIcon;
    [SerializeField] private Sprite muteMusicIcon;
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
        if (SoundOn.soundOn)
        {
            foreach (AudioSource audioSource in hiveSceneAudio)
            {
                audioSource.enabled = true;
            }
            
            musicIconImage.sprite = musicIcon;
        } else
        {
            foreach (AudioSource audioSource in hiveSceneAudio)
            {
                audioSource.enabled = false;
            }
            musicIconImage.sprite = muteMusicIcon;
        }
            
        
    }

    /// <summary>
    ///  On sound button click turns all sounds in scene on or off.
    /// </summary>
    public void SoundClickButton()
    {
        if (hiveSceneAudio != null)
        {
            foreach (AudioSource audioSource in hiveSceneAudio)
            {
            audioSource.enabled = !audioSource.enabled;
            }
            SoundOn.soundOn = hiveSceneAudio[0].enabled;
            if (hiveSceneAudio[0].enabled)
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
         hiveButtonGO.GetComponent<Button>().onClick.RemoveListener(this.SoundClickButton);
    }
}