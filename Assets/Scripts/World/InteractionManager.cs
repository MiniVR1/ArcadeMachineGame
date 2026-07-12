using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class InteractionManager : MonoBehaviour
{
    public Texture2D grabCursor;
    public Texture2D HitCursor;
    public Texture2D ZoomInCursor;
    public Texture2D ZoomOutCursor;

    void Update()
    {
        HandleItemPhysicsState();
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

                Cursor.SetCursor(CursorLookup(interactable.cursorType), Vector2.zero, CursorMode.ForceSoftware);
                hoveringInteractable = true;

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
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

    private void HandleItemPhysicsState()
    {
        bool holdingSomething = NewItem.current != null;
        NewItem[] allItems = FindObjectsByType<NewItem>();

        foreach (NewItem item in allItems)
        {
            if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                if (holdingSomething && item == NewItem.current)
                {
                    rb.isKinematic = false;
                }
                else
                {
                    rb.isKinematic = holdingSomething;
                }
            }
        }
    }

    private Texture2D CursorLookup(cursorType type)
    {
        switch (type)
        {
            case cursorType.grab:
                return grabCursor;
            case cursorType.hit:
                return HitCursor;
            case cursorType.zoomIn:
                return ZoomInCursor;
            case cursorType.zoomOut:
                return ZoomOutCursor;
            default: return null;
        }
    }
}

public enum cursorType
{
    grab,
    hit,
    zoomIn,
    zoomOut
}