using UnityEngine;
using UnityEngine.InputSystem;

public class GlitchManager : MonoBehaviour
{
    public GlitchZone[] GlitchZones;

    [SerializeField] private int currentGlitchZone; 

    public GameObject GlitchZone1;

    GlitchZone script;

    public InputActionReference glitchHit;

    void Start()
    {
        GlitchZones = gameObject.GetComponentsInChildren<GlitchZone>();
        foreach (GlitchZone child in GlitchZones)
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
        foreach (GlitchZone child in GlitchZones)
        {
            script = (GlitchZone)child.GetComponent(typeof(GlitchZone));
            if (script.zone == currentGlitchZone)
            {
                script.GlitchEnable();
            }
            else
            {
                script.GlitchDisable();
            }
        }
        if (currentGlitchZone <= GlitchZones.Length-1)
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
