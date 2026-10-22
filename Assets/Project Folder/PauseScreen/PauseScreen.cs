using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public GameObject pausePanel; // This matches your Inspector slot
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        pausePanel.SetActive(true); // <-- THIS turns the menu on!
        Time.timeScale = 0f;
        isPaused = true;
        Debug.Log("Game paused");
    }

    public void Resume()
    {
        pausePanel.SetActive(false); // <-- THIS turns the menu off!
        Time.timeScale = 1f;
        isPaused = false;
        Debug.Log("Game continued");
    }
}