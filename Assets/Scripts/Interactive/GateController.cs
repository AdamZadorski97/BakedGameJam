using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // Import the DOTween namespace

public class GateController : MonoBehaviour
{
    [SerializeField] private Vector3 gatePositionClosed;
    [SerializeField] private Vector3 gatePositionOpened;
    [SerializeField] private Transform gateTransform;
    [SerializeField] private float duration = 2f; // Duration of the animation
    private bool isGateOpened;
    private Sequence gateMoveSequence;
    public void SwitchGate()
    {
        if (isGateOpened)
            CloseGate();
        else
            OpenGate();
    }

    public void OpenGate()
    {
        // Use DOTween to move the gate to the opened position
        if (gateMoveSequence != null)
            gateMoveSequence.Kill();
        gateMoveSequence = DOTween.Sequence();

        gateMoveSequence.Append(gateTransform.DOLocalMove(gatePositionOpened, duration).SetEase(Ease.InOutQuad)); // Choose an easing function that suits the movement
    }

    public void CloseGate()
    {
        // Use DOTween to move the gate to the closed position
        if (gateMoveSequence != null)
            gateMoveSequence.Kill();
        gateMoveSequence = DOTween.Sequence();
        gateMoveSequence.Append(gateTransform.DOLocalMove(gatePositionClosed, duration).SetEase(Ease.InOutQuad)); // Choose an easing function that suits the movement
    }
}
