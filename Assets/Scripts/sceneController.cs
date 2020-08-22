using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneController : MonoBehaviour
{
    public void quitButton()
    {
        Application.Quit();
        Debug.Log("quit");
    }
    public void playButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void RestartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void winBattle()
    {
        SceneManager.LoadScene(2);
    }
    public void loseBattle()
    {
        SceneManager.LoadScene(3);
    }

    public void endBattle(string winOrLose)
    {

        if (winOrLose == "Win")
        {
            Invoke("winBattle", 2);
        }
        else
            Invoke("loseBattle", 2);
    }
}
