using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public cursorType cursorType;
    
    public abstract void OnInteract();
}
