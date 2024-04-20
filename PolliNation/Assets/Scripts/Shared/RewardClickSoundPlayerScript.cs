using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardClickSoundPlayerScript : MonoBehaviour
{
    [SerializeField] private AudioSource _claimRewardFX;
    private static RewardClickSoundPlayerScript _instance;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        _instance = this;
        DontDestroyOnLoad(this);
    }
    
    /// <summary>
    /// Play the button click sound.
    /// </summary>
    public static void PlayClaimRewardFX()
    {
        Debug.Log("DEBUG in playRewardSound");
        if (_instance._claimRewardFX.enabled)
        {
            Debug.Log("DEBUG playing sound");
            _instance._claimRewardFX.Play();
        }
    }
}
