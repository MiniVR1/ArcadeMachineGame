using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HammerMovement : MonoBehaviour
{
    Vector3 returnPos;
    Quaternion returnRot;
    public float moveSpeed = 5f;
    public float distance = 3f;
    public Rigidbody rb;

    public List<GameObject> pivots;
    public LayerMask raycastMask;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        returnPos = rb.position;
        returnRot = rb.rotation;
    }

    // Update is called once per frame
    void Update()
    {       
        // Do different logic depending if we are returning the hammer or picking it up
        Vector3 worldPos;
        if (Mouse.current.leftButton.IsPressed())
        {
            // convert the mouse position to a world position
            Vector3 mousePos = Mouse.current.position.ReadValue();
            
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit, 100, raycastMask))
                worldPos = hit.point;
            else
                worldPos = returnPos;


            // find out which pivot is closest
            float closestDist = 1000f;
            GameObject bestPivot = pivots[0];
            foreach (GameObject pivot in pivots)
            {
                if ((rb.position - pivot.transform.position).magnitude < closestDist)
                {
                    bestPivot = pivot;
                    closestDist = (worldPos - pivot.transform.position).magnitude;
                }
            }

            // rotate to face away from the pivot point
            float rot = Vector2.SignedAngle(Vector2.up, worldPos - bestPivot.transform.position);
            rb.MoveRotation(Quaternion.Euler(0, 0, rot));
        }
        else
        {
            worldPos = returnPos;
            // rotate to face upwards
            float rot = returnRot.eulerAngles.z;
            rb.MoveRotation(Quaternion.Euler(0, 0, rot));
        }
        rb.linearVelocity = (worldPos - transform.position) * moveSpeed;
    }
}
