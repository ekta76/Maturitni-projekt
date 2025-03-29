using Unity.VisualScripting;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Movement playerMovement;
    public Transform teleportLocation;
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;
    public GameObject mapCamera;
    public Vector3 cameraTeleportPosition;

    private void Start()
    {
        mapCamera = GameObject.FindGameObjectWithTag("MapCamera");
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerMovement.Teleport(teleportLocation.position);
        }

        if (mapCamera != null)
        {
            mapCamera.transform.position = cameraTeleportPosition;
        }

        foreach (GameObject obj in objectsToDeactivate)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(true);
        }
    }
}
