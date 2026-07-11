using UnityEngine;
using UnityEngine.Playables;

public class OutOfOrderFlip : InteractableObject
{
    public Animator animator;
    public PlayableDirector director;

    public override void OnInteract()
    {
        animator.Play("Flip");
        SoundManager.instance.PlayUISound(SoundManager.instance.paperSfx);
        director.enabled = true;
    }
}
