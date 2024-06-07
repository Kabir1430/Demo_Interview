using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Walking,
        Jumping
    }

    [Header("Movement")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float inputThreshold = 0.1f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private float Rotate_Speed;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float Ray_Jump = 1.1f;

    [Header("Animation")]
    [SerializeField] private Animator Player_Animator;

    [Header("Fps")]
    [SerializeField] private TextMeshProUGUI fpsText; // Reference to a TextMeshPro Text element to display the FPS
    [SerializeField] private float refreshRate = 0.5f;
    [SerializeField] private float timer;
    [SerializeField] private int frameCount;
    [SerializeField] private float totalDeltaTime;

    
    
    [Header("Script")]
    [SerializeField] private Waves Waves_Script;


    [Header("PlayerState")]
    [SerializeField] private PlayerState currentState;

    private void Awake()
    {
        currentState = PlayerState.Idle;
    }

    private void Update()
    {
        Application.targetFrameRate = 60;
        Waves_Script.Timer();//Optimize of Update loop


        Waves_Script.Wave_Timer_Fun();//Optimize of Update loop
        switch (currentState)
        {
            case PlayerState.Idle:
                HandleIdleState();
                break;
            case PlayerState.Walking:
                HandleWalkingState();
                break;
            case PlayerState.Jumping:
                HandleJumpingState();
                break;
        }

        // Draw the ground check ray for debugging
        DrawGroundCheckRay();
        IsGrounded();
        Fps();
    }
        private void HandleIdleState()
    {
        Player_Animator.SetBool("Idle", true);
        Player_Animator.SetBool("Run", false);
        Player_Animator.SetBool("Jump", false);
        if (Mathf.Abs(joystick.Horizontal) > inputThreshold || Mathf.Abs(joystick.Vertical) > inputThreshold)
        {
            TransitionToState(PlayerState.Walking);
        }
        else
        {
            // Zero out horizontal and vertical velocity when idle
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    private void HandleWalkingState()
    {
        Move();
        Player_Animator.SetBool("Run", true);
        Player_Animator.SetBool("Jump", false);
        Player_Animator.SetBool("Idle", false);
        if (Mathf.Abs(joystick.Horizontal) <= inputThreshold && Mathf.Abs(joystick.Vertical) <= inputThreshold)
        {
            TransitionToState(PlayerState.Idle);
        }
    }


    private void HandleJumpingState()
    {
        if (IsGrounded())
        {
            TransitionToState(PlayerState.Idle);
        }
        else
        {
            Player_Animator.SetBool("Jump", true);
            Player_Animator.SetBool("Run", false);
            Player_Animator.SetBool("Idle", false);
        }
    }

    private void Move()
    {
        float moveInputH = joystick.Horizontal;
        float moveInputV = joystick.Vertical;
        Vector3 moveDirection = new Vector3(moveInputH, 0, moveInputV).normalized;

        if (moveDirection.magnitude >= inputThreshold)
        {
            // Rotate the player to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * Rotate_Speed);
        }

        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
    }
    public void Jump()
    {
        if (IsGrounded())
        {
            Player_Animator.SetBool("Jump", true);
            Player_Animator.SetBool("Run", false);
            Player_Animator.SetBool("Idle", false);
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            TransitionToState(PlayerState.Jumping); // Ensure state is set to Jumping
        }
    }

    private void TransitionToState(PlayerState newState)
    {
        currentState = newState;
    }


    private bool IsGrounded()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // Slightly raise the origin to avoid self-intersection
        isGrounded = Physics.Raycast(rayOrigin, Vector3.down, out hit, Ray_Jump, groundLayer);
        return isGrounded;
    }

    private void DrawGroundCheckRay()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        Debug.DrawRay(rayOrigin, Vector3.down * Ray_Jump, isGrounded ? Color.green : Color.red);



    }

    private void Fps()
    {
        frameCount++;
        totalDeltaTime += Time.deltaTime;

        // Check if it's time to update the FPS display
        if (totalDeltaTime >= refreshRate)
        {
            // Calculate FPS
            float fps = frameCount / totalDeltaTime;

            // Update the TextMeshPro Text element
            if (fpsText != null)
            {
                fpsText.text = string.Format("FPS: {0:F2}", fps);
            }

            // Reset counters
            frameCount = 0;
            totalDeltaTime = 0;
        }
    }
}
