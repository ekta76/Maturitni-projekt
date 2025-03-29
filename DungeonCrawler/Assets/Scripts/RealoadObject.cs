using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealoadObject : MonoBehaviour
{
    public List<GameObject> objectsToToggle;

    void Start()
    {
        ToggleObjectList();
    }

    void ToggleObjectList()
    {
        foreach (GameObject obj in objectsToToggle)
        {
                obj.SetActive(false);
                obj.SetActive(true);
        }
    }
}
