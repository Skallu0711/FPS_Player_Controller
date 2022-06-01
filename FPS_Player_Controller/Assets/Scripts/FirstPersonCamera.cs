using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    private static FirstPersonCamera _self;
    public static FirstPersonCamera self => _self;
    
    private Camera mainCamera;
    private PlayerController playerController;
    private InputManager inputManager;

    [SerializeField] private float xMouseSensitivity;
    [SerializeField] private float yMouseSensitivity;

    private float multiplier = 0.01f;

    private float xRotation;
    private float yRotation;
    
    private void Awake()
    {
        if (_self != null && _self != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _self = this;

            mainCamera = Camera.main;
            
            playerController = PlayerController.self;
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
        
        mainCamera.gameObject.transform.localRotation = Quaternion.Euler(xRotation, 0,0 );
        playerController.gameObject.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void GetInput()
    {
        var mouseXInput = inputManager.mouseXInput;
        var mouseYInput = inputManager.mouseYInput;

        yRotation += mouseXInput * xMouseSensitivity * multiplier;
        xRotation -= mouseYInput * yMouseSensitivity * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90, 60);
    }

} 
