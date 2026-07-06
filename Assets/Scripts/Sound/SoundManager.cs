using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [Header("[Sound Players]")]
    public AudioSource musicPlayer;
    public AudioSource[] sfxPlayer;
    [Header("[Music List]")]
    public AudioClip levelMusic;
    [Header("[SFX]")]
    public AudioClip sfxName1;

    void Awake()
    {
        Instance = this;
    }
    public void playMusic(AudioClip musicToBePlayed)
    {
        musicPlayer.clip = musicToBePlayed;
        musicPlayer.Play();
    }

    //multiple audioClips to prevent case of overlap. Unity sometimes does that ):
    public void playSFX(AudioClip audioToPlay)
    {
        for (int i = 0; i < sfxPlayer.Length; i++)
        {
            if (!sfxPlayer[i].isPlaying)
            {
                sfxPlayer[i].PlayOneShot(audioToPlay);
                return;
            }
        }
    }
    
    
}

