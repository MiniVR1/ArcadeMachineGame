using UnityEngine;

public class NewGlitches : MonoBehaviour
{
    public CurrentLevel level;
    public bool glitchEnabled = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // BaseballHit.Instance.hitTop += ToggleGlitch;
        // swap modes so that initialisation sets it to the correct mode
        gameObject.SetActive(glitchEnabled); // initialize the glitch in case the glitch starts disabled
    }

    public void ToggleGlitch()
    {
        if (LevelManager.Instance.level == level)
        {
            glitchEnabled = !glitchEnabled;
            gameObject.SetActive(glitchEnabled);
        }
    }
}
