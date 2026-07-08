using Cinemachine;
using UnityEngine;

public class SwitchCam : MonoBehaviour
{
    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;
    public GameObject Reality_Canvas;
    public SetsUiElementToSelectOnInteraction script;

    public void SwitchCam1()
    {
        Reality_Canvas.SetActive(false);
        CameraManager.SwitchCamera(cam1);
        script.JumpToElement();
    }

    public void SwitchCam2()
    {
        CameraManager.SwitchCamera(cam2);
    }
}
