using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VoulmeControl : MonoBehaviour
{
    [Header("[Audio]")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider sfx_Slider;

    [Header("[UI]")]
    public GameObject SoundPanel;
    public bool soundPageOpen;
    void Start()
    {
        if (PlayerPrefs.HasKey("Master"))
        {
            LoadVolume();
        }
        else
        {
            SetMasterVol();
            SetMusicVol();
            Set_sfx_Vol();
        }
    }
    public void SetVolume(String audioGroup, Slider sliderName)
    {
        float volume = sliderName.value;
        audioMixer.SetFloat(audioGroup, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(audioGroup, volume);
    }

    public void SetMasterVol()
    {
        SetVolume("Master", MasterSlider);
    }

    public void SetMusicVol()
    {
        SetVolume("Music", MusicSlider);
    }

    public void Set_sfx_Vol()
    {
        SetVolume("SFX", sfx_Slider);
    }

    public void LoadVolume()
    {
        MasterSlider.value = PlayerPrefs.GetFloat("Master");
        MusicSlider.value = PlayerPrefs.GetFloat("Music");
        sfx_Slider.value = PlayerPrefs.GetFloat("SFX");

        SetMasterVol();
        SetMusicVol();
        Set_sfx_Vol();
    }
    

}
