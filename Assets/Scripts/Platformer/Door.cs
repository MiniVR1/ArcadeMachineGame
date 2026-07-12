using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private List<KeyType> requiredKeys = new();
    public IReadOnlyList<KeyType> RequiredKeys => requiredKeys;

    public GameObject player;
    public GameObject levelManager;
    public GameObject endingScene;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ArcadePlayer player = other.GetComponent<ArcadePlayer>();

            foreach (KeyType key in requiredKeys)
            {
                if (!player.HasKey(key))
                {
                    Debug.Log($"Not enough keys - {requiredKeys}");
                    return;
                }
            }

            foreach (KeyType key in requiredKeys)
            {
                player.RemoveKey(key);
            }
            // Destroy(gameObject);
            if (level < (CurrentLevel)levelCount)
            {
                level = level + 1;
                levelScript.ChangeLevel(level);
                Debug.Log(level);
            }
            else
            {
                endingScene.SetActive(true);
                Debug.Log("Reached Final Level"); // ACTIVATE END ANIMATION FROM HERE!
            }

        }
    }
}
