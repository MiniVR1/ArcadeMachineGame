using UnityEngine;

public class JumpButton : InteractableObject
{
    private static BaseballHit instance;

    public static BaseballHit Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindAnyObjectByType<BaseballHit>();
            return instance;
        }
    }

    public delegate void JumpButtonPressed();

    public JumpButtonPressed jumpButtonPressed;

    public override void OnInteract()
    {
        if (jumpButtonPressed != null)
           jumpButtonPressed();
    }
}
