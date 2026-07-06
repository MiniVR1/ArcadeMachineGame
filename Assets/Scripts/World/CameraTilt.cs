using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTilt : MonoBehaviour
{
    const float EPS = 1e-3f;
    const float TILT_ANG = 30.0f;

    private float tilt = 0.0f;
    [SerializeField] private float tiltSpeed = 4f;
    private Quaternion from;
    private Quaternion to;
    private bool rotating = false;

    private float lerp = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        from = transform.rotation;
        to = from;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotating)
        {
            lerp += Time.deltaTime * tiltSpeed;
            transform.rotation = Quaternion.Lerp(from, to, lerp);
            if (lerp >= 1.0f)
            {
                transform.rotation = to;
                rotating = false;
            }
        }
    }

    public void OnWASD(InputValue value)
    {
        if (rotating) return;

        float horizontal = value.Get<float>();

        if (horizontal < 0)
        {
            TiltLeft();
        }
        else if (horizontal > 0)
        {
            TiltRight();
        }
    }

    private void StartRotation()
    {
        from = transform.rotation;
        to = Quaternion.Euler(0, 0, tilt);

        if (Quaternion.Angle(from, to) > EPS)
        {
            lerp = 0.0f;
            rotating = true;
        }
    }

    public void TiltLeft()
    {
        tilt = Mathf.Clamp(tilt - TILT_ANG, -TILT_ANG, TILT_ANG);
        StartRotation();
    }

    public void TiltRight()
    {
        tilt = Mathf.Clamp(tilt + TILT_ANG, -TILT_ANG, TILT_ANG);
        StartRotation();
    }
}
