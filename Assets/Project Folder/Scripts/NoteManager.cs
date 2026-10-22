using UnityEngine;
using TMPro;

public class NoteManager : MonoBehaviour
{
    public static NoteManager instance;

    [Header("UI & Progress")]
    public TextMeshProUGUI noteCounterText;
    public int notesCollected = 0;
    public int totalNotesNeeded = 4;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void OnNoteCollected()
    {
        notesCollected++;
        UpdateUI();

        if (notesCollected >= totalNotesNeeded)
        {
            Debug.Log("All notes collected! School doors are now unlocked.");
            // Trigger door unlock state here
        }
    }

    void UpdateUI()
    {
        if (noteCounterText != null)
        {
            noteCounterText.text = $"Notes: {notesCollected}/{totalNotesNeeded}";
        }
    }
}