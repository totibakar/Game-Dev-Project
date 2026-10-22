using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light flashlightBeam;
    public KeyCode toggleKey = KeyCode.F;

    [Header("Dynamic Fog Settings")]
    [Tooltip("Fog density when the flashlight is ON (e.g., 0.097)")]
    public float fogDensityOn = 0.097f;

    [Tooltip("Fog density when the flashlight is OFF (e.g., 0.734)")]
    public float fogDensityOff = 0.734f; // Set to your current heavy fog setting

    void Start()
    {
        if (flashlightBeam != null)
        {
            // Start with the flashlight off and heavy fog
            flashlightBeam.enabled = false;
            RenderSettings.fogDensity = fogDensityOff;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (flashlightBeam != null)
            {
                // Toggle the light
                flashlightBeam.enabled = !flashlightBeam.enabled;

                // Adjust the fog density based on the light's new state
                if (flashlightBeam.enabled)
                {
                    RenderSettings.fogDensity = fogDensityOn;
                }
                else
                {
                    RenderSettings.fogDensity = fogDensityOff;
                }
            }
        }
    }
}