using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HiveSoundManager : MonoBehaviour
{
    public static HiveSoundManager instance;
    private AudioSource[] hiveSceneAudio;
    [SerializeField] private AudioSource buttonClickFX;
    [SerializeField] private AudioSource ClaimRewardFX;
    [SerializeField] private GameObject backgroundSound;
    [SerializeField] private GameObject hiveButtonGO;
    private UnityEngine.UI.Image musicIconImage;
    private Button[] buttons;

    private void Awake()
    {
       if (instance == null)
        {
            instance = this;
        } 
        hiveButtonGO.GetComponent<Button>().onClick.AddListener(this.SoundClickButton);
        musicIconImage = hiveButtonGO.GetComponent<UnityEngine.UI.Image>();
        // Put all audio sources into the array.
        AudioSource[] background = backgroundSound.GetComponents<AudioSource>();
        hiveSceneAudio = new AudioSource[background.Length + 2];
        for (int i = 0; i < background.Length; i++)
        {
            hiveSceneAudio[i] = background[i];
        }
        if (buttonClickFX != null)
        {
            hiveSceneAudio[background.Length] = buttonClickFX;
        }
        if (ClaimRewardFX != null)
        {
            hiveSceneAudio[background.Length + 1] = ClaimRewardFX;
        }

        // Add click noise to each button.
        buttons = GameObject.FindObjectsOfType<Button>(true);
        //remove claim reward buttons
        buttons = Array.FindAll(buttons, element => element.gameObject.name != "Claim Reward Button");
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(HiveSoundManager.PlayButtonClickFX);
            Debug.Log("RSRSRS button: " + button.gameObject.name);
        }

        // Add click sound function to each tile.
        Tile[] tiles = GameObject.FindObjectsOfType<Tile>(true);
        foreach (Tile tile in tiles)
        {
            tile.playSoundOnClick = HiveSoundManager.PlayButtonClickFX;
        }
        
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

    /// <summary>
    /// Play the button click sound.
    /// </summary>
    public static void PlayButtonClickFX()
    {
        if (instance.buttonClickFX.enabled)
        {
            instance.buttonClickFX.Play();
        }
    }

    /// <summary>
    /// Play the claim reward click sound.
    /// </summary>
    public static void PlayClaimRewardButtonClickFX()
    {
        if (instance.ClaimRewardFX != null && instance.ClaimRewardFX.enabled)
        {
            instance.ClaimRewardFX.Play();
        }
    }

    private void OnDestroy()
    {
        hiveButtonGO.GetComponent<Button>().onClick.RemoveListener(this.SoundClickButton);
        foreach (Button button in buttons)
        {
            button.onClick.RemoveListener(HiveSoundManager.PlayButtonClickFX);
        }
    }
}