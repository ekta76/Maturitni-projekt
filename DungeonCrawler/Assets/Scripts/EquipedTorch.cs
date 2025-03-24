using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipedTorch : MonoBehaviour
{
    public GameObject torchLights;
    public GameObject normalLights;

    public int hasLights = 0;

    void Update()
    {
        if (hasLights < 0)
        {
            hasLights = 0;
        }

        if (hasLights > 0)
        {
            torchLights.SetActive(true);
            normalLights.SetActive(false);
        }
        else
        {
            torchLights.SetActive(false);
            normalLights.SetActive(true);
        }
    }
}
