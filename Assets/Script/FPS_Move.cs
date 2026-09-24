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

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Lock and hide mouse cursor inside game view
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Camera.main == null) return;

        // --- 1. MOUSE LOOK ---
        // Rotate ONLY the Hero Body horizontally (Y-axis)
        float mouseX = Input.GetAxis("Mouse X") * speedPutar;
        transform.Rotate(0, mouseX, 0);

        // Rotate ONLY the Camera vertically (X-axis)
        rotationY += Input.GetAxis("Mouse Y") * speedPutar;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        Camera.main.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);

        // --- 2. HORIZONTAL MOVEMENT ---
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Calculate move direction relative to body rotation
        Vector3 moveDir = transform.right * h + transform.forward * v;
        
        // CRITICAL FIX: Lock movement to the X-Z plane so looking up doesn't make you fly
        moveDir.y = 0f; 

        // --- 3. GRAVITY & GROUNDING ---
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Keeps character glued to ground and stairs
        }

        velocity.y += gravity * Time.deltaTime;

        // Apply final movement via CharacterController
        controller.Move((moveDir * speed + velocity) * Time.deltaTime);
    }
}