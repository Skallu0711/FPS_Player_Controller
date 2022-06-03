using System.Collections;
using System.Transactions;
using SkalluUtils.PropertyAttributes;
using SkalluUtils.Utils.Sound;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(AudioSource))]
public class WeaponController : ItemController
{
    private MuzzleFlash muzzleFlash;
    
    public WeaponData weaponData;
    [ReadOnlyInspector] public int currentAmmo;

    [SerializeField] private Transform end;

    private Camera cam;

    protected override void Awake()
    {
        base.Awake();
        muzzleFlash = MuzzleFlash.self;
        cam = Camera.main;
    }

    private void Start()
    {
        currentAmmo = weaponData.MagSize;
    }
    
    //private Vector3 lookDir = Vector3.zero;

    public void Shoot()
    {
        if (currentAmmo > 0)
        {
            var lookDir = Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit lookPointHit, 20)
                ? (lookPointHit.point - end.position).normalized 
                : cam.transform.forward;
            
            if (Physics.Raycast(end.position, lookDir, out RaycastHit hit))
            {
                if (hit.collider != null)
                {
                    TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, hit));
                }
            }

            muzzleFlash.Show();
            SoundUtils.PlaySingleSound(audioSource, weaponData.Sounds[2]); // play shot sound
            
            currentAmmo -= 1;
        }
    }

    public void Reload()
    {
        currentAmmo = weaponData.MagSize;
    }

    protected override void GetPickedUp()
    {
        base.GetPickedUp();
        SoundUtils.PlaySingleSound(audioSource, weaponData.Sounds[0]);
        
        muzzleFlash.transform.localPosition = weaponData.MuzzleEndPosition; // set muzzle flash position to equipped gun's muzzle end
    }

    protected override void GetDropped()
    {
        base.GetDropped();
        SoundUtils.PlaySingleSound(audioSource, weaponData.Sounds[1]);
        
        muzzleFlash.Hide();
    }

    public Transform bulletSpawnPoint;
    public ParticleSystem impactParticleSystem;
    public TrailRenderer bulletTrail;

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        var time = 0f;
        var startPos = trail.transform.position;

        while (time < 0)
        {
            trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;
        Instantiate(impactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
        
        Destroy(trail.gameObject, trail.time);
    }

} 