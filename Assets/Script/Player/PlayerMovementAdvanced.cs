using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAdvanced : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public float baseFOV;
    public float sprintFOV;
    public float FOVTransitionSpeed = 10f;  // New variable for transition speed
    public Camera cameraMain;

    [Header("Jumping")]
    public float jumpHeight = 1.0f;
    public bool isJumping;
    public float airMultiplier;


    [Header("Grounded check parameters:")]
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float rayDistance = 1;
    [field: SerializeField]
    public bool IsGrounded { get; private set; }

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    [SerializeField]

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;

        cameraMain.fieldOfView = baseFOV;
    }

    private void Update()
    {
        // ground check
        MyInput();
        SpeedControl();
        StateHandler();

        IsGrounded = Physics.Raycast(transform.position, Vector3.down, rayDistance, groundMask);

        // Handle FOV transition
        float targetFOV = (state == MovementState.sprinting && horizontalInput != 0f || state == MovementState.sprinting && verticalInput != 0f) ? sprintFOV : baseFOV;
        cameraMain.fieldOfView = Mathf.Lerp(cameraMain.fieldOfView, targetFOV, FOVTransitionSpeed * Time.deltaTime);


    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        isJumping = Input.GetKey(jumpKey);

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        // Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (isJumping && IsGrounded)
            AddJumpForce();


        if (IsGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!IsGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);


    }

    private void SpeedControl()
    {
        // limiting speed on slope
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

  

    private void AddJumpForce()
    {
        rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
    }

    

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector3.down * rayDistance);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}
