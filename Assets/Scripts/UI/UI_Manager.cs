using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.Log("More than one UI_Manager instance in scene!");
    }
}
