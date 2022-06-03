using System;
using JetBrains.Annotations;
using SkalluUtils.PropertyAttributes;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerCombat : MonoBehaviour
{
    private PlayerController playerController;
    private InputManager inputManager;
    
    public CameraRecoil cameraRecoil;
    
    [Space]
    [ReadOnlyInspector] public bool hasWeapon;
    [ReadOnlyInspector] [CanBeNull] public WeaponController equippedWeapon;
    
    private float fullAutoShootTimer = 0; // timer for calculating when to shoot while full autoing
    private float nextShootTime = 0; // prevents from spamming shoot button

    public event EventHandler OnShoot;

    private void Awake()
    {
        playerController = PlayerController.self;
        inputManager = playerController.inputManager;
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
            if (Time.time >= nextShootTime && InputManager.KeyPressed(inputManager.shootButton))
            {
                ShootOnce();
                nextShootTime = Time.time + 1f * equippedWeapon.weaponData.ShootRate;
            }

            if (InputManager.KeyHold(inputManager.shootButton) && equippedWeapon.weaponData.CanFullAuto)
                ShootFullAuto();

            if (InputManager.KeyReleased(inputManager.shootButton))
            {
                fullAutoShootTimer = equippedWeapon.weaponData.ShootRate;
                cameraRecoil.StopRecoil();
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
        if (equippedWeapon.currentAmmo > 0)
        {
            equippedWeapon.Shoot();
            
            cameraRecoil.StartRecoil();
            OnShoot?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            cameraRecoil.StopRecoil();
        }
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

    // Equips weapon provided as parameter
    private void Equip(WeaponController weaponToEquip)
    {
        equippedWeapon = weaponToEquip;
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

        var recoilParameters = equippedWeapon.weaponData.RecoilParameters;
        cameraRecoil.SetRecoil(recoilParameters.RecoilX, recoilParameters.RecoilY, recoilParameters.RecoilZ, recoilParameters.Snappiness, recoilParameters.ReturnSpeed);
    }

    // Called everytime player drops equipped weapon
    private void PlayerControllerOnOnItemDropped(object sender, EventArgs e)
    {
        cameraRecoil.StopRecoil();
        UnEquip();   
    }
    
}