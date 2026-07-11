using UnityEngine;
using System.Collections.Generic;

public class GlitchZone : MonoBehaviour
{
    public int zone;

    List<Glitches> glitches1 = new List<Glitches>();
    List<Glitches> glitches2 = new List<Glitches>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Glitches[] glitches = gameObject.GetComponentsInChildren<Glitches>();
        for (int i = 0; i < glitches.Length; i += 2)
        {
            glitches1.Add(glitches[i]);
        }
        for (int i = 1; i < glitches.Length; i += 2)
        {
            glitches2.Add(glitches[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // public void Test(int i)
    // {
    //     Debug.Log("Test Success this is zone " + i);
    // }

    public void GlitchEnable(int zone)
    {
        foreach (Glitches child in glitches1)
        {
            child.EnableGlitch(zone);
        }
        foreach (Glitches child in glitches2)
        {
            child.DisableGlitch();
        }
    }

    public void GlitchDisable()
    {
        foreach (Glitches child in glitches2)
        {
            child.EnableGlitch(zone);
        }
        foreach (Glitches child in glitches1)
        {
            child.DisableGlitch();
        }
    }
}
