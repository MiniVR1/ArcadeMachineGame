using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    public Texture2D grabCursor;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit[] hits = Physics.RaycastAll(ray, 10);
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
                if (interactable != null)
                {
                    Cursor.SetCursor(grabCursor, Vector2.zero, CursorMode.Auto);

                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        Debug.Log("Interacting with: " + interactable.gameObject.name);
                        interactable.OnInteract();
                    }
                    return; // only find the first interactable thing and skip resetting the cursor
                }
            }
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}
