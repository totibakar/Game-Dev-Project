using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public GameObject PausePanel;

    void update(){}

    public void pause() 
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
        Debug.Log("Game paused");
    }

    public void Continue() 
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        Debug.Log("Game continued");
    }

    //Quit
    public void Quit() 
    {
        Application.Quit();
        Debug.Log("Player has quit the game");
    }
}
