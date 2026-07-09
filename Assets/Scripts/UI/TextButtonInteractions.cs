using UnityEngine;
using UnityEngine.EventSystems;


public class TextButtonInteractions : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public int uiNumToActivateOn = 0; // The problem with this system is it won't properly deselect when moving to another UI scene so animation plays continuously + Activates if the mouse
    // is clicked anywhere due to the remember currently selected script.

    public void OnSelect(BaseEventData eventData)
    {
        if (UI_Manager.uiMenuNum == uiNumToActivateOn)
        {
            Debug.Log("selected");
        }
    }
    public void OnDeselect(BaseEventData eventData)
    {
        if (UI_Manager.uiMenuNum == uiNumToActivateOn)
        {
            Debug.Log("de-selected");
        }
    }
}
