using System;
using JetBrains.Annotations;
using SkalluUtils.PropertyAttributes;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerCombat : MonoBehaviour
{
    private PlayerController playerController;
    private InputManager inputManager;
    [HideInInspector] public MuzzleFlash muzzleFlash;
    
    [ReadOnlyInspector] public bool hasWeapon;
    [ReadOnlyInspector] [CanBeNull] public WeaponController equippedWeapon;
    
    private float fullAutoShootTimer = 0; // timer for calculating when to shoot while full autoing
    private float nextShootTime = 0; // prevents from spamming shoot button

    public event EventHandler OnShoot;

    private void Awake()
    {
        playerController = PlayerController.self;
        inputManager = InputManager.self;
        
        muzzleFlash = MuzzleFlash.self;
    }

    private void OnEnable()
    {
        playerController.OnItemPickedUp += PlayerControllerOnOnItemPickedUp;
        playerController.OnItemDropped += PlayerControllerOnOnItemDropped;
    }

    private void Update()
    {
        // shooting
        if (equippedWeapon != null)
        {
            if (Time.time >= nextShootTime && inputManager.KeyPressed(inputManager.shootButton))
            {
                ShootOnce();
                nextShootTime = Time.time + 1f * equippedWeapon.weaponData.ShootRate;
            }

            if (inputManager.KeyHold(inputManager.shootButton) && equippedWeapon.weaponData.CanFullAuto)
                ShootFullAuto();

            if (inputManager.KeyReleased(inputManager.shootButton))
            {
                fullAutoShootTimer = equippedWeapon.weaponData.ShootRate;
                equippedWeapon.HideMuzzleFlash();
            }
        }
    }
    
    private void OnDisable()
    {
        playerController.OnItemPickedUp -= PlayerControllerOnOnItemPickedUp;
        playerController.OnItemDropped -= PlayerControllerOnOnItemDropped;
    }

    // Performs single shot
    private void ShootOnce()
    {
        equippedWeapon.Shoot();
        OnShoot?.Invoke(this, EventArgs.Empty);
    }

    // Full autoing
    private void ShootFullAuto()
    {
        fullAutoShootTimer -= Time.deltaTime;
        
        if (fullAutoShootTimer <= 0)
        {
            ShootOnce();
            fullAutoShootTimer = equippedWeapon.weaponData.ShootRate;
        }
    }

    // Equips triggered weapon
    private void Equip(WeaponController weaponToEquip)
    {
        equippedWeapon = weaponToEquip;
        muzzleFlash.transform.localPosition = equippedWeapon.weaponData.MuzzleEndPosition; // set muzzle flash position to equipped gun's muzzle end
        
        hasWeapon = true;
    }

    // Unequips current weapon
    private void UnEquip()
    {
        equippedWeapon = null;
        hasWeapon = false;
    }

    // Called everytime player picks up triggered weapon
    private void PlayerControllerOnOnItemPickedUp(object sender, PlayerController.OnItemPickedUpEventArgs e)
    {
        Equip(e.pickedUpWeapon);
        fullAutoShootTimer = equippedWeapon.weaponData.ShootRate;
    }

    // Called everytime player drops equipped weapon
    private void PlayerControllerOnOnItemDropped(object sender, EventArgs e)
    {
        muzzleFlash.Hide();
        UnEquip();   
    }
} 
