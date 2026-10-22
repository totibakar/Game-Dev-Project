using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmeny : MonoBehaviour
{
    //LoadScene
    public void NewGame() 
    {
        SceneManager.LoadScene("puta");
    }
  
    //Quit
    public void Quit() 
    {
        Application.Quit();
        Debug.Log("Player has quit the game");
    }
}
