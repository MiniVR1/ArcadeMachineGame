using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public TiltAmount tilt;
    public static event Action<TiltAmount, BoxCollider2D> collide;

    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            collide?.Invoke(tilt, boxCollider);
        }
    }
}
