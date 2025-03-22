using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator animator;
    public float transitionTime = 2f;

     public void playGame()
    {
        StartCoroutine(playAnimation(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator playAnimation(int levelIndex)
    {
        animator.SetTrigger("nextLevel");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    public void exitGame ()
    {
        Application.Quit();
    }
}
