using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Vector3 doorRotationClosed;
    [SerializeField] private Vector3 doorRotationOpened;
    [SerializeField] private Transform gateTransform;
    [SerializeField] private float duration = 2f; // Duration of the animation
    private bool isDoorOpened;
    private Sequence DoorMoveSequence;
    public bool isDoorLocked;
    public InteractorController interactorController;
    public void SwitchDoor()
    {
        if (isDoorOpened)
            CloseDoor();
        else
            TryToOpenDoor();
    }

    public void TryToOpenDoor()
    {
        if(interactorController.playerController.hasKey)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        // Use DOTween to move the gate to the opened position
        if (DoorMoveSequence != null)
            DoorMoveSequence.Kill();
        DoorMoveSequence = DOTween.Sequence();

        DoorMoveSequence.Append(gateTransform.DOLocalRotate(doorRotationOpened, duration).SetEase(Ease.InOutQuad)); // Choose an easing function that suits the movement
    }

    public void CloseDoor()
    {
        // Use DOTween to move the gate to the closed position
        if (DoorMoveSequence != null)
            DoorMoveSequence.Kill();
        DoorMoveSequence = DOTween.Sequence();
        DoorMoveSequence.Append(gateTransform.DOLocalRotate(doorRotationClosed, duration).SetEase(Ease.InOutQuad)); // Choose an easing function that suits the movement
    }
}
