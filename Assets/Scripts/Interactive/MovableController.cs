using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableController : MonoBehaviour
{
    public InteractorController interactorController;
    public bool isMoving;
    public Vector3 offset;
    public bool isColliding;
    public void OnInteract()
    {
        if (!interactorController.playerController.canMoveObjects)
        {
            return;
        }


        if (!isMoving)
        {
            isMoving = true;

            // Calculate the position offset based on the current positions of the object and the player
            Vector3 positionOffset = transform.parent.transform.position - interactorController.playerController.transform.position;

            // Transform the position offset by the inverse of the player's rotation to make it rotation-dependent
            offset = Quaternion.Inverse(interactorController.playerController.transform.rotation) * positionOffset;
            interactorController.playerController.canRotate = false;
        }
        else
        {
            interactorController.playerController.canRotate = true;
            isMoving = false;
        }
    }
    private void LateUpdate()
    {
        if (isMoving && !isColliding)
        {
            Rigidbody rb = transform.parent.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Apply the player's rotation to the offset before adding it to the player's position
                Vector3 rotatedOffset = interactorController.playerController.transform.rotation * offset;
                Vector3 desiredPosition = interactorController.playerController.transform.position + rotatedOffset;

                // Check for obstructions
                RaycastHit hit;
                if (!Physics.Linecast(transform.parent.position, desiredPosition, out hit))
                {
                    rb.MovePosition(desiredPosition);
                }
                else
                {
                    // Handle collision (e.g., stop movement, adjust position, etc.)
                }
            }
        }
    }


    private void OnCollisionExit(Collision collision)
    {

    }


}
