using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPingPongBehaviour : MonoBehaviour
{
    [SerializeField] private float acceleration = 5.0f;
    [SerializeField] private Vector3 direction = Vector3.forward;
    [SerializeField] private float rotationSpeed = 0.0f;
    [SerializeField] private float minSpeed = -45.0f;
    [SerializeField] private float maxSpeed = 45.0f;

    [SerializeField] private bool isPingPongEnabled = false;
    private bool isPingPongDirectionForward = true;

    [SerializeField] private float minZAngle = -45.0f;
    [SerializeField] private float maxZAngle = 45.0f;

    [Tooltip("Random time in seconds before rotation starts when the scene is loaded (capped at this value)")]
    [SerializeField] private float maxDelay = 3.0f;
    private float delay = 0.0f;
    private float startPoint = 0.0f;

    private float targetZAngle;

    void Start()
    {
        SetStartPoint();
    }

    void Update()
    {
        Rotation();
    }

    private void SetStartPoint()
    {
        delay = Random.Range(0.0f, maxDelay); // Randomize delay
        startPoint = Time.time + delay;
    }

    public virtual void Rotation()
    {
        // Check if it's time to start rotating
        if (Time.time < startPoint)
        {
            return;
        }

        if (isPingPongEnabled)
        {
            if (isPingPongDirectionForward)
            {
                rotationSpeed += acceleration * Time.deltaTime;
                if (rotationSpeed >= maxSpeed)
                {
                    rotationSpeed = maxSpeed;
                    isPingPongDirectionForward = false;
                }
            }
            else
            {
                rotationSpeed -= acceleration * Time.deltaTime;
                if (rotationSpeed <= minSpeed)
                {
                    rotationSpeed = minSpeed;
                    isPingPongDirectionForward = true;
                }
            }
        }
        else
        {
            rotationSpeed = Mathf.Clamp(rotationSpeed, minSpeed, maxSpeed);
        }

        // Calculate the target angle and handle negative angles correctly
        targetZAngle = Mathf.Clamp(targetZAngle + rotationSpeed * Time.deltaTime, minZAngle, maxZAngle);

        // Adjust for negative angles
        if (targetZAngle > 180)
        {
            targetZAngle -= 360;
        }

        // Smoothly rotate to the target angle
        float currentZAngle = transform.eulerAngles.z;
        float newZAngle = Mathf.MoveTowardsAngle(currentZAngle, targetZAngle, Mathf.Abs(rotationSpeed) * Time.deltaTime);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, newZAngle);
    }

    #region Direction

    public void SetDirection(Vector3 value)
    {
        direction = value.normalized;
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
            direction = Vector3.Lerp(startDirection, targetDirection, curveValue).normalized;
            yield return 0f;
        }
        direction = targetDirection.normalized;
    }

    #endregion

    #region Speed

    public void SetRotationSpeed(float value)
    {
        rotationSpeed = value;
    }

    public void AddRotationSpeed(float value)
    {
        rotationSpeed += value;
    }

    public IEnumerator<float> TweenRotationSpeed(float duration, float targetSpeed, AnimationCurve curve)
    {
        float t = 0f;
        float startSpeed = rotationSpeed;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = t / duration;
            float curveValue = curve.Evaluate(normalizedTime);
            rotationSpeed = Mathf.Lerp(startSpeed, targetSpeed, curveValue);
            yield return 0f;
        }
        rotationSpeed = targetSpeed;
    }

    #endregion

    #region Ping Pong

    public void TogglePingPong(bool enable)
    {
        isPingPongEnabled = enable;
        isPingPongDirectionForward = true;
    }

    #endregion
}
