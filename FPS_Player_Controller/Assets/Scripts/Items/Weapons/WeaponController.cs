using SkalluUtils.PropertyAttributes;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class WeaponController : ItemController
{
    public WeaponData weaponData;

    [ReadOnlyInspector] public int currentAmmo;

    private void Start()
    {
        currentAmmo = weaponData.MagSize;
    }

    public void Shoot()
    {
        if (currentAmmo > 0)
        {
            // var bulletInstance = Instantiate(bulletPrefab, muzzle.transform.position, Quaternion.identity);
            // bulletInstance.moveDirection = -transform.forward;
            
            ShowMuzzleFlash();
            //soundManager.PlaySingleSound(audioSource, weaponData.fireSound); // play shot sound
            
            currentAmmo -= 1;
        }
    }

    private void Reload()
    {
        
    }
    
    public void ShowMuzzleFlash() => playerController.playerCombat.muzzleFlash.Show();
    
    public void HideMuzzleFlash() => playerController.playerCombat.muzzleFlash.Hide();

} 