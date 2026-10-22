using UnityEngine;
using UnityEngine.SceneManagement;

public class SchoolDoorTrigger : MonoBehaviour
{
    [Header("Settings")]
    public string nextSceneName = "Level2_SchoolInterior";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponentInParent<FPS_Move>() != null)
        {
            // Note the lowercase 'instance' matching your NoteManager code
            if (NoteManager.instance != null && NoteManager.instance.AreAllNotesCollected())
            {
                Debug.Log("All notes collected! Entering the school...");
                LoadNextLevel();
            }
            else
            {
                Debug.Log("The doors are locked. I need to collect all the notes first.");
            }
        }
    }

    void LoadNextLevel()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(nextSceneName);
    }
}