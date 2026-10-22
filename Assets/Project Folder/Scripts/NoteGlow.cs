using UnityEngine;

public class NoteGlow : MonoBehaviour
{
    [Header("Glow Settings")]
    public float highlightDistance = 15f; // Distance at which the note glows (10-20 meters)
    private Transform playerCamera;

    [Header("Visual References")]
    public Light noteGlowLight; // Drag your child point light here

    void Start()
    {
        if (Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }

        if (noteGlowLight != null)
        {
            noteGlowLight.enabled = false; // Start with the glow turned off
        }
    }

    void Update()
    {
        if (playerCamera == null) return;

        // Check distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, playerCamera.position);

        if (distanceToPlayer <= highlightDistance)
        {
            if (noteGlowLight != null) noteGlowLight.enabled = true;
        }
        else
        {
            if (noteGlowLight != null) noteGlowLight.enabled = false;
        }
    }
}