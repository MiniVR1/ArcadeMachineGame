using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private BoxCollider2D senseCollider;
    [SerializeField] private BoxCollider2D collider;
    public TiltAmount tilt;
    public static event Action<TiltAmount, BoxCollider2D> collide;

    private bool touching = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !touching)
        {
            touching = true;
            collide?.Invoke(tilt, collider);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            touching = false;
        }
    }
}
