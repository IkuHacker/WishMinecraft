using UnityEngine;

public class PlayerMovementAdvanced : MonoBehaviour
{

    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    float movementMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Sprinting Effects")]
    [SerializeField] Camera cam;
    [SerializeField] float sprintFOV = 90f;
    [SerializeField] float walkFOV = 60f;
    public float fovTransitionSpeed = 5f; 



    [Header("Jumping")]
    public float jumpForce = 5f;
    public float jumpRate = 15f;

    [Header("Crouching")]
    public float crouchScale = 0.75f;
    public float crouchSpeed = 1f;
    float crouchMultiplier = 5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftControl;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftShift;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    public bool isGrounded { get; private set; }

    public bool isCrouching;
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool isMoving;

    Vector3 moveDirection;

    Rigidbody rb;

    float nextTimeToJump = 0f;
     Animator animator;
    public Animator inventoryPlayerPrevieuw;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        inventoryPlayerPrevieuw = GameObject.FindGameObjectWithTag("Preview").GetComponent<Animator>();


}

private void Update()
    {
        UpdateAnimatorSpeed();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (moveDirection == new Vector3(0f, 0f, 0f))
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        MyInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetKey(jumpKey) && Time.time >= nextTimeToJump)
        {
            nextTimeToJump = Time.time + 1f / jumpRate;
            Jump();

        }

        if (Input.GetKeyDown(crouchKey) && !isCrouching)
        {
            Crouch();
        }
        else if (Input.GetKeyUp(crouchKey) && isCrouching)
        {
            UnCrouch();
        }

        float targetFOV;

        if (isSprinting && isMoving)
        {
            targetFOV = sprintFOV;
        }
        else
        {
            targetFOV = walkFOV;
        }

        // Transition progressive du FOV
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, fovTransitionSpeed * Time.deltaTime);
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    private void UpdateAnimatorSpeed()
    {
        float speed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        animator.SetFloat("speed", speed);
        animator.SetBool("isSprinting", isSprinting);

        inventoryPlayerPrevieuw.SetFloat("speed", speed);
        inventoryPlayerPrevieuw.SetBool("isSprinting", isSprinting);

    }
    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Crouch()
    {
        Vector3 _crouchScale = new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);
        transform.localScale = _crouchScale;

        isCrouching = true;
    }

    void UnCrouch()
    {
        Vector3 normalScale = new Vector3(transform.localScale.x, 1.8f, transform.localScale.z);
        transform.localScale = normalScale;

        isCrouching = false;
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isMoving)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
            isSprinting = true;
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
            isSprinting = false;
        }

        if (Input.GetKey(crouchKey))
        {
            moveSpeed = Mathf.Lerp(moveSpeed, crouchSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
        else if (isCrouching)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * crouchMultiplier, ForceMode.Acceleration);
        }
    }


}
