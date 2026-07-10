using System;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    public static event Action<GameObject> entered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            entered?.Invoke(other.gameObject);
        }
    }
}
