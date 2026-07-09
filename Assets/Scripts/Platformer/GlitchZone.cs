using UnityEngine;
using System.Collections.Generic;

public class GlitchZone : MonoBehaviour
{
    public int zone;

    List<Glitches> glitches = new List<Glitches>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        glitches.AddRange(gameObject.GetComponentsInChildren<Glitches>());
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
        foreach (Glitches child in glitches)
        {
            child.EnableGlitch(zone);
        }
    }

    public void GlitchDisable()
    {
        foreach (Glitches child in glitches)
        {
            child.DisableGlitch();
        }
    }
}
