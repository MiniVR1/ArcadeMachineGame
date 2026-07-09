using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothSpeed = 0.125f;
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
       if (target != null)
        {
            Vector3 desiredPosition = transform.position + offset;
            transform.position = desiredPosition;
        }
    }
}
