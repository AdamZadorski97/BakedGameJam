using DG.Tweening;
using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    public InputDevice InputDevice { get; set; } // Add this line

    public int playerID;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 5f; // The force applied upwards when jumping

    private Vector3 moveInput;
    private Vector2 lookInput;

    [SerializeField] private Transform groundCheck; // A transform positioned at the bottom of the player used for grounding checks
    [SerializeField] private float groundDistance = 0.2f; // The radius of the grounding check
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private Animator animator;
    private bool isGrounded;
    public bool isCrouch;
    public bool isMoveObject;
    [SerializeField] private bool canJump;
    [SerializeField] private bool canCrouch;
    public bool canRotate;
    public bool canMoveObjects;
    public InteractorController tempInteractorController;
    private bool interact;
    private Quaternion lastIntendedRotation;
    public bool hasKey;
    public bool isMan;
    public CapsuleCollider capsuleCollider;
    private void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Retrieve movement input based on the player ID
        if (playerID == 1)
        {
            Vector2 input = InputController.Instance.player1MoveValue;
            moveInput = new Vector3(input.x, 0, input.y).normalized;
            lookInput = InputController.Instance.Player1Actions.lookAction.Value;
            if (isGrounded && InputController.Instance.Player1Actions.jumpAction.WasPressed)
            {
                Jump();
            }
            if (InputController.Instance.Player1Actions.interactionAction.WasPressed && tempInteractorController != null)
            {
                tempInteractorController.OnInteract(this);
            }
            if (InputController.Instance.Player1Actions.crowlAction.WasPressed)
            {
                Crouch();
            }
        }
        else if (playerID == 2)
        {
            Vector2 input = InputController.Instance.player2MoveValue;
            moveInput = new Vector3(input.x, 0, input.y).normalized;
            lookInput = InputController.Instance.Player2Actions.lookAction.Value;
            if (isGrounded && InputController.Instance.Player2Actions.jumpAction.WasPressed)
            {
                Jump();
            }
            if (InputController.Instance.Player2Actions.interactionAction.WasPressed && tempInteractorController != null)
            {
                tempInteractorController.OnInteract(this);
            }
            if (InputController.Instance.Player2Actions.crowlAction.WasPressed)
            {
                Crouch();
            }
        }

        // Animation control
        if (moveInput.magnitude > 0.1f) // Assuming a small threshold to account for joystick drift
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }

    private void LateUpdate()
    {
        // Movement
        if (canRotate)
        {
            if (!IsObstacleInFront()) // Only move forward if there's no obstacle
            {
                if(!isCrouch)
                rb.MovePosition(rb.position + moveInput * moveSpeed * Time.deltaTime);
                else
                {
                    rb.MovePosition(rb.position + moveInput * moveSpeed/2 * Time.deltaTime);
                }
            }
        }
        else
        {
            Vector3 adjustedMoveInput = moveInput;
            float forwardMovement = Vector3.Dot(moveInput, transform.forward);
            adjustedMoveInput = transform.forward * forwardMovement;

            if (!IsObstacleInFront()) // Check for obstacle when not rotating
            {
                rb.MovePosition(rb.position + adjustedMoveInput * moveSpeed * Time.deltaTime);
            }
        }

        // Rotation
        if (canRotate)
        {
            Vector3 intendedDirection = lookInput.magnitude > 0.1f ? new Vector3(lookInput.x, 0, lookInput.y) : moveInput.normalized;
            if (intendedDirection != Vector3.zero)
            {
                // Update the last intended rotation based on the current input
                lastIntendedRotation = Quaternion.LookRotation(intendedDirection);
                // Use Slerp for smoother spherical interpolation
                rb.rotation = Quaternion.Slerp(rb.rotation, lastIntendedRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                // If there's no input, maintain current rotation smoothly
                SnapRotationToEnd();
            }
        }
    }

    public void SnapRotationToEnd()
    {
        if (lastIntendedRotation != Quaternion.identity) // Ensure there's a last known rotation to snap to
        {
            rb.rotation = Quaternion.RotateTowards(rb.rotation, lastIntendedRotation, rotationSpeed * 2 * Time.fixedDeltaTime);
        }
    }

    public void SnapToMovable()
    {

    }
    public AudioClip gosiaHop;
    private void Jump()
    {
        if (canJump)
        {
            GetComponent<AudioSource>().PlayOneShot(gosiaHop);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    private bool IsObstacleInFront()
    {
        if (isMoveObject) return false;

        if (moveInput == Vector3.zero) return false; // If there's no move input, no need to check for obstacles.

        RaycastHit hit;
        // Normalize moveInput to ensure consistent ray length
        Vector3 direction = moveInput.normalized;
        // Cast a ray from the player's position in the direction of the move input
        bool hasHit = Physics.Raycast(transform.position, direction, out hit, 0.3f, wallMask); // Adjust the distance as needed
                                                                                               // Optionally, visualize the raycast in the scene view
        Debug.DrawRay(transform.position, direction * 0.5f, Color.red); // Adjust the ray length for visualization as needed

        return hasHit;
    }
    private void Crouch()
    {
        if (canCrouch)
        {
            if (isCrouch)
            {
                isCrouch = false;
                animator.SetBool("isCrouch", false);
                capsuleCollider.height = 1;
                // Tween the center back to the standing position
                capsuleCollider.DOComplete(); // Optional: Complete any previous tweens on the collider
                DOTween.To(() => capsuleCollider.center, x => capsuleCollider.center = x, new Vector3(0, 0f, 0), 0.5f);
            }
            else
            {
                isCrouch = true;
                animator.SetBool("isCrouch", true);
                capsuleCollider.height = 0.3f;
                // Tween the center to the crouching position
                capsuleCollider.DOComplete(); // Optional: Complete any previous tweens on the collider
                DOTween.To(() => capsuleCollider.center, x => capsuleCollider.center = x, new Vector3(0, -0.35f, 0), 0.2f);
            }
        }
    }
    public void Movable(bool valuemove)
    {
        isMoveObject = valuemove;
        animator.SetBool("isMovable", valuemove);
    }

    public void Interact()
    {
        tempInteractorController.OnInteract(this);
    }

    public AudioClip jasHit;
    public AudioClip gosiaHit;
    public AudioClip JasWalk;

    public void GetHit()
    {
        if (playerID == 1)
        {
            InputController.Instance.Vibrate(0.25f, InputController.Instance.Player1Actions, 0.3f);
        }

        if (playerID == 2)
        {
            InputController.Instance.Vibrate(0.25f, InputController.Instance.Player2Actions, 0.3f);
        }


        if (isMan) GetComponent<AudioSource>().PlayOneShot(jasHit);
        else GetComponent<AudioSource>().PlayOneShot(gosiaHit);
    }

    public void WalkSound()
    {
        InputController.Instance.Vibrate(0.1f, InputController.Instance.Player1Actions, 0.1f);
        if (isMan) GetComponent<AudioSource>().PlayOneShot(JasWalk);
    }

}