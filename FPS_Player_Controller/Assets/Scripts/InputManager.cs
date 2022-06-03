using System;
using SkalluUtils.PropertyAttributes;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _self;
    public static InputManager self => _self;
    
    #region KEYCODES
    [Header("Key binds")]
    public KeyCode sprintButton;
    public KeyCode jumpButton;
    public KeyCode actionButton;
    public KeyCode dropWeaponButton;
    public KeyCode shootButton;
    #endregion

    #region INPUT VALUES
    [Header("Input values")] 
    [ReadOnlyInspector] public float horizontalMovementInput;
    [ReadOnlyInspector] public float verticalMovementInput;
    
    [Space]
    [ReadOnlyInspector] public float mouseXInput;
    [ReadOnlyInspector] public float mouseYInput;
    #endregion

    [HideInInspector] public bool isMoving;

    private void Awake()
    {
        if (_self != null && _self != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            _self = this;
        }
    }

    private void Start()
    {
        // set default values for those keybinds that do not have any value set
        if (sprintButton is KeyCode.None) sprintButton = KeyCode.LeftShift;
        if (jumpButton is KeyCode.None) jumpButton = KeyCode.Space;
        if (actionButton is KeyCode.None) actionButton = KeyCode.E;
        if (dropWeaponButton is KeyCode.None) dropWeaponButton = KeyCode.Q;
        if (shootButton is KeyCode.None) shootButton = KeyCode.Mouse0;
    }

    private void Update()
    {
        // track horizontal and vertical movement input
        horizontalMovementInput = Input.GetAxisRaw("Horizontal");
        verticalMovementInput = Input.GetAxisRaw("Vertical");

        isMoving = Math.Abs(horizontalMovementInput) > 0 || Math.Abs(verticalMovementInput) > 0;

        // track horizontal and vertical mouse input
        mouseXInput = Input.GetAxisRaw("Mouse X");
        mouseYInput = Input.GetAxisRaw("Mouse Y");
    }

    /// <summary>
    /// Checks if provided key is pressed
    /// </summary>
    /// <param name="keyCode"> keycode to check if it is pressed </param>
    /// <returns> bool value that specifies whether key is pressed </returns>
    public static bool KeyPressed(KeyCode keyCode) => Input.GetKeyDown(keyCode);

    /// <summary>
    /// Checks if provided key is hold
    /// </summary>
    /// <param name="keyCode"> keycode to check if it is hold </param>
    /// <returns> bool value that specifies whether key is hold </returns>
    public static bool KeyHold(KeyCode keyCode) => Input.GetKey(keyCode);

    /// <summary>
    /// Checks if provided key is released
    /// </summary>
    /// <param name="keyCode"> keycode to check if it is released </param>
    /// <returns> bool value that specifies whether key is released </returns>
    public static bool KeyReleased(KeyCode keyCode) => Input.GetKeyUp(keyCode);
    
} 