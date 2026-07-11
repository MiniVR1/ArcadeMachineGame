using UnityEngine;

public class NewGlitches : MonoBehaviour
{
    public bool glitchEnabled = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // BaseballHit.Instance.hitTop += ToggleGlitch;
        glitchEnabled = !glitchEnabled; // swap modes so that initialisation sets it to the correct mode
        ToggleGlitch(); // initialize the glitch in case the glitch starts disabled
    }

    public void ToggleGlitch()
    {
        glitchEnabled = !glitchEnabled;
        gameObject.SetActive(glitchEnabled);
    }
}
