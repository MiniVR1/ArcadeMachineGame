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
    public AudioClip buttonSelectSfx;
    public AudioClip confirmSelectSfx;
    public AudioClip paperSfx;
    public AudioClip selectRealityButtonSfx;

    [Header("Audio Sources")]
    public AudioSource SFXSoundSource;
    public GameObject MainMenuMusiceSource;
    public GameObject Level1MusicSource;
    public GameObject Level2MusicSource;
    public GameObject Level3MusicSource;

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
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonSelectSfx);
    }

    public void SetSFXVolume(float level)
    {
        sfxVolume = level;
        audioMixer.SetFloat("SFX", Mathf.Log10(level) * 20f);
        sfx_VolumeTxt.text = (Mathf.Round(level * 100)).ToString();
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonSelectSfx);
    }

    public void SetMusicVolume(float level)
    {
        bgmVolume = level;
        audioMixer.SetFloat("Music", Mathf.Log10(level) * 20f);
        music_VolumeTxt.text = (Mathf.Round(level * 100)).ToString();
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonSelectSfx);
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.Log("More than one SoundManager instance in scene!");
    }

    private void Start()
    {
        audioMixer.SetFloat("Master", Mathf.Log10(0.5f) * 20f);
        audioMixer.SetFloat("SFX", Mathf.Log10(0.5f) * 20f);
        audioMixer.SetFloat("Music", Mathf.Log10(0.5f) * 20f);
    }
}

