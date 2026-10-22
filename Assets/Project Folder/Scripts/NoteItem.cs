using UnityEngine;

public class NoteItem : MonoBehaviour
{
    [Header("Note Settings")]
    public string noteID; // Optional identifier if you want specific notes to trigger specific events

    public void CollectNote()
    {
        // Tell the GameManager or UI that a note was collected
        NoteManager.instance.OnNoteCollected();

        // Destroy or disable this note object
        Destroy(gameObject);
    }
}