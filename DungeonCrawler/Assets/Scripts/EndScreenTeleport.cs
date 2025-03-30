using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenTeleport : MonoBehaviour
{
    public Animator transitionAnimator;
    public float transitionTime = 2f;
    public PauseMenu pauseMenu;
    public InventoryShow inventoryShow;
    public Movement movement;

    private void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
        inventoryShow = FindObjectOfType<InventoryShow>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            pauseMenu.canPressEsc = false;
            inventoryShow.enabled = false;
            movement.enabled = false;
            StartCoroutine(playAnimation(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    IEnumerator playAnimation(int levelIndex)
    {
        transitionAnimator.SetTrigger("nextLevel");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
