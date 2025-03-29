using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class EndingScreenMenu : MonoBehaviour
{
    public Animator animator;
    public float transitionTime = 2f;

    public void loadMainMenu()
    {
        StartCoroutine(playAnimation("MainMenu"));
    }

    IEnumerator playAnimation(string text)
    {
        animator.SetTrigger("nextLevel");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(text);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
