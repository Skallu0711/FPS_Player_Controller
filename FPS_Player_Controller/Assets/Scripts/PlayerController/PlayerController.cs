using System;
using JetBrains.Annotations;
using SkalluUtils.PropertyAttributes;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _self;
    public static PlayerController self => _self;
    
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public PlayerCombat playerCombat;
    [HideInInspector] public InputManager inputManager;
    private Camera mainCamera;

    #region MOVEMENT PARAMETERS
    [Header("Movement parameters")]
    public float movementSpeed = 6;
    public float groundDrag = 6;
    public float sprintMultiplier = 1.5f;
    
    [Space]
    public float jumpForce = 7;
    public float midairMultiplier = 0.5f;
    public float airDrag = 2;
    #endregion

    [Space]
    [SerializeField] private float pickUpRange = 1;
    [SerializeField] private LayerMask itemsLayer;
    [SerializeField] [ReadOnlyInspector] [CanBeNull] private WeaponController triggeredWeapon;
    [HideInInspector] public Vector3 lookPosition;
    [HideInInspector] public Transform weaponHolder;

    public class OnItemPickedUpEventArgs: EventArgs { public WeaponController pickedUpWeapon; }
    public event EventHandler<OnItemPickedUpEventArgs> OnItemPickedUp;
    public event EventHandler OnItemDropped;

    private void Awake()
    {
        if (_self != null && _self != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _self = this;

            playerMovement = GetComponent<PlayerMovement>();
            playerCombat = GetComponent<PlayerCombat>();
            inputManager = InputManager.self;
            mainCamera = Camera.main;
            
            weaponHolder = GameObject.FindGameObjectWithTag("WeaponHolder").transform;
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        SearchForItems();
        
        //
        if (InputManager.KeyPressed(inputManager.actionButton) && triggeredWeapon != null)
        {
            if (playerCombat.hasWeapon)
                Drop(playerCombat.equippedWeapon);

            PickUp(triggeredWeapon);
        }
        
        //
        if (InputManager.KeyPressed(inputManager.dropWeaponButton) && playerCombat.hasWeapon)
            Drop(playerCombat.equippedWeapon);
    }
    
    private void SearchForItems()
    {
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, pickUpRange, itemsLayer))
        {
            if (hit.collider != null)
            {
                var collidedObject = hit.collider.gameObject;
                
                if (collidedObject.CompareTag("Weapon") && collidedObject.TryGetComponent(out WeaponController weaponController))
                {
                    triggeredWeapon = weaponController;
                }

                lookPosition = hit.point;
            }
        }
        else
        {
            triggeredWeapon = null;
        }
        
    }

    private void PickUp(WeaponController itemToPickUp)
    {
        if (!playerCombat.hasWeapon)
            OnItemPickedUp?.Invoke(this, new OnItemPickedUpEventArgs{pickedUpWeapon = triggeredWeapon});
    }

    private void Drop(WeaponController itemToDrop)
    {
        if (playerCombat.hasWeapon)
        {
            OnItemDropped?.Invoke(this, EventArgs.Empty);
            
            itemToDrop.rb.AddForce(mainCamera.transform.forward * 5f, ForceMode.Impulse);
        }
    }


} 
