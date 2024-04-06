using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
public class SwitchController : MonoBehaviour
{
    public UnityEvent switchOnEvent;
    public UnityEvent switchOffEvent;
    [SerializeField] private Vector3 switchOnRotation;
    [SerializeField] private Vector3 switchOffRotation;
    [SerializeField] private Transform switchPivot;
    [SerializeField] private float switchAnimationSpeed;
    private bool isSwitchOn;


    public void Switch()
    {
        if (isSwitchOn)
        {
            isSwitchOn = false;
            switchOffEvent.Invoke();
            switchPivot.DOLocalRotate(switchOffRotation, switchAnimationSpeed);
        }
        else
        {
            isSwitchOn = true;
            switchOnEvent.Invoke();
            switchPivot.DOLocalRotate(switchOnRotation, switchAnimationSpeed);
        }
    }



}
