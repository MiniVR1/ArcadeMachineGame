using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public GameObject reality_UI;
    public GameObject game_UI;
    public GameObject outOfOrder;
    public SwitchCam switchcam;

    public bool enableUI;

    public static UI_Manager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.Log("More than one UI_Manager instance in scene!");
    }

    private void Start()
    {
        if (!enableUI)
        {
            reality_UI.SetActive(false);
            game_UI.SetActive(false);
            outOfOrder.SetActive(false);
            switchcam.SwitchToCamera(1);
        }
    }
}
