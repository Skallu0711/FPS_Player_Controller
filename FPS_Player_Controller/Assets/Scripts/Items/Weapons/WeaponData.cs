using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponData : ItemData
{
    [Serializable]
    public struct Recoil
    {
        [SerializeField] private float recoilX, recoilY, recoilZ;
        public float RecoilX => recoilX;
        public float RecoilY => recoilY;
        public float RecoilZ => recoilZ;

        [SerializeField] private float snappiness;
        public float Snappiness => snappiness;
        
        [SerializeField] private float returnSpeed;
        public float ReturnSpeed => returnSpeed;
    }

    [Header("Weapon data Parameters")]
    
    [SerializeField] private bool canFullAuto = false;
    public bool CanFullAuto => canFullAuto;
    
    [SerializeField] private float shootRate;
    public float ShootRate => shootRate;

    [SerializeField] private int magSize;
    public int MagSize => magSize;

    [SerializeField] private Vector3 muzzleEndPosition;
    public Vector3 MuzzleEndPosition => muzzleEndPosition;

    [Space]
    
    [SerializeField] private Recoil recoilParameters;
    public Recoil RecoilParameters => recoilParameters;
    
}