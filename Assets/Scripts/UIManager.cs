using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;

    public GameObject blueWin;
    public GameObject redWin;

    private bool isGameStarted = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseMenu.activeInHierarchy)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeInHierarchy)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            isGameStarted = true;
        }

        if (isGameStarted)
        {
            GameObject[] bluePlayer = GameObject.FindGameObjectsWithTag("BlueTeam");
            GameObject[] redPlayer = GameObject.FindGameObjectsWithTag("RedTeam");

            if(bluePlayer.Length == 0)
            {
                redWin.SetActive(true);
            }

            if(redPlayer.Length == 0)
            {
                blueWin.SetActive(true);
            }
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    
}
