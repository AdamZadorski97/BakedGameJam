using UnityEngine;
using DG.Tweening;

public class SpikesController : MonoBehaviour
{
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private float moveDuration = 2f; // Duration for the spike to reach the end position
    [SerializeField] private float delayBeforeStart = 3f; // Delay before the movement starts
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position; // Store the initial position as the start position
        MoveSpikeWithDelay();
    }

    private void MoveSpikeWithDelay()
    {
        // Calculate the actual end position based on the current position plus the endPosition offset
        Vector3 actualEndPosition = startPosition + endPosition;

        // Create a sequence to move the spike to the end position and back with a delay
        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(delayBeforeStart) // Initial delay before the first movement
                   .Append(transform.DOMove(actualEndPosition, moveDuration).SetEase(Ease.Linear)) // Move to end position
                   .AppendInterval(delayBeforeStart) // Delay at the end position before moving back
                   .Append(transform.DOMove(startPosition, moveDuration).SetEase(Ease.Linear)) // Move back to start position
                   .SetLoops(-1, LoopType.Restart); // Loop the sequence indefinitely
    }
}
