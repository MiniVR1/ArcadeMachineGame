using System.Collections;
using UnityEngine;

public class HitBroadcaster : MonoBehaviour
{
    public float lockedTime = 0.2f;
    private bool hammerLocked = false;

    public delegate void HammerHasHit(float hitStrength);


    // Create 3 lists of delegates so that other scripts can listen to when a hammer swing is detected.
    // E.g. to make a public function called "TiltMachine(float hitStrength)" be called whenever the hammer is swung, run "HammerHit.instance.hammerHitLeft += TiltMachine;" on startup
    public HammerHasHit hammerHitLeft;
    public HammerHasHit hammerHitRight;
    public HammerHasHit hammerHitTop;

    private void OnTriggerEnter(Collider other)
    {
        if (!hammerLocked)
            // Debug.Log(other.gameObject.name);
            switch (other.gameObject.name) // call the right delegate based on which hitbox was collided with
            {
                case "LeftSide":
                    {
                        StartCoroutine(CallHammerHit(hammerHitLeft));
                        break;
                    }
                case "RightSide":
                    {
                        StartCoroutine(CallHammerHit(hammerHitRight));
                        break;
                    }
                case "Top":
                    {
                        StartCoroutine(CallHammerHit(hammerHitTop));
                        break;
                    }
            }
    }

    private IEnumerator CallHammerHit(HammerHasHit list)
    {
        hammerLocked = true;
        Vector3 startPos = transform.position;
        yield return null;
        // after 1 frame find out how far the hammer has travelled
        Vector3 endPos = transform.position;
        float speed = (startPos - endPos).magnitude / Time.deltaTime;
        Debug.Log("Hit at " + speed + " units of speed");
        Debug.Log(list);
        if (list != null)
            list(speed);
        // then leave the hammer locked until the lockedTime expires
        yield return new WaitForSeconds(lockedTime);
        hammerLocked = false;
    }

}
