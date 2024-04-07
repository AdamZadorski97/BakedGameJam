using System.Collections;
using UnityEngine;
using DG.Tweening;

public class GateController : MonoBehaviour
{
    [SerializeField] private Vector3 gatePositionClosed;
    [SerializeField] private Vector3 gatePositionOpened;
    [SerializeField] private Transform gateTransform;
    [SerializeField] private float duration = 2f; // Duration of the animation
    private bool isGateOpened;
    private Coroutine stayOpenCoroutine;

    public void SwitchGate()
    {
        if (isGateOpened)
            CloseGate();
        else
            OpenGate();
    }

    public void OpenGate()
    {
        if (!isGateOpened)
        {
            // Use DOTween to move the gate to the opened position
            isGateOpened = true;
            gateTransform.DOLocalMove(gatePositionOpened, duration).SetEase(Ease.InOutQuad);
        }

        // Restart the coroutine to keep the gate open for 5 more seconds
        if (stayOpenCoroutine != null)
            StopCoroutine(stayOpenCoroutine);
        stayOpenCoroutine = StartCoroutine(KeepGateOpen());
    }

    IEnumerator KeepGateOpen()
    {
        yield return new WaitForSeconds(2f);
        CloseGate();
    }

    public void CloseGate()
    {
        if (isGateOpened)
        {
            // Use DOTween to move the gate to the closed position
            isGateOpened = false;
            gateTransform.DOLocalMove(gatePositionClosed, duration).SetEase(Ease.InOutQuad);
        }
    }
}
