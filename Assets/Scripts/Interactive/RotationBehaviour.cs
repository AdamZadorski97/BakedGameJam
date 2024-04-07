using UnityEngine;
using MEC;
using System.Collections.Generic;


public class RotationBehaviour : MonoBehaviour
{

    [SerializeField] private float acceleration = 5.0f;
    [SerializeField] private Vector3 direction = Vector3.forward;
    [SerializeField] private float speed = 0.0f;

    void Update()
    {
        Rotation();
    }

    public virtual void Rotation()
    {
        transform.Rotate(direction * speed * Time.deltaTime);
    }

    #region Direction

    public void SetDirection(Vector3 value)
    {
        direction = value;
    }

 

    public IEnumerator<float> TweenDirection(float duration, Vector3 targetDirection, AnimationCurve curve)
    {
        float t = 0f;
        Vector3 startDirection = direction;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = t / duration;
            float curveValue = curve.Evaluate(normalizedTime);
            direction = Vector3.Lerp(startDirection, targetDirection, curveValue);
            yield return Timing.WaitForOneFrame;
        }
        direction = targetDirection;
    }


    #endregion

    #region Speed
    public void SetSpeed(float value)
    {
        speed = value;
    }

    public void AddSpeed(float value)
    {
        speed += value;
    }

    public IEnumerator<float> TweenSpeed(float duration, float targetSpeed, AnimationCurve curve)
    {
        float t = 0f;
        float startSpeed = speed;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = t / duration;
            float curveValue = curve.Evaluate(normalizedTime);
            speed = Mathf.Lerp(startSpeed, targetSpeed, curveValue);
            yield return Timing.WaitForOneFrame;
        }
        speed = targetSpeed;
    }
    #endregion
}
