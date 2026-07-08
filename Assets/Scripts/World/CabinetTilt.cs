using UnityEngine;

public class CabinetTilt : MonoBehaviour
{
    private TiltAmount tiltState = TiltAmount.none;

    public float tiltLeft;
    public float tiltRight;

    private void OnEnable()
    {
        HammerHit.Instance.hammerHitLeft += HammerCabinetLeft;
        HammerHit.Instance.hammerHitRight += HammerCabinetRight;
    }

    public void HammerCabinetLeft(float strength)
    {
        // first determine how much force was applied
        if (strength > 20) // really strong swing, move it fully right no matter what
        {
            RightTilt();
        }
        else if (strength > 10) // move only by one stage
        {
            if (tiltState == TiltAmount.left)
                DefaultTilt();
            else
                RightTilt();
        }
        // if it's less than 10, don't move at all as it wasn't a powerful enough swing
    }

    public void HammerCabinetRight(float strength)
    {
        // first determine how much force was applied
        if (strength > 20) // really strong swing, move it fully left no matter what
        {
            LeftTilt();
        }
        else if (strength > 10) // move only by one stage
        {
            if (tiltState == TiltAmount.left)
                DefaultTilt();
            else
                LeftTilt();
        }
        // if it's less than 10, don't move at all as it wasn't a powerful enough swing
    }

    private void LeftTilt()
    {
        transform.localRotation = Quaternion.Euler(tiltLeft, 0, 0);
        tiltState = TiltAmount.left;
    }

    private void RightTilt()
    {
        transform.localRotation = Quaternion.Euler(tiltRight, 0, 0);
        tiltState = TiltAmount.right;
    }
    private void DefaultTilt()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        tiltState = TiltAmount.none;
    }

}
