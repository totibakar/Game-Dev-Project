using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    [Header("Settings")]
    public string menuSceneName = "Main Menu"; // Make sure this matches your exact Main Menu scene name!

    void Update()
    {
        // Check if the player presses the E key or Spacebar
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            ReturnToMainScreen();
        }
    }

    void ReturnToMainScreen()
    {
        // Unlock cursor before switching to menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(menuSceneName);
    }
}