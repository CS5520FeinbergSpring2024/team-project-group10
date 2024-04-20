using System.Collections;
using Unity.VisualScripting;
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

    /// <summary>
    ///  Fades out audioSource over given duration to 0.
    /// </summary>
    /// <param name="audioSource"> audioSource to fade out </param>
    /// <param name="duration"> duration to fade out audio from starting volume to 0 </param>
   public static IEnumerator AudioFadeOut(AudioSource audioSource, float duration)
   {
    float startVolume = audioSource.volume;
    float timeElapsed = 0f;
    audioSource.Play();
    while (audioSource.volume > 0)
    {
        audioSource.volume = startVolume * (1 - timeElapsed / duration);
        timeElapsed += Time.deltaTime;
        yield return null;
    }
    audioSource.Stop();
    audioSource.volume = startVolume;
    yield return null;
   }

    /// <summary>
    ///  Fades in audioSource over given duration.
    /// </summary>
    /// <param name="audioSource"> audioSource to fade in </param>
    /// <param name="duration"> duration to fade out audio from given volume to target original volume </param>
    public static IEnumerator AudioFadeIn(AudioSource audioSource, float startingVolume, float duration)
   {
    float targetVolume = audioSource.volume;
    audioSource.volume = startingVolume;
    float timeElapsed = 0f;
    audioSource.Play();
    while (audioSource.volume < targetVolume)
    {
        audioSource.volume = targetVolume * ( timeElapsed / duration);
        timeElapsed += Time.deltaTime;
        yield return null;
    }
    yield return null;
   }
}

public class SoundOn
{
    public static bool soundOn = true;
}