using System.Collections;
using UnityEngine;

public class ZoomIn : InteractableObject
{
    private bool zoomedIn = true;
    public Vector3 zoomedOutPos;
    public Vector3 zoomedInPos;
    private Vector3 cameraStartPos;
    private Coroutine coroutine;

    public override void OnInteract()
    {
        zoomedIn = !zoomedIn;
        if (zoomedIn)
            cursorType = cursorType.zoomOut;
        else 
            cursorType = cursorType.zoomIn;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ZoomToPosition());
    }

    private IEnumerator ZoomToPosition()
    {
        float time = 0;
        cameraStartPos = Camera.main.transform.position;
        while (time < 1)
        {
            if (zoomedIn)
            {
                Camera.main.transform.position = Vector3.Slerp(cameraStartPos, zoomedInPos, time);
                time += Time.deltaTime;
            }
            else
            {
                Camera.main.transform.position = Vector3.Slerp(cameraStartPos, zoomedOutPos, time);
                time += Time.deltaTime;
            }
            yield return null;
        }
    }
}
