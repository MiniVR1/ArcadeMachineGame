using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonStateChecker : MonoBehaviour, ISelectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("selected");
    }
}
