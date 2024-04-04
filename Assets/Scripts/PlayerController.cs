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
    [SerializeField] private LayerMask groundMask; // A LayerMask that specifies the layers considered as ground
    [SerializeField] private Animator animator;
    private bool isGrounded;

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
        // Apply movement...
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        // Determine the rotation direction based on second stick input or movement input
        Vector3 intendedDirection = lookInput.magnitude > 0.1f ? new Vector3(lookInput.x, 0, lookInput.y) : moveInput;
        if (intendedDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(intendedDirection);
            rb.rotation = Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}