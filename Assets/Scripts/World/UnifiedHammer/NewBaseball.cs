using UnityEngine;

public class NewBaseball : NewItem
{
    [SerializeField] private float maxBatAngle = 70f;
    [SerializeField] private float batVelocity = 500f;   // deg/s
    [SerializeField] private float recoverSpeed = 250f; // deg/s

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

        /*
          Baseball orientation is laying flat on bench (length-wise).
         */
        uprightRotation = originRotation * Quaternion.Euler(0f, 0f, -90f);
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
        if (state == State.Idle) // just for redundancy, ensure this only can be called in a reasonable state
        {
            // setup the item into a state where it can be manipulated correctly
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
