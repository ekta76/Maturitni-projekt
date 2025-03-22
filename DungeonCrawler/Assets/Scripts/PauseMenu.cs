using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject optionsMenu;

    public Animator transitionAnimator;
    public float transitionTime = 2f;

    private InventoryShow inventoryShow;

    private bool canPressEsc = false;
    private bool couriteHasRun = false;

    private void Start()
    {
        inventoryShow = FindObjectOfType<InventoryShow>();

        if (inventoryShow == null)
        {
            Debug.LogWarning("InventoryShow script not added!");
        }
    }

    public void Update()
    {
        if (!couriteHasRun)
        {
            StartCoroutine(EscKeyStartStop(2.5f));
            couriteHasRun = true;
        }

        if (canPressEsc)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    IEnumerator EscKeyStartStop(float time)
    {
        yield return new WaitForSeconds(time);
        canPressEsc = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        if (inventoryShow != null)
        {
            inventoryShow.enabled = true;
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        if (inventoryShow != null)
        {
            inventoryShow.enabled = false;
        }
    }

    public void LoadMenu()
    {
        Resume();
        canPressEsc = false;
        StartCoroutine(playAnimation("MainMenu"));
    }

    IEnumerator playAnimation(string text)
    {
        transitionAnimator.SetTrigger("nextLevel");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(text);
    }

    public void QuitGame()
    {
        canPressEsc = false;
        Application.Quit();
    }
}
