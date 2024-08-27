using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject player; // link camera movement with player movement
    [SerializeField] private float cameraOffsetX = 0f; //camera offset values to determine fixed position away from player
    [SerializeField] private float cameraOffsetY = 2.5f;
    [SerializeField] private float cameraOffsetZ = -10f;

    private bool isDetached = false; //  change when the camera needs to stop following the player

    private Vector3 fallbackPosition; // position to move the camera to after detaching (not working for now)
    private Quaternion fallbackRotation; // rotation to set for the camera after detachment

    private void Awake()
    {
        // detach method to player death event
        Death.OnPlayerDeath += DetachCamera;

        // fallback position and rotation for the camera in case of detachment
        fallbackPosition = new Vector3(0f, 10f, -10f); 
        fallbackRotation = Quaternion.Euler(30f, 0f, 0f); // random values
    }

    void Update()
    {
        //  follow player position onlyy if the camera is not detached and player still exists
        if (!isDetached && player != null)
        {
            transform.position = player.transform.position + new Vector3(cameraOffsetX, cameraOffsetY, cameraOffsetZ);
        }
    }

    private void OnDestroy()
    {
        //  detach method from player death event
        Death.OnPlayerDeath -= DetachCamera;
    }

    private void DetachCamera()
    {
        //stop following player when this is called
        isDetached = true;

        // set the camera to a fixed position and rotation to ensure it stays active
        transform.position = fallbackPosition;
        transform.rotation = fallbackRotation;
    }

    private void LateUpdate()
    {
        // ensure the camera is rendering
        if (Camera.main == null)
        {
            // Re-enable the camera component if it gets disabled
            GetComponent<Camera>().enabled = true;
        }
    }
}


