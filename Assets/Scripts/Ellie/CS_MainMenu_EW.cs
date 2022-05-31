using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CS_MainMenu_EW : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene(0); 
    }

    public void Quit()
    {
        Application.Quit(); 
    }

    public void MainToControls()
    {
        SceneManager.LoadScene(2);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(1); 
    }
}
