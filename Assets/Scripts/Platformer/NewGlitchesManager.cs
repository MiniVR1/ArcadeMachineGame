using UnityEngine;
using System.Collections.Generic;

public class NewGlitchesManager : MonoBehaviour
{
    List<NewGlitches> glitches1 = new List<NewGlitches>();
    List<NewGlitches> glitches2 = new List<NewGlitches>();

    void Start()
    {
        NewGlitches[] glitches = GetComponents<NewGlitches>();

        for (int i = 0; i < glitches.Length; i += 2)
        {
            glitches1.Add(glitches[i]);
        }
        for (int i = 1; i < glitches.Length; i += 2)
        {
            glitches2.Add(glitches[i]);
        }
        foreach (var glitch in glitches1)
        {
            glitch.ToggleGlitch();
        }
    }

    void OnEnable()
    {
        BaseballHit.Instance.hitTop += ToggleGlitch;
    }

    void OnDisable()
    {
        BaseballHit.Instance.hitTop -= ToggleGlitch;
    }

    void ToggleGlitch()
    {
        foreach (var glitch in glitches1)
        {
            glitch.ToggleGlitch();
        }
        foreach (var glitch in glitches2)
        {
            glitch.ToggleGlitch();
        }
    }
}
