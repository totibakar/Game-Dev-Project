using UnityEngine;

public class FPS_Move : MonoBehaviour
{
    private CharacterController controller;

    [Header("Look Settings")]
    public float speedPutar = 3f;
    public float minimumY = -60f;
    public float maximumY = 60f;
    private float rotationY = 0f;

    [Header("Movement Settings")]
    public float speed = 5f;
    public float gravity = -9.81f;
    private Vector3 velocity;

    [Header("Footstep Settings")]
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepClips; // Drag your footstep audio clips here in the Inspector
    public float stepInterval = 0.5f;   // Time in seconds between each footstep sound
    private float stepTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Lock and hide mouse cursor inside game view
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // If you didn't assign it in the inspector, try grabbing it from the character
        if (footstepAudioSource == null)
        {
            footstepAudioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Camera.main == null) return;

        // --- 1. MOUSE LOOK ---
        float mouseX = Input.GetAxis("Mouse X") * speedPutar;
        float mouseY = Input.GetAxis("Mouse Y") * speedPutar;

        // Rotate body horizontally (Y-axis)
        transform.Rotate(0, mouseX, 0);

        // Pitch main camera vertically (X-axis)
        rotationY += mouseY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        Camera.main.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);

        // --- 2. HORIZONTAL MOVEMENT ---
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Fetch camera directional vectors directly from Camera.main
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        // Flatten Y axis so looking up or down doesn't tilt walking direction
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Calculate move direction relative strictly to Camera.main direction
        Vector3 moveDir = camForward * v + camRight * h;

        // Prevent diagonal movement speed boost (W + A)
        if (moveDir.magnitude > 1f)
        {
            moveDir.Normalize();
        }

        // --- 3. GRAVITY & GROUNDING ---
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Keeps character glued to ground and stairs
        }

        velocity.y += gravity * Time.deltaTime;

        // Apply final movement
        controller.Move((moveDir * speed + velocity) * Time.deltaTime);

        // --- 4. FOOTSTEPS ---
        if (controller.isGrounded && moveDir.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstepSound();
                stepTimer = stepInterval;
            }
        }
        else
        {
            // Reset timer so the first footstep plays immediately when moving again
            stepTimer = 0f;
        }
    }

    void PlayFootstepSound()
    {
        if (footstepAudioSource == null || footstepClips.Length == 0) return;

        // Pick a random clip from the array to make footsteps sound natural
        int randomIndex = Random.Range(0, footstepClips.Length);
        AudioClip clip = footstepClips[randomIndex];

        footstepAudioSource.PlayOneShot(clip);
    }
}