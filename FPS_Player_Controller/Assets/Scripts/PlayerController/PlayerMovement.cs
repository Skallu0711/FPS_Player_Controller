using SkalluUtils.PropertyAttributes;
using UnityEngine;

[RequireComponent(typeof(PlayerController), typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerController playerController;
    private InputManager inputManager;
    private Rigidbody rb;

    [SerializeField] private Transform orientation;
    [Space]

    // ground movement
    private float movementSpeed;
    private float horizontalMovementInput, verticalMovementInput;
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
        playerController = PlayerController.self;
        inputManager = playerController.inputManager;
        
        rb = GetComponent<Rigidbody>();
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
    
    // Gets input from input manager
    private void GetInput()
    {
        horizontalMovementInput = inputManager.horizontalMovementInput;
        verticalMovementInput = inputManager.verticalMovementInput;

        isSprinting = InputManager.KeyHold(inputManager.sprintButton) && onGround;
        
        if (InputManager.KeyPressed(inputManager.jumpButton) && onGround) 
            Jump();
    }
    
    // Moves player character
    private void Move()
    {
        var movementDirection = (orientation.forward * verticalMovementInput + orientation.right * horizontalMovementInput).normalized;
        
        var multiplier = onGround
            ? isSprinting
                ? sprintMultiplier : 1
            : midairMultiplier;

        rb.AddForce(movementDirection * (movementSpeed * 10 * multiplier), ForceMode.Acceleration);
    }
    
    // Performs single jump
    private void Jump()
    {
        var currentVelocity = rb.velocity;
        
        rb.velocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    
    // Determines drag based on whether player is on the ground or in the air
    private void HandleDrag() => rb.drag = onGround ? groundDrag : airDrag;
    
    // Limits velocity taking into account the current movement multiplier
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
