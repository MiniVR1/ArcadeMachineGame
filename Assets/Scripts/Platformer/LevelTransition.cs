using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    public GameObject player;
    public GameObject levelManager;

    public CurrentLevel level;
    int levelCount;

    LevelManager levelScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelCount = CurrentLevel.GetValues(typeof(CurrentLevel)).Length - 1;
        Debug.Log("We are on the " + levelCount + " level");
        levelScript = (LevelManager)levelManager.GetComponent(typeof(LevelManager));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (level < (CurrentLevel)levelCount)
            {
                level = level + 1;
                levelScript.ChangeLevel(level);
                Debug.Log(level);
            }
            else
            {
                Debug.Log("Reached Final Level");
            }
            
        }
    }
}
