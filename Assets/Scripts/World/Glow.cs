using UnityEngine;

public class ButtonGlow : MonoBehaviour
{
    [ColorUsage(true, true)] // Enables HDR color picker for glow intensity
    [SerializeField] private Color glowColor = Color.cyan;
    [SerializeField] private float blinkSpeed = 4f;
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 3.0f;

    private Material targetMaterial;
    private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");

    void Start()
    {
        // Get the material from the renderer
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            // Using .material creates a local instance copy so it doesn't affect other buttons
            targetMaterial = renderer.material;
            targetMaterial.EnableKeyword("_EMISSION");
        }
        else
        {
            Debug.LogError("ButtonGlow requires a Renderer component on the GameObject!");
            enabled = false;
        }
    }

    void Update()
    {
        float wave = (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f;
        float currentIntensity = Mathf.Lerp(minIntensity, maxIntensity, wave);
        Color finalColor = glowColor * currentIntensity;
        targetMaterial.SetColor(EmissionColorID, finalColor);
    }

    void OnDestroy()
    {
        if (targetMaterial != null)
        {
            Destroy(targetMaterial);
        }
    }
}
