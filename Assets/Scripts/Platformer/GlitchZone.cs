using UnityEngine;

public class GlitchZone : MonoBehaviour
{
    public int zone;
    public Glitches[] GlitchList;

    Glitches script;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // public void Test(int i)
    // {
    //     Debug.Log("Test Success this is zone " + i);
    // }

    public void GlitchEnable()
    {
        GlitchList = gameObject.GetComponentsInChildren<Glitches>();
        foreach (Glitches child in GlitchList)
        {
            script = (Glitches)child.GetComponent(typeof(Glitches));
            script.EnableGlitch();
        }
    }

    public void GlitchDisable()
    {
        GlitchList = gameObject.GetComponentsInChildren<Glitches>();
        foreach (Glitches child in GlitchList)
        {
            script = (Glitches)child.GetComponent(typeof(Glitches));
            script.DisableGlitch();
        }
    }
}
