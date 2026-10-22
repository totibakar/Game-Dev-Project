using UnityEngine;
using TMPro;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    [Header("UI References")]
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;

    [Header("Player Control Reference")]
    public MonoBehaviour playerMovement; // Drag your player movement script here

    private bool isDialogActive = false;
    private System.Action onDialogClosedCallback;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        // If dialog is open, press E or Space to close it
        if (isDialogActive && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)))
        {
            CloseDialog();
        }
    }

    public void ShowDialog(string message, System.Action onClosed = null)
    {
        if (dialogPanel != null) dialogPanel.SetActive(true);
        if (dialogText != null) dialogText.text = message;

        isDialogActive = true;
        onDialogClosedCallback = onClosed;

        // Freeze player movement while reading
        if (playerMovement != null) playerMovement.enabled = false;

        // Unlock cursor so they can see/click if needed, or keep locked for FPS
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseDialog()
    {
        if (dialogPanel != null) dialogPanel.SetActive(false);
        isDialogActive = false;

        // Restore player movement
        if (playerMovement != null) playerMovement.enabled = true;

        // Re-lock cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Run any follow-up event (like unlocking doors or starting timers)
        onDialogClosedCallback?.Invoke();
        onDialogClosedCallback = null;
    }
}