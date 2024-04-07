using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

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
    [SerializeField] private bool canJump;
    [SerializeField] private bool canCrouch;
    public bool canRotate;
    public bool canMoveObjects;
    public InteractorController tempInteractorController;
    private bool interact;
    private Quaternion lastIntendedRotation;
    public bool hasKey;
    private void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Retrieve movement input based on the player ID
        if (playerID == 1)
        {
            Vector2 input = InputController.Instance.player1MoveValue;
            moveInput = new Vector3(input.x, 0, input.y);
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
            moveInput = new Vector3(input.x, 0, input.y);
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

    private void FixedUpdate()
    {
        if (canRotate)
        {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 adjustedMoveInput = moveInput;
            float forwardMovement = Vector3.Dot(moveInput, transform.forward);
            adjustedMoveInput = transform.forward * forwardMovement;
          //  if(IsObstacleInFront())
            rb.MovePosition(rb.position + adjustedMoveInput * moveSpeed * Time.deltaTime);
        }

        if (canRotate)
        {
            Vector3 intendedDirection = lookInput.magnitude > 0.1f ? new Vector3(lookInput.x, 0, lookInput.y) : moveInput;
            if (intendedDirection != Vector3.zero)
            {
                // Update the last intended rotation based on the current input
                lastIntendedRotation = Quaternion.LookRotation(intendedDirection);
                rb.rotation = Quaternion.Lerp(rb.rotation, lastIntendedRotation, rotationSpeed * Time.fixedDeltaTime);
            }
            else
            {
                // If there's no input, snap to the last known intended rotation
                SnapRotationToEnd();
            }
        }
    }
    private void SnapRotationToEnd()
    {
        if (lastIntendedRotation != Quaternion.identity) // Ensure there's a last known rotation to snap to
        {
            rb.rotation = Quaternion.RotateTowards(rb.rotation, lastIntendedRotation, rotationSpeed *2* Time.fixedDeltaTime);
        }
    }

    private void Jump()
    {
        if (canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Crouch()
    {
        if(canCrouch)
        {
            if (isCrouch)
            {
                isCrouch = false;
                animator.SetBool("isCrouch", false);
            }
            else
            {
                isCrouch = true;
                animator.SetBool("isCrouch", true);
            }
        }
    }

    public void Interact()
    {
        tempInteractorController.OnInteract(this);
    }
}