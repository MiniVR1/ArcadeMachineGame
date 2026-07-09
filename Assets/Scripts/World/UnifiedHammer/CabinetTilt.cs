using System.Collections;
using UnityEngine;

public class CabinetTilt : MonoBehaviour
{
    private TiltAmount tiltState = TiltAmount.none;

    public float tiltLeft;
    public float tiltRight;

    public float animationTime = 0.05f;

    private void OnEnable()
    {
        HammerHit.Instance.hitLeft += HammerCabinetLeft;
        HammerHit.Instance.hitRight += HammerCabinetRight;
    }

    public void HammerCabinetLeft(float strength)
    {
        if (tiltState == TiltAmount.left)
        {
            DefaultTilt();
        }
        else if (tiltState == TiltAmount.none)
        {
            RightTilt();
        }
    }

    public void HammerCabinetRight(float strength)
    {
        if (tiltState == TiltAmount.right)
        {
            DefaultTilt();
        }
        else if (tiltState == TiltAmount.none)
        {
            LeftTilt();
        }
    }

    private void LeftTilt()
    {
        StopAllCoroutines(); // cancel any previous animations
        StartCoroutine(TiltToAngle(tiltLeft)); // now smoothly lerp to the required angle over time
        tiltState = TiltAmount.left; // and record the state for determining future changes
    }

    private void RightTilt()
    {
        StopAllCoroutines(); // cancel any previous animations
        StartCoroutine(TiltToAngle(tiltRight)); // now smoothly lerp to the required angle over time
        tiltState = TiltAmount.right; // and record the state for determining future changes
    }
    private void DefaultTilt()
    {
        StopAllCoroutines(); // cancel any previous animations
        StartCoroutine(TiltToAngle(0)); // now smoothly lerp to the required angle over time
        tiltState = TiltAmount.none; // and record the state for determining future changes
    }

    private IEnumerator TiltToAngle(float angle)
    {
        // record where the animation starts from
        float time = 0;
        while (time < 1)
        {
            // smoothly lerp the rotation to the desired point over time
            transform.localRotation = Quaternion.Euler(Mathf.Lerp(0, angle, time), 0, 0);
            time += Time.deltaTime / animationTime;
            yield return null;
        }
    }
}
