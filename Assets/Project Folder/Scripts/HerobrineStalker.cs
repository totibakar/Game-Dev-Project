using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HerobrineStalker : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;
    public GameObject ghostVisuals;
    private Collider ghostCollider;

    [Header("Jumpscare & Audio Setup")]
    public GameObject jumpscareUI;
    public AudioSource jumpscareAudio;
    public MonoBehaviour playerMovement;
    public float jumpscareDuration = 2f;
    public float rushSpeed = 35f;

    [Header("Gameplay UI Warning")]
    public GameObject warningTextObject; // Drag your WarningText GameObject here

    [Header("Jumpscare Face Alignment")]
    [Tooltip("How far from the camera lens the ghost stops. Increase if the head clips through the screen.")]
    public float stopDistance = 1.1f;
    [Tooltip("Lowers/Raises the ghost body so its eyes align with the camera. Higher = ghost goes lower.")]
    public float ghostFaceHeightOffset = 1.7f;

    [Header("Timer Settings")]
    public float gracePeriod = 180f;
    public float minSpawnDelay = 3f;
    public float maxSpawnDelay = 6f;

    [Header("Spawn Distance Settings")]
    public float minDistance = 6f;
    public float maxDistance = 10f;

    [Header("Stare Detection Settings")]
    public float stareTimeRequired = 3f;
    private float currentStareTime = 0f;

    [Header("Model Rotation Fix")]
    public float yRotationOffset = 180f;

    private bool isSpotted = false;
    private bool isGhostActiveState = false;
    private bool hasJumpscared = false;
    private float groundY;

    [Header("Game Over UI Setup")]
    public GameObject gameOverPanel; // Drag your GameOverPanel here in the Inspector


    void Start()
    {
        ghostCollider = GetComponent<Collider>();
        groundY = transform.position.y;

        Vanish();
        StartCoroutine(GracePeriodRoutine());
    }

    void Update()
    {
        if (!isGhostActiveState || hasJumpscared) return;

        // 1. Face player
        Vector3 lookDirection = playerCamera.position - transform.position;
        lookDirection.y = 0f;
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = targetRotation * Quaternion.Euler(0, yRotationOffset, 0);
        }

        // 2. Visibility Check
        Vector3 dirToGhost = (transform.position - playerCamera.position).normalized;
        bool isInView = Vector3.Dot(playerCamera.forward, dirToGhost) > 0.7f;

        if (isInView)
        {
            isSpotted = true;
            currentStareTime += Time.deltaTime;

            if (currentStareTime >= stareTimeRequired)
            {
                StartCoroutine(RushJumpscareRoutine("Player stared at the ghost for too long!"));
            }
        }
        else
        {
            if (isSpotted)
            {
                Debug.Log("Ghost vanished safely into the fog.");
                Vanish();
            }
            else
            {
                currentStareTime = 0f;
            }
        }
    }

    IEnumerator GracePeriodRoutine()
    {
        yield return new WaitForSeconds(gracePeriod);
        StartCoroutine(SpawnLoopRoutine());
    }

    IEnumerator SpawnLoopRoutine()
    {
        while (!hasJumpscared)
        {
            float waitTime = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(waitTime);

            if (!isGhostActiveState && !hasJumpscared)
            {
                TryTeleportAndSpawn();
            }
        }
    }

    void TryTeleportAndSpawn()
    {
        float randomAngle = Random.Range(0f, 360f);
        float distance = Random.Range(minDistance, maxDistance);

        Vector3 spawnOffset = new Vector3(Mathf.Sin(randomAngle), 0f, Mathf.Cos(randomAngle)) * distance;
        Vector3 targetPosition = playerCamera.position + spawnOffset;

        targetPosition.y = groundY;
        transform.position = targetPosition;

        isSpotted = false;
        currentStareTime = 0f;

        if (ghostVisuals != null) ghostVisuals.SetActive(true);
        if (ghostCollider != null) ghostCollider.enabled = true;

        isGhostActiveState = true;

        if (warningTextObject != null) warningTextObject.SetActive(true);
    }

    void Vanish()
    {
        isSpotted = false;
        isGhostActiveState = false;
        currentStareTime = 0f;

        if (ghostVisuals != null) ghostVisuals.SetActive(false);
        if (ghostCollider != null) ghostCollider.enabled = false;

        if (warningTextObject != null) warningTextObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponentInParent<FPS_Move>() != null)
        {
            if (!hasJumpscared)
            {
                ExecuteFinalJumpscareEffects("Player walked directly into the ghost!");
            }
        }
    }

    // --- RUSH SEQUENCE ---
    IEnumerator RushJumpscareRoutine(string reason)
    {
        hasJumpscared = true;
        isGhostActiveState = false;

        // 1. DISABLE COLLIDER: This allows the ghost to clip through the ground and player physics
        if (ghostCollider != null) ghostCollider.enabled = false;

        // 2. Lock player controls
        if (playerMovement != null) playerMovement.enabled = false;

        Vector3 targetPos = CalculateFaceTargetPosition();

        // 3. Dash toward camera
        while (Vector3.Distance(transform.position, targetPos) > stopDistance)
        {
            Vector3 lookDir = playerCamera.position - transform.position;
            lookDir.y = 0f;
            if (lookDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDir) * Quaternion.Euler(0, yRotationOffset, 0);
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, rushSpeed * Time.deltaTime);
            targetPos = CalculateFaceTargetPosition();

            yield return null;
        }

        // 4. FINAL SNAP ALIGNMENT: Forces exact position so face is guaranteed on screen without clipping hollow body
        transform.position = CalculateFaceTargetPosition();
        Vector3 finalLook = playerCamera.position - transform.position;
        finalLook.y = 0f;
        if (finalLook != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(finalLook) * Quaternion.Euler(0, yRotationOffset, 0);
        }

        ExecuteFinalJumpscareEffects("Ghost reached player face.");
    }

    Vector3 CalculateFaceTargetPosition()
    {
        // Pushes ghost out by stopDistance along camera forward ray, lowered by ghostFaceHeightOffset
        Vector3 target = playerCamera.position + (playerCamera.forward * stopDistance);
        target.y = playerCamera.position.y - ghostFaceHeightOffset;
        return target;
    }

    // --- IMPACT & AUDIO TRIGGER ---
    void ExecuteFinalJumpscareEffects(string reason)
    {
        hasJumpscared = true;

        if (playerMovement != null) playerMovement.enabled = false;

        if (jumpscareAudio != null)
        {
            jumpscareAudio.Stop();
            jumpscareAudio.Play();
        }

        if (jumpscareUI != null) jumpscareUI.SetActive(true);

        // Start the Game Over sequence instead of reloading the scene
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        // Wait for the initial jumpscare audio/flash to hit (e.g., 1.5 seconds)
        yield return new WaitForSeconds(2f);

        // Hide the flashing jumpscare overlay and show the persistent Game Over screen
        if (jumpscareUI != null) jumpscareUI.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        // Unlock mouse cursor so UI interaction is free if needed
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Wait another 2.5 seconds on the Game Over screen before shutting down the game
        yield return new WaitForSeconds(2.5f);

        //Debug.Log("Exiting Game...");
        //Application.Quit();

        // Note: Application.Quit() does not close the window inside the Unity Editor Play Mode. 
        // To test it in the Editor, you can uncomment the line below:

        //UnityEditor.EditorApplication.isPlaying = false;

        Debug.Log("Returning to Main Menu...");

        // Load your Main Menu scene (make sure the string matches your exact scene name)
        SceneManager.LoadScene("Main Menu");

    }
}