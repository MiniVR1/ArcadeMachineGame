using UnityEngine;

public class OutOfOrderFlip : InteractableObject
{
    public Animator animator;
    public override void OnInteract()
    {
        animator.Play("Flip");
    }
}
