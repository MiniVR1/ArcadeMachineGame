using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GlitchManager : MonoBehaviour
{

    List<GlitchZone> glitchZones = new List<GlitchZone>();

    [SerializeField] private int currentGlitchZone; 

    GlitchZone script;

    public InputActionReference glitchHit;

    void Start()
    {
        glitchZones.AddRange(gameObject.GetComponentsInChildren<GlitchZone>());
        foreach (GlitchZone glitches in glitchZones)
        {
            Debug.Log("I exist");
        }
    }

    private void OnEnable()
    {
        glitchHit.action.started += GlitchHit;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void cycleGlitches()
    {
        foreach (GlitchZone child in glitchZones)
        {
            script = (GlitchZone)child.GetComponent(typeof(GlitchZone));
            if (script.zone == currentGlitchZone)
            {
                script.GlitchEnable(currentGlitchZone);
            }
            else
            {
                script.GlitchDisable();
            }
        }
        if (currentGlitchZone <= glitchZones.Count-1)
        {
            currentGlitchZone++;
        }
        else
        {
            currentGlitchZone = 1;
        }
    }

    private void GlitchHit(InputAction.CallbackContext context)
    {
        Debug.Log("Glitch Hit - Current Glitch Zone: " + currentGlitchZone);
        cycleGlitches();
    }
}
