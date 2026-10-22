using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BasinDrip : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Drop Timing (Seconds)")]
    public float minDelay = 2.0f;
    public float maxDelay = 4.5f;

    void Start()
    {
        // Grab the Audio Source attached to this object
        audioSource = GetComponent<AudioSource>();
        
        // Start the infinite looping timer
        StartCoroutine(DropTimer());
    }

    IEnumerator DropTimer()
    {
        while (true)
        {
            // Pick a random wait time between your min and max
            float waitTime = Random.Range(minDelay, maxDelay);
            
            // Wait for that amount of time
            yield return new WaitForSeconds(waitTime);

            // Slightly randomize the pitch so every drop sounds a little different
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            
            // Play the sound once
            audioSource.Play();
        }
    }
}