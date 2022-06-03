using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    private static FirstPersonCamera _self;
    public static FirstPersonCamera self => _self;

    private InputManager inputManager;
    
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform cameraPos;
    [SerializeField] private float cameraSensitivity;
    private float multiplier = 0.01f;

    private float verticalRotation, horizontalRotation;
    private float mouseInputX, mouseInputY;

    private void Awake()
    {
        if (_self != null && _self != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _self = this;
            
            inputManager = InputManager.self;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        GetInput();
        Rotate();
    }

    private void GetInput()
    {
        mouseInputX = inputManager.mouseXInput * cameraSensitivity;
        mouseInputY = inputManager.mouseYInput * cameraSensitivity;
    }

    /// Rotates camera along with x and y axes
    private void Rotate()
    {
        // calculate rotation
        horizontalRotation += mouseInputX * multiplier;
        verticalRotation -= mouseInputY * multiplier;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 80f);
        
        // rotate
        cameraPos.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
        orientation.rotation = Quaternion.Euler(0, horizontalRotation, 0);
    }

}