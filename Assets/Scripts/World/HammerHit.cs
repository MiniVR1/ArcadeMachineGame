using System.Collections;
using UnityEngine;

public class HammerHit : MonoBehaviour
{
    private static HammerHit instance;
    public Rigidbody hammerRB;

    public delegate void HammerHasHit(float hitStrength);

    public static HammerHit Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindAnyObjectByType<HammerHit>();
            return instance;
        }
    }


    // Create 3 lists of delegates so that other scripts can listen to when a hammer swing is detected.
    // E.g. to make a public function called "TiltMachine(float hitStrength)" be called whenever the hammer is swung, run "HammerHit.instance.hammerHitLeft += TiltMachine;" on startup
    public HammerHasHit hammerHitLeft;
    public HammerHasHit hammerHitRight;
    public HammerHasHit hammerHitTop;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        switch (other.gameObject.name)
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
        Vector3 startPos = transform.position;
        yield return null;
        // after 1 frame find out how far the hammer has travelled
        Vector3 endPos = transform.position;
        float speed = (startPos - endPos).magnitude / Time.deltaTime;
        Debug.Log("Hit at " + speed + " units of speed");
        Debug.Log(list);
        if (list != null)
            list(speed);
    }

}
