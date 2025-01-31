using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject deathScreenMenuUI;

    public void playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Resume();
    }

    public void Resume()
    {
        deathScreenMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        deathScreenMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void gameOver()
    {
        Pause();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Resume();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
