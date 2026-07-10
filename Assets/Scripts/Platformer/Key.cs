using System;
using UnityEngine;

public enum KeyType
{
    Red,
    Blue,
    Green,
}

public class Key : MonoBehaviour
{
    public KeyType type;
    public static event Action<Key, GameObject> grabbed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            grabbed?.Invoke(this, other.gameObject);
            Destroy(gameObject);
        }
    }
}
