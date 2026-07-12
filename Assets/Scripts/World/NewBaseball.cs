using UnityEngine;
using UnityEngine.InputSystem;

public class NewBaseball : NewItem
{
    [Header("Pivot")]
    [SerializeField] protected float maxShiftUp = 0.3f;
    [SerializeField] protected float gripLerpSpeed = 8f;

    [Header("Bat Swing")]
    [SerializeField] private float maxBatAngle = 70f;
    [SerializeField] private float batVelocity = 500f;
    [SerializeField] private float recoverSpeed = 250f;

    private Vector3 originalLocalPivot;
    private float currentGripOffset = 0f;
    private float swingAngle = 0f;

    protected override void Awake()
    {
        base.Awake();
        swingSensitivity = 3f;
        rotationSpeed = 12f;
    }

    protected override void Start()
    {
        base.Start();

        if (grabPoint != null)
        {
            originalLocalPivot = grabPoint.localPosition;
        }

        /*
          Baseball orientation is laying flat on bench (length-wise).
         */
        uprightRotation = originRotation * Quaternion.Euler(0f, 0f, -90f);
    }

    protected override void FixedUpdate()
    {
        // Only modify the pivot calculations if the bat is actively being held or swung
        if (state != State.Idle && grabPoint != null)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            float normalizedMouseY = mousePos.y / Screen.height;
            normalizedMouseY = Mathf.Clamp01(normalizedMouseY);

            float targetGripOffset = 0f;
            if (normalizedMouseY < 0.5f)
            {
                // Remap 0.0 - 0.5 into a clean 0.0 - 1.0 range for the Lerp
                float t = normalizedMouseY / 0.5f;
                // At 0.5 mouse height, offset is 0. At 0.0 mouse height (bottom), offset is maxShiftUp
                targetGripOffset = Mathf.Lerp(maxShiftUp, 0f, t);
            }

            // Smoothly slide the current grip value
            currentGripOffset = Mathf.Lerp(currentGripOffset, targetGripOffset, Time.fixedDeltaTime * gripLerpSpeed);

            // Directly modify the grabPoint's local transform position before base.FixedUpdate runs its calculations
            Vector3 dynamicLocalPivot = originalLocalPivot;
            dynamicLocalPivot.y += currentGripOffset;
            grabPoint.localPosition = dynamicLocalPivot;
        }

        // Run the normal NewItem physics loop using our newly adjusted grabPoint location
        base.FixedUpdate();
    }

    protected override Quaternion GetTargetRotation()
    {
        switch (state)
        {
            case State.Hold:
                return base.GetTargetRotation();
            case State.Swing:
                {
                    swingAngle += batVelocity * Time.fixedDeltaTime;

                    if (swingAngle >= maxBatAngle)
                    {
                        swingAngle = maxBatAngle;
                        state = State.Recover;
                    }
                    break;
                }
            case State.Recover:
                {
                    swingAngle -= recoverSpeed * Time.fixedDeltaTime;

                    if (swingAngle <= 0f)
                    {
                        swingAngle = 0f;
                        state = State.Hold;
                    }
                    break;
                }
        }

        return uprightRotation * Quaternion.Euler(swingAngle, 0f, 0f);
    }

    public override void OnInteract()
    {
        if (state == State.Idle)
        {
            current = this;
            state = State.Hold;

            rb.rotation = uprightRotation;
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;

            rb.useGravity = false;
        }
        else if (state == State.Hold)
        {
            state = State.Swing;
        }
    }
}
