using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealoadObject : MonoBehaviour
{
    public List<GameObject> disableObjectsToToggle;
    public List<GameObject> enableObjectsToToggle;
    public GameObject optionsMenu;

    void Start()
    {
        DisableToggleObjectList();
        EnableToggleObjectList();

        StartCoroutine(reloadOptionsAtStart());
    }

    void DisableToggleObjectList()
    {
        foreach (GameObject obj in disableObjectsToToggle)
        {
            obj.SetActive(true);
            obj.SetActive(false);
        }
    }

    void EnableToggleObjectList()
    {
        foreach (GameObject obj in enableObjectsToToggle)
        {
            obj.SetActive(false);
            obj.SetActive(true);
        }
    }

    IEnumerator reloadOptionsAtStart()
    {
        optionsMenu.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        optionsMenu.SetActive(false);
    }
}
