using System.Collections;
using UnityEngine;

public class HitBroadcaster : MonoBehaviour
{
    public float lockedTime = 0.2f;
    private bool itemLocked = false;

    public delegate void HasHit();


    // Create 3 lists of delegates so that other scripts can listen to when a item swing is detected.
    // E.g. to make a public function called "TiltMachine(float hitStrength)" be called whenever the item is swung, run "HammerHit.instance.hitLeft += TiltMachine;" on startup
    public HasHit hitLeft;
    public HasHit hitRight;
    public HasHit hitTop;
    public HasHit hitConsole;
    public HasHit hitJumpButton;

    private void OnTriggerEnter(Collider other)
    {
        if (!itemLocked)
            // Debug.Log(other.gameObject.name);
            switch (other.gameObject.name) // call the right delegate based on which hitbox was collided with
            {
                case "LeftSide":
                    {
                        StartCoroutine(CallItemHit(hitLeft));
                        break;
                    }
                case "RightSide":
                    {
                        StartCoroutine(CallItemHit(hitRight));
                        break;
                    }
                case "Top":
                    {
                        StartCoroutine(CallItemHit(hitTop));
                        break;
                    }
                case "MainConsole":
                    {
                        StartCoroutine(CallItemHit(hitConsole));
                        break;
                    }
                case "JumpButton":
                    {
                        StartCoroutine(CallItemHit(hitJumpButton));
                        break;
                    }
            }
    }

    private IEnumerator CallItemHit(HasHit list)
    {
        itemLocked = true;
        if (list != null)
            list();
        // then leave the item locked until the lockedTime expires
        yield return new WaitForSeconds(lockedTime);
        itemLocked = false;
    }

}
