using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponData : ItemData
{
    [Header("Weapon data Parameters")]
    
    [SerializeField] private bool canFullAuto = false;
    public bool CanFullAuto => canFullAuto;
    
    [SerializeField] private float shootRate;
    public float ShootRate => shootRate;

    [SerializeField] private int magSize;
    public int MagSize => magSize;

    [Space]
    
    [SerializeField] private Vector3 muzzleEndPosition;
    public Vector3 MuzzleEndPosition => muzzleEndPosition;
    
}