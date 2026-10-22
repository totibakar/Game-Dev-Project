using UnityEngine;
using TMPro;

public class NoteManager : MonoBehaviour
{
    public static NoteManager instance;

    [Header("UI & Progress")]
    public TextMeshProUGUI noteCounterText;
    public int notesCollected = 0;
    public int totalNotesNeeded = 4;

    [Header("Objective UI")]
    public TextMeshProUGUI objectiveText; // Drag your ObjectiveText component here

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();

        Invoke("TriggerIntroDialog", 0.1f);
    }

    void TriggerIntroDialog()
    {
        if (DialogManager.instance != null)
        {
            DialogManager.instance.ShowDialog("I wonder what secrets I could discover in a place like this. I heard that there are missing notes that tells us stories of this place scattered around the grounds! (Press E)");
        }
    }

    public void OnNoteCollected()
    {
        notesCollected++;
        UpdateUI();

        if (notesCollected >= totalNotesNeeded)
        {
            Debug.Log("All notes collected! School doors are now unlocked.");
            // Trigger door unlock state here

            // --- POST-COLLECTION DIALOG TRIGGER ---
            if (DialogManager.instance != null)
            {
                DialogManager.instance.ShowDialog("I think that's all the notes there are here. Maybe I can find some more inside the school building. There's only one way to find out! (Press E)");
            }
        }
    }

    void UpdateUI()
    {
        if (noteCounterText != null)
        {
            noteCounterText.text = $"\nNotes: {notesCollected}/{totalNotesNeeded}";
        }

        if (noteCounterText != null)
        {
            noteCounterText.text = $"\nNotes: {notesCollected}/{totalNotesNeeded}";
        }

        // Update objective description when all notes are gathered
        if (objectiveText != null)
        {
            if (notesCollected >= totalNotesNeeded)
            {
                objectiveText.text = "Enter the School Building NOW!";
            }
            else
            {
                objectiveText.text = "Find the scattered notes";
            }
        }
    }

    // ADD THIS METHOD SO THE DOOR TRIGGER CAN CHECK PROGRESS
    public bool AreAllNotesCollected()
    {
        return notesCollected >= totalNotesNeeded;
    }
}