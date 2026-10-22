using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public GameObject PausePanel;

    void Update()
    {
        // Press ESC key to toggle between pause and continue
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PausePanel.activeSelf)
            {
                Continue();
            }
            else
            {
                pause();
            }
        }
    }

    public void pause() 
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0; // Freezes the game time

        // Unlock and show the mouse cursor so you can click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Game paused");
    }

    public void Continue() 
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1; // Resumes game time

        // Lock and hide the cursor again for first-person gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Game continued");
    }

    // Quit
    public void Quit() 
    {
        Time.timeScale = 1; // Always reset time scale before quitting or changing scenes
        Application.Quit();
        Debug.Log("Player has quit the game");
    }
}