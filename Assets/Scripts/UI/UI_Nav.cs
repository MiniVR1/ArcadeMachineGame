using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Nav : MonoBehaviour
{
    [Header("UI Menus")]
    public GameObject startMenu;
    public GameObject selectLevelMenu;
    public GameObject optionsMenu;
    public GameObject escapeMenu;
    public GameObject StartingScreen;
    public GameObject gameScreen;
    public GameObject backgroundScreen;

    [Header("Setup")]
    public Selectable startButton;
    public Selectable levelSelectButton;
    public Selectable optionsButton;
    public Selectable escapeMenuButton;

    [Header("UI Nav Variables")]
    [SerializeField] private EventSystem eventSystem;
    public bool isInStartMenu = true;
    public ArcadePlayer gamePlayer;
    public LevelManager levelManager;
    public Door doorlevel;

    public void StartGame()
    {
        SoundManager.instance.PlayUISound(SoundManager.instance.confirmSelectSfx);
        startMenu.SetActive(false);
        doorlevel.level = CurrentLevel.Level1;
        levelManager.StartGameLevel();
        backgroundScreen.SetActive(false);
        gameScreen.SetActive(true);
        gamePlayer.isPaused = false;
        isInStartMenu = false;
    }

    public void OpenLevelSelect()
    {
        startMenu.SetActive(false);
        selectLevelMenu.SetActive(true);
        JumpToElement(levelSelectButton);
    }

    public void CloseLevelSelect()
    {
        selectLevelMenu.SetActive(false);
        startMenu.SetActive(true);
        JumpToElement(startButton);
    }

    public void OpenOptions()
    {
        SoundManager.instance.PlayUISound(SoundManager.instance.confirmSelectSfx);
        if (isInStartMenu)
        {
            startMenu.SetActive(false);
            optionsMenu.SetActive(true);
            JumpToElement(optionsButton);
        }
        else
        {
            escapeMenu.SetActive(false);
            optionsMenu.SetActive(true);
            JumpToElement(optionsButton);
        }
    }

    public void CloseOptions()
    {
        if (isInStartMenu)
        {
            optionsMenu.SetActive(false);
            startMenu.SetActive(true);
            JumpToElement(startButton);
        }
        else
        {
            optionsMenu.SetActive(false);
            escapeMenu.SetActive(true);
            JumpToElement(escapeMenuButton);
        }
    }

    public void OpenEscapeMenu()
    {
        escapeMenu.SetActive(true);
        JumpToElement(escapeMenuButton);
    }

    public void CloseEscapeMenu()
    {
        SoundManager.instance.PlayUISound(SoundManager.instance.confirmSelectSfx);
        escapeMenu.SetActive(false);
        gamePlayer.isPaused = false;
        eventSystem.SetSelectedGameObject(null);
    }

    public void OpenStartMenu()
    {
        SoundManager.instance.PlayUISound(SoundManager.instance.confirmSelectSfx);
        SoundManager.instance.Level1MusicSource.SetActive(false);
        SoundManager.instance.Level2MusicSource.SetActive(false);
        SoundManager.instance.Level3MusicSource.SetActive(false);
        SoundManager.instance.MainMenuMusiceSource.SetActive(true);
        isInStartMenu = true;
        escapeMenu.SetActive(false);
        gameScreen.SetActive(false);
        backgroundScreen.SetActive(true);
        startMenu.SetActive(true);
        JumpToElement(startButton);
    }

    public void StopAnimation()
    {
        StartingScreen.SetActive(false);
        SoundManager.instance.MainMenuMusiceSource.SetActive(true);
        JumpToElement(startButton);
    }

    public void JumpToElement(Selectable selectedObject)
    {
        if (eventSystem == null)
            Debug.Log("This item has no event system referenced yet.", this);

        if (selectedObject == null)
            Debug.Log("This should jump where?", this);

        eventSystem.SetSelectedGameObject(selectedObject.gameObject);
    }
}
