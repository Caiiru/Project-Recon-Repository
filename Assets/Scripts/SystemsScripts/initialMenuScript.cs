using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class initialMenuScript : MonoBehaviour
{
    public GameObject initialMenu;
    public GameObject optionsMenu;
    public GameObject setChangeMenu;
    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    public void playButton()
    {
        setChangeMenu.SetActive(true);
        initialMenu.SetActive(false);
        Debug.Log("Play Button");
    }

    public void optionsButton()
    {
        initialMenu.SetActive(false);
        optionsMenu.SetActive(true);
        Debug.Log("Options Button");
    }
    public void optionsBackButton()
    {
        initialMenu.SetActive(true);
        optionsMenu.SetActive(false);
        setChangeMenu.SetActive(false);
    }
    public void GoToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void selectSet(int set)
    {
        Debug.Log(set);
        Invoke("GoToNextScene", 2f);
    }
}
