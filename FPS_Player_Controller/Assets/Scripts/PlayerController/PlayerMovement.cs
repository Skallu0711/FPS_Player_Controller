using SkalluUtils.PropertyAttributes;
using UnityEngine;

[RequireComponent(typeof(PlayerController), typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerController playerController;
    private InputManager inputManager;

    // ground movement
    private float movementSpeed;
    private float horizontalMovementInput;
    private float verticalMovementInput;
    private float groundDrag;
    [SerializeField] [ReadOnlyInspector] private bool onGround;

    // sprinting
    [SerializeField] [ReadOnlyInspector] private bool isSprinting;
    private float sprintMultiplier;

    // jumping and midair movement
    private float jumpForce;
    private float midairMultiplier;
    private float airDrag;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        playerController = PlayerController.self;
        inputManager = InputManager.self;
    }

    private void Start()
    {
        rb.freezeRotation = true;

        // set movement variables
        movementSpeed = playerController.movementSpeed;
        groundDrag = playerController.groundDrag;
        sprintMultiplier = playerController.sprintMultiplier;
        jumpForce = playerController.jumpForce;
        midairMultiplier = playerController.midairMultiplier;
        airDrag = playerController.airDrag;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        onGround = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        
        GetInput();
        HandleDrag();
        LimitVelocity();
    }

    /// <summary>
    /// Gets input from input manager
    /// </summary>
    private void GetInput()
    {
        horizontalMovementInput = inputManager.horizontalMovementInput;
        verticalMovementInput = inputManager.verticalMovementInput;

        isSprinting = inputManager.KeyHold(inputManager.sprintButton) && onGround;
        
        if (inputManager.KeyPressed(inputManager.jumpButton) && onGround) 
            Jump();
    }

    /// <summary>
    /// Moves player character
    /// </summary>
    private void Move()
    {
        var movementDirection = (transform.forward * verticalMovementInput + transform.right * horizontalMovementInput).normalized;
        
        var multiplier = onGround
            ? isSprinting
                ? sprintMultiplier : 1
            : midairMultiplier;

        rb.AddForce(movementDirection * (movementSpeed * 10 * multiplier), ForceMode.Acceleration);
    }

    /// <summary>
    /// Performs single jump
    /// </summary>
    private void Jump()
    {
        var currentVelocity = rb.velocity;
        
        rb.velocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Determines drag based on whether player is on the ground or in the air
    /// </summary>
    private void HandleDrag() => rb.drag = onGround ? groundDrag : airDrag;

    /// <summary>
    /// Limits velocity taking into account the current movement multiplier
    /// </summary>
    private void LimitVelocity()
    {
        var currentVelocity = rb.velocity;

        if (currentVelocity.magnitude > movementSpeed)
        {
            var limitedVelocity = onGround
                ? isSprinting 
                    ? currentVelocity.normalized * (movementSpeed * sprintMultiplier) : currentVelocity.normalized * movementSpeed
                : currentVelocity.normalized * (movementSpeed * midairMultiplier);
            
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

}
