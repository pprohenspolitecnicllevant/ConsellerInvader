using UnityEngine;

public class ChangeEmissionIntensity : MonoBehaviour
{
    [SerializeField] private Material material; // The material whose emission map intensity you want to change
    [SerializeField] private Color emissionColor; // The desired emission color
    [SerializeField] private float maxIntens; // Maximum quantity of emisison
    [SerializeField] private float minIntens; // Minimum quantity of emisison
    [SerializeField] private float speed; // Blinking velocity

    [SerializeField] private GlowElement glowElement;

    private enum GlowElement
    {
        cover,
        terrain,
        glass
    }

    private void Start()
    {
        switch (glowElement)
        {
            default:
            case GlowElement.cover:
                minIntens = StatsManager.tower.cover.minIntensGlow;
                maxIntens = StatsManager.tower.cover.minIntensGlow;
                speed = StatsManager.tower.cover.glowSpeed;
                break;
            case GlowElement.terrain:
                minIntens = StatsManager.terrain.minIntensGlow;
                maxIntens = StatsManager.terrain.maxIntensGlow;
                speed = StatsManager.terrain.glowSpeed;
                break;
            case GlowElement.glass:
                //minIntens = StatsManager.terrain.minIntensGlow;
                //maxIntens = StatsManager.terrain.maxIntensGlow;
                //speed = StatsManager.terrain.glowSpeed;
                break;
        }
        
    }

    private void Update() {
        float intensity = minIntens + (Mathf.Abs(Mathf.Sin(Time.time * speed ) * maxIntens)); // Calculate a varying intensity value

        // Set the emission color with the desired intensity
        material.SetColor("_EmissionColor", emissionColor * intensity);
        
        // Update the material's properties to show the changes
        material.EnableKeyword("_EMISSION");
        material.globalIlluminationFlags &= ~MaterialGlobalIlluminationFlags.EmissiveIsBlack;
    }
}