using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Nav : MonoBehaviour
{
    [Header("UI Menus")]
    public GameObject StartMenu;
    public GameObject OptionsMenu;
    public GameObject SelectLevelMenu;

    [Header("Setup")]
    [SerializeField] private EventSystem eventSystem;
    public Selectable StartButton;
    public Selectable optionsButton;
    public Selectable levelSelectButton;

    public void OpenLevelSelect()
    {
        StartMenu.SetActive(false);
        SelectLevelMenu.SetActive(true);
        JumpToElement(levelSelectButton);
    }

    public void CloseLevelSelect()
    {
        SelectLevelMenu.SetActive(false);
        StartMenu.SetActive(true);
        JumpToElement(StartButton);
    }

    public void OpenOptions()
    {
        StartMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        JumpToElement(optionsButton);
    }

    public void CloseOptions()
    {
        OptionsMenu.SetActive(false);
        StartMenu.SetActive(true);
        JumpToElement(StartButton);
    }

    public void JumpToElement(Selectable selectedObject)
    {
        if (eventSystem == null)
            Debug.Log("This item has no event system referenced yet.", this);

        if (selectedObject == null)
            Debug.Log("This should jump where?", this);

        eventSystem.SetSelectedGameObject(selectedObject.gameObject);
    }
}
