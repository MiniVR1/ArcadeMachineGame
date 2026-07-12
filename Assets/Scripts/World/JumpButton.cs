using UnityEngine;

public class JumpButton : InteractableObject
{
    [SerializeField] private AudioSource audioSource;
    private static JumpButton instance;

    public static JumpButton Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindAnyObjectByType<JumpButton>();
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
