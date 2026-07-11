using System.Collections;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject player;
    public GameObject level1Start;
    public GameObject level2Start;
    public GameObject level3Start;

    public Vector3 l1StartLoc;
    public Vector3 l2StartLoc;
    public Vector3 l3StartLoc;
    public Vector3 menuLocation;

    public CurrentLevel level = CurrentLevel.Menu;
    private Coroutine levelTransition;

    [Header("UI Refenerces")]
    public GameObject levelTransitionScreen;
    public GameObject textObject;
    public TextMeshProUGUI textContents;
    public TextMeshProUGUI gameLevelText;


    void Start()
    {
        l1StartLoc = level1Start.transform.position;
        l2StartLoc = level2Start.transform.position;
        l3StartLoc = level3Start.transform.position;
    }

    public void ChangeLevel(CurrentLevel nextLevel)
    {
        Debug.Log("The current level is " + level);
        Debug.Log("The next level is " + nextLevel);
        level = nextLevel;

        levelTransition = StartCoroutine(LevelTransition(((int)nextLevel)));
    }

    public void StartGameLevel()
    {
        level = CurrentLevel.Level1;
        levelTransition = StartCoroutine(LevelTransition(1));
    }

    private IEnumerator LevelTransition(int levelnum)
    {
        if (UI_Manager.instance.enableUI)
        {
            SoundManager.instance.MainMenuMusiceSource.SetActive(false);
            SoundManager.instance.Level1MusicSource.SetActive(false);
            SoundManager.instance.Level2MusicSource.SetActive(false);
            SoundManager.instance.Level3MusicSource.SetActive(false);
            levelTransitionScreen.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            textContents.text = $"LEVEL {levelnum} START";
            textObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            if (level == CurrentLevel.Level1)
            {

                SoundManager.instance.Level1MusicSource.SetActive(true);
                gameLevelText.text = "LEVEL 1";
                player.transform.position = l1StartLoc;
            }
            else if (level == CurrentLevel.Level2)
            {
                SoundManager.instance.Level2MusicSource.SetActive(true);
                gameLevelText.text = "LEVEL 2";
                player.transform.position = l2StartLoc;
            }
            else if (level == CurrentLevel.Level3)
            {
                SoundManager.instance.Level3MusicSource.SetActive(true);
                gameLevelText.text = "LEVEL 3";
                player.transform.position = l3StartLoc;
            }
            else
            {
                player.transform.position = menuLocation;
            }

            textObject.SetActive(false);
            levelTransitionScreen.SetActive(false);
        }
        else
        {
            if (level == CurrentLevel.Level1)
            {
                player.transform.position = l1StartLoc;
            }
            else if (level == CurrentLevel.Level2)
            {
                player.transform.position = l2StartLoc;
            }
            else if (level == CurrentLevel.Level3)
            {
                player.transform.position = l3StartLoc;
            }
            else
            {
                player.transform.position = menuLocation;
            }
        }


    }
}

public enum CurrentLevel
{
    Menu,
    Level1,
    Level2,
    Level3
}

