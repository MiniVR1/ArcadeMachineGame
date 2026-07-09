using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;

    [Header("UI Trackers")]
    public static int uiMenuNum = 0; //0 = Reality Start Menu, 1 = MainMenu Game, 2 = LevelSelect, 3 = Options, 4 = Escape Menu, 5 = Level Transition, 6 = Game

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.Log("More than one UI_Manager instance in scene!");
    }
}
