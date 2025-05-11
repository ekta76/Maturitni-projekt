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

    public Vector3 cameraStartPosition;

    private void Start()
    {
        cameraStartPosition = new Vector3(0.5f, 14.22f, 0f);

        if (mapCamera != null)
        {
            mapCamera.transform.localPosition = cameraStartPosition;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerMovement.Teleport(teleportLocation.position);

            if (mapCamera != null)
            {
                mapCamera.transform.position = cameraTeleportPosition;
            }

            foreach (GameObject obj in objectsToDeactivate)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }

            foreach (GameObject obj in objectsToActivate)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
        }
    }
}
