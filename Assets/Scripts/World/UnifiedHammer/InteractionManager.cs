using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class InteractionManager : MonoBehaviour
{
    public Texture2D grabCursor;

    void Update()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit[] hits = Physics.RaycastAll(ray, 10);
        var sortedHits = hits.OrderBy(h => h.distance);

        bool hoveringInteractable = false;

        foreach (RaycastHit hit in sortedHits)
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                if (NewItem.current != null && interactable.gameObject != NewItem.current.gameObject)
                {
                    interactable = NewItem.current;
                }

                Cursor.SetCursor(grabCursor, Vector2.zero, CursorMode.Auto);
                hoveringInteractable = true;

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    // Debug.Log("Interacting with: " + interactable.gameObject.name);
                    interactable.OnInteract();
                }

                break;
            }
        }

        if (!hoveringInteractable)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}
