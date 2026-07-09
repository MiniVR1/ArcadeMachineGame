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

    void Start()
    {
        l1StartLoc = level1Start.transform.position;
        l2StartLoc = level2Start.transform.position;
        l3StartLoc = level3Start.transform.position;
    }

    public void ChangeLevel(CurrentLevel nextLevel)
    {
        level = nextLevel;

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

public enum CurrentLevel
{
    Menu,
    Level1,
    Level2,
    Level3
}
