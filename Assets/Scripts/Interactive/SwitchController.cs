using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
public class SwitchController : MonoBehaviour
{
    public UnityEvent switchOnEvent;
    public UnityEvent switchOffEvent;
    public UnityEvent switchFirstTimeEvent;
    [SerializeField] private Vector3 switchOnRotation;
    [SerializeField] private Vector3 switchOffRotation;
    [SerializeField] private Transform switchPivot;
    [SerializeField] private float switchAnimationSpeed;
    private bool isSwitchOn;
    public bool WasSwitched;

    public void Switch()
    {

        if(!WasSwitched )
        {
            switchFirstTimeEvent.Invoke();
        }

        WasSwitched = true;
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
