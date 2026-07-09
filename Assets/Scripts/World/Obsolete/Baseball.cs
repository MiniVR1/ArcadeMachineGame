using UnityEngine;
using UnityEngine.InputSystem;

public class Baseball : Item
{
    [SerializeField] private float maxBatAngle = 70f;
    [SerializeField] private float batVelocity = 500f;   // deg/s
    [SerializeField] private float recoverSpeed = 250f; // deg/s

    private float swingAngle = 0f;

    protected override void Awake()
    {
        base.Awake();
        zDistance = 3f;
        swingSensitivity = 3f;
        rotationSpeed = 12f;
    }

    protected override void Start()
    {
        base.Start();

        /*
          Baseball orientation is laying flat on bench (length-wise).
         */
        uprightRotation = originRotation * Quaternion.Euler(0f, 0f, -90f);
    }

    protected override bool IsGrabbing(InputValue value)
    {
        if (!value.isPressed || InUI)
            return true;

        if (state == State.Hold)
        {
            state = State.Swing;
            return true;
        }

        return state != State.Idle;
    }

    protected override Quaternion GetTargetRotation()
    {
        switch (state)
        {
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
            case State.Hold:
                swingAngle = 0f;
                break;
        }

        return uprightRotation * Quaternion.Euler(swingAngle, 0f, 0f);
    }
}
