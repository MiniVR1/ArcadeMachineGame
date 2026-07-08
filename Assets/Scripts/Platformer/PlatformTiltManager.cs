using UnityEngine;
using System.Collections;

public class PlatformTiltManager : MonoBehaviour
{
    [SerializeField] private TiltAmount tiltState = TiltAmount.none;
    public GameObject Player;
    private Collider2D collider;

    private ArcadePlayer playerScript;

    [SerializeField] private Vector3 leftTilt = new Vector3(0f, 0f, -10f);
    [SerializeField] private Vector3 rightTilt = new Vector3(0f, 0f, 10f);
    [SerializeField] private Vector3 noTilt = new Vector3(0f, 0f, 0f);


    void Start()
    {
        if (tiltState == TiltAmount.left)
        {
            transform.rotation = Quaternion.Euler(leftTilt);
        }
        else if (tiltState == TiltAmount.right)
        {
            transform.rotation = Quaternion.Euler(rightTilt);
        }
        else
        {
            transform.rotation = Quaternion.Euler(noTilt);
        }

        playerScript = (ArcadePlayer)Player.GetComponent(typeof(ArcadePlayer));
        collider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (playerScript.tiltState == tiltState)
            {
                collider.enabled = true;
                Player.transform.SetParent(transform);
                Player.transform.rotation = transform.rotation;
            }
            else
            {
                collider.enabled = false;
            }
            StartCoroutine(ColliderRoutine());
        }
    }

    private void enableCollider()
    {
        collider.enabled = true;
    }

    IEnumerator ColliderRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        enableCollider();
    }
}
