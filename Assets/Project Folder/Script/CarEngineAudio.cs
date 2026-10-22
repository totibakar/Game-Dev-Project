using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class CarEngineAudio : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Engine Sounds")]
    public AudioClip idleSound;
    public AudioClip shutoffSound;

    [Header("Timing")]
    [Tooltip("How many seconds the car idles before shutting off")]
    public float timeUntilShutoff = 5.0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        // Ensure 3D settings apply correctly
        audioSource.playOnAwake = false; 

        // Start the engine idle loop
        audioSource.clip = idleSound;
        audioSource.loop = true;
        audioSource.Play();

        // Start the countdown timer
        StartCoroutine(ShutoffSequence());
    }

    IEnumerator ShutoffSequence()
    {
        // Wait for the delay
        yield return new WaitForSeconds(timeUntilShutoff);

        // Stop the idle sound and disable looping
        audioSource.Stop();
        audioSource.loop = false;

        // Play the shutoff sound once
        audioSource.clip = shutoffSound;
        audioSource.Play();
    }
}