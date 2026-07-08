using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Unity.Audio;
using UnityEngine.Audio;
using TMPro;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public static SoundManager instance;

    [Header("UI SFX")]
    public AudioClip uisfx;

    [Header("Music SFX")]
    public AudioClip musicsfx;

    [Header("Audio Sources")]
    public AudioSource SFXSoundSource;
    
    [Header("Internal Values")]
    private float masterVolume = 1f; // We store this so when we unmute it goes to the set value intended by the player
    private float sfxVolume = 1f;
    private float bgmVolume = 0.5f;
    //public bool isMuted = false;

    [Header("Audio UI")]
    public TextMeshProUGUI master_VolumeTxt;
    public TextMeshProUGUI sfx_VolumeTxt;
    public TextMeshProUGUI music_VolumeTxt;

    public Slider master_Volume;
    public Slider sfx_Volume;
    public Slider music_Volume;


    public void PlayUISound(AudioClip audioToPlay)
    {
        SFXSoundSource.PlayOneShot(audioToPlay);
    }


    // Volume Controls
    public void SetMasterVolume(float level)
    {
        masterVolume = level;
        audioMixer.SetFloat("Master", Mathf.Log10(level) * 20f);

        master_VolumeTxt.text = (Mathf.Round(level * 100)).ToString();
    }

    public void SetSFXVolume(float level)
    {
        sfxVolume = level;
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(level) * 20f);
        sfx_VolumeTxt.text = (Mathf.Round(level * 100)).ToString();
    }

    public void SetMusicVolume(float level)
    {
        bgmVolume = level;
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
        music_VolumeTxt.text = (Mathf.Round(level * 100)).ToString();
    }

    /*
    public void MuteAllSound()
    {
        if (!muVolume.isOn)
        {
            audioMixer.SetFloat("masterVolume", Mathf.Log10(masterVolume) * 20f);
            isMuted = false;
        }
        else
        {
            audioMixer.SetFloat("masterVolume", -80f);
            isMuted = true;
        }
    }
    */

    /*
    public void SetMuted(bool mute)
    {
        if (isMuted)
        {
            audioMixer.SetFloat("masterVolume", -80f);
            isMuted = true;
        }
        else
        {
            audioMixer.SetFloat("masterVolume", Mathf.Log10(masterVolume) * 20f);
            isMuted = false;
        }
    }
    */

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.Log("More than one SoundManager instance in scene!");
    }
}

