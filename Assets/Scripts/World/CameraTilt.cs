using UnityEngine;

public class CameraTilt : MonoBehaviour
{
    [SerializeField] private float tiltSpeed = 4f;
    [SerializeField] const float tilt_ang = 30.0f;
    const float EPS = 1e-3f;

    private float tilt = 0.0f;
    private Quaternion from;
    private Quaternion to;
    private bool rotating = false;

    private float lerp = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        from = transform.rotation;
        to = from;
        HammerHit.instance.hammerHitLeft += HandleLeftHit;
        HammerHit.instance.hammerHitRight += HandleRightHit;
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

    void HandleLeftHit(float hitStrength)
    {
        TiltRight();
    }

    void HandleRightHit(float hitStrength)
    {
        TiltLeft();
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
        tilt = Mathf.Clamp(tilt - tilt_ang, -tilt_ang, tilt_ang);
        StartRotation();
    }

    public void TiltRight()
    {
        tilt = Mathf.Clamp(tilt + tilt_ang, -tilt_ang, tilt_ang);
        StartRotation();
    }
}
