using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossRoomCloseDoor : MonoBehaviour
{
    public Animator wallAnimator;
    public BoxCollider wallDoor;

    public AudioSource wallDoorAudioSource;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            wallDoorAudioSource.Play();

            if (wallAnimator != null)
            {
                wallAnimator.SetTrigger("Pressed");
            }

            if (wallDoor != null)
            {
                wallDoor.enabled = true;
            }

            Destroy(gameObject);
        }
    }
}
