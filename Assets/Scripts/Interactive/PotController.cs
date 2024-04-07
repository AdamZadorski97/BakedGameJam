using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PotController : MonoBehaviour
{
    public GameObject key;
    public Vector3 targetRotation;

    private Vector3 startRotation;
    private Sequence rotateSequence;
 [SerializeField]   private Transform pot;
    [SerializeField] private float rotateToTargetSpeed;
    public Transform keyDropPosition;
    public float keyJumpPower;
    public float keyJumpDuration;
    private bool potWasRotated;

    private void Start()
    {
        startRotation = pot.localEulerAngles;
    }

    public void RotatePot()
    {
        if(potWasRotated)
        {
            return;
        }
        potWasRotated = true;
        if (rotateSequence != null)
            rotateSequence.Kill();
        rotateSequence = DOTween.Sequence();
        GetComponent<AudioSource>().Play();
        rotateSequence.Append(pot.DOLocalRotate(targetRotation, rotateToTargetSpeed));
        rotateSequence.AppendCallback(() => key.gameObject.SetActive(true));
        rotateSequence.AppendCallback(() => key.transform.SetParent(null));
        rotateSequence.Append(pot.DOLocalRotate(startRotation, rotateToTargetSpeed));

        rotateSequence.Join(key.transform.DOJump(keyDropPosition.position, keyJumpPower, 1, keyJumpDuration));
    }
}
