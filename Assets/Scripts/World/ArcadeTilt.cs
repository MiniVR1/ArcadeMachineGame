using UnityEngine;

public class ArcadeTilt : MonoBehaviour
{
    [SerializeField] private float tiltSpeed = 20f;
    [SerializeField] const float tilt_ang = 30.0f;
    [SerializeField] private float hitCooldown = 0.15f;
    [SerializeField] private ArcadePlayer player;

    private float cooldownTimer = 0f;
    const float EPS = 1e-3f;

    private float tilt = 0.0f;
    private Quaternion from;
    private Quaternion to;
    private bool rotating = false;
    private Quaternion originalRotation;

    private float lerp = 0.0f;
    private Collider[] colliders;

    void Awake()
    {
        player = FindAnyObjectByType<ArcadePlayer>();
    }

    void Start()
    {
        originalRotation = transform.rotation;
        from = transform.rotation;
        to = from;
        colliders = GetComponentsInChildren<Collider>();
    }

    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

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

    private void OnEnable()
    {
        HammerHit.Instance.hammerHitLeft += TiltRight;
        HammerHit.Instance.hammerHitRight += TiltLeft;
    }

    private void OnDisable()
    {
        if (HammerHit.Instance != null)
        {
            HammerHit.Instance.hammerHitLeft -= TiltRight;
            HammerHit.Instance.hammerHitRight -= TiltLeft;
        }
    }

    private bool IsHammerTouching()
    {
        Sledgehammer hammer = Object.FindAnyObjectByType<Sledgehammer>();
        if (hammer == null)
        {
            return false;
        }

        Collider[] hammerColliders = hammer.GetComponentsInChildren<Collider>();

        foreach (var arcadeCollider in colliders)
        {
            foreach (var hammerCollider in hammerColliders)
            {
                if (arcadeCollider.bounds.Intersects(hammerCollider.bounds))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void StartRotation()
    {
        from = transform.rotation;
        to = originalRotation * Quaternion.Euler(tilt, 0, 0);

        if (Quaternion.Angle(from, to) > EPS)
        {
            lerp = 0.0f;
            rotating = true;
            cooldownTimer = hitCooldown;
        }
    }

    private void TiltLeft(float hitStrength)
    {
        if (rotating || (cooldownTimer > 0f && IsHammerTouching()))
        {
            return;
        }

        tilt = Mathf.Clamp(tilt - tilt_ang, -tilt_ang, tilt_ang);
        StartRotation();
        if (Mathf.Approximately(tilt, 0f))
        {
            player.DefaultTilt();
        }
        else
        {
            player.LeftTilt();
        }
    }

    private void TiltRight(float hitStrength)
    {
        if (rotating || (cooldownTimer > 0f && IsHammerTouching()))
        {
            return;
        }

        tilt = Mathf.Clamp(tilt + tilt_ang, -tilt_ang, tilt_ang);
        StartRotation();
        if (Mathf.Approximately(tilt, 0f))
        {
            player.DefaultTilt();
        }
        else
        {
            player.RightTilt();
        }
    }
}
