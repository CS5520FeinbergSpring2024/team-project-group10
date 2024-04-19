using UnityEngine;

public static class AudioUtility
{
    // sprites for audion on/off button 
    private static readonly Sprite musicIcon = Resources.Load<Sprite>("Sound/speaker");
    private static readonly Sprite muteMusicIcon = Resources.Load<Sprite>("Sound/muteSpeaker");

    /// <summary>
    ///  On Scene start plays or mutes scene audio.
    /// </summary>
    /// <param name="audioSources"> Array of all AudioSources in scene </param>
    /// <param name="audioButtonImage"> Sound Button Image for scene </param>
    public static void OnSceneAudioStart(AudioSource[] audioSources, UnityEngine.UI.Image audioButtonImage)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (SoundOn.soundOn && audioSource != null)
            {
                audioSource.enabled = true;
                audioButtonImage.sprite = musicIcon;
            } 
            else if (!SoundOn.soundOn && audioSource != null)
            {
                audioSource.enabled = false;
                audioButtonImage.sprite = muteMusicIcon;
            }
            else
            {
                Debug.Log("null AudioSource");
            }
        }
    }

    /// <summary>
    ///  On sound button click turns all AudioSource sounds in scene on or off.
    /// </summary>
    /// <param name="audioSources"> Array of all AudioSources in scene </param>
    /// <param name="audioButtonImage"> Sound Button Image for scene </param>
   public static void AudioButtonClick(AudioSource[] audioSources, UnityEngine.UI.Image audioButtonImage)
   {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource != null)
            {   
                audioSource.enabled = !audioSource.enabled;
                SoundOn.soundOn = audioSource.enabled;
            }
        }
        if (SoundOn.soundOn)
        {
            audioButtonImage.sprite = musicIcon;
        }
        else
        {
            audioButtonImage.sprite = muteMusicIcon;
        }
   }
}

public class SoundOn
{
    public static bool soundOn = true;
}