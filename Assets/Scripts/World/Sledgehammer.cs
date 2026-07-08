using UnityEngine;
using UnityEngine.InputSystem;

public class Sledgehammer : MonoBehaviour
{
    [SerializeField] private float zDistance = 4f;
    [SerializeField] private float followSpeed = 15f;
    [SerializeField] private float swingSensitivity = 4f;
    [SerializeField] private float maxSwingAngle = 80f;
    [SerializeField] private float rotationSpeed = 20f;

    [SerializeField] private Camera camera;
    [SerializeField] private Transform grabPoint;

    private Vector3 originPosition;
    private Quaternion originRotation;
    private Quaternion uprightRotation;

    private Rigidbody rb;
    private Collider[] colliders;

    private bool grabbing = false;
    private bool pendingRelease = false;

    private Vector2 mouse = Vector2.zero;
    private Vector2 previousMouse = Vector2.zero;
    private Vector2 mouseDelta = Vector2.zero;


    void Start()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;

        // Flip hammer upright
        uprightRotation = originRotation * Quaternion.Euler(0f, 0f, 180f);

        rb = GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();

        if (camera == null)
        {
            camera = Camera.main;
        }
    }


    void FixedUpdate()
    {
        if (grabbing)
        {
            Vector3 screenPos = mouse;
            screenPos.z = zDistance;
            Vector3 world = camera.ScreenToWorldPoint(screenPos);
            // Rotate the local position of the pivot to the hammer's position
            Vector3 grabWorldOffset = rb.rotation * grabPoint.localPosition;
            Vector3 targetPosition = world - grabWorldOffset;

            // Move the Rigidbody using physics-safe velocity mapping rather than hard-setting position
            Vector3 moveVelocity = (targetPosition - rb.position) * followSpeed;
            rb.linearVelocity = moveVelocity;

            // Calculate Swing Rotation (Flipped 180 degrees baseline)
            float horizontalVelocity = rb.linearVelocity.x;

            float tiltOffset = -horizontalVelocity * swingSensitivity;
            tiltOffset = Mathf.Clamp(tiltOffset, -maxSwingAngle, maxSwingAngle);

            // This forces it to tilt along the screen's flat plane, no matter your inspector settings
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, tiltOffset) * uprightRotation;

            // Apply the Rotation via Angular Velocity
            Quaternion deltaRot = targetRotation * Quaternion.Inverse(rb.rotation);
            deltaRot.ToAngleAxis(out float angle, out Vector3 axis);

            if (angle > 180f) angle -= 360f;

            if (Mathf.Abs(angle) > 0.1f)
            {
                rb.angularVelocity = axis.normalized * (angle * Mathf.Deg2Rad * rotationSpeed);
            }
            else
            {
                rb.angularVelocity = Vector3.zero;
            }
        }

        if (pendingRelease)
        {
            pendingRelease = false;
            grabbing = false;


            rb.position = originPosition;
            rb.rotation = originRotation;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.useGravity = true;
        }
    }

    public void OnMovement(InputValue value)
    {
        previousMouse = mouse;
        mouse = value.Get<Vector2>();
        mouseDelta = mouse - previousMouse;
    }

    public void OnGrab(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        if (!grabbing)
        {
            Ray ray = camera.ScreenPointToRay(mouse);
            if (Physics.Raycast(ray, out RaycastHit hit) && Collided(hit.collider))
            {
                grabbing = true;

                rb.rotation = uprightRotation;
                rb.angularVelocity = Vector3.zero;
                rb.linearVelocity = Vector3.zero;

                rb.useGravity = false;
            }
        }
        else
        {
            pendingRelease = true;
        }
    }

    private bool Collided(Collider hitCollider)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (hitCollider == colliders[i])
            {
                return true;
            }
        }
        return false;
    }
}
