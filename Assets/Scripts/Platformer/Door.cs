using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private List<KeyType> requiredKeys = new();
    public IReadOnlyList<KeyType> RequiredKeys => requiredKeys;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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

            Destroy(gameObject);
            Debug.Log("Door opened");
        }
    }
}
