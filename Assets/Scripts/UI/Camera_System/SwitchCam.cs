using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class SwitchCam : MonoBehaviour
{
    public CameraPositions[] cameraPositions;
    private Vector3 cameraStartPos;
    private Vector3 cameraRotation;

    public Canvas canvas;

    public void SwitchToCamera(int cameraPos)
    {
        StopAllCoroutines();
        StartCoroutine(ZoomToPosition(cameraPositions[cameraPos].position, cameraPositions[cameraPos].rotation));
        canvas.enabled = false;
    }

    private IEnumerator ZoomToPosition(Vector3 position, Vector3 rotation)
    {
        float time = 0;
        cameraStartPos = Camera.main.transform.localPosition;
        cameraRotation = Camera.main.transform.localRotation.eulerAngles;
        while (time < 1)
        {
            Camera.main.transform.localPosition = Vector3.Slerp(cameraStartPos, position, time);
            Camera.main.transform.localRotation = Quaternion.Euler(Vector3.Slerp(cameraRotation, rotation, time));
            time += Time.deltaTime;
            yield return null;
        }
    }


    [Serializable]
    public class CameraPositions
    {
        public Vector3 position;
        public Vector3 rotation;
        public CameraPositions(Vector3 position, Vector3 rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
}
