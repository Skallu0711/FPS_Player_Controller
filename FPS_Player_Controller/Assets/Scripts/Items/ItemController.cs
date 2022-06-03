using System;
using SkalluUtils.Utils.Sound;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(AudioSource))]
public class ItemController : MonoBehaviour
{
    protected PlayerController playerController;

    [HideInInspector] public Rigidbody rb;
    private Collider col;
    protected AudioSource audioSource;

    protected virtual void Awake()
    {
        playerController = PlayerController.self;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
    }
    
    private void OnEnable()
    {
        playerController.OnItemPickedUp += PlayerControllerOnOnItemPickedUp;
        playerController.OnItemDropped += PlayerControllerOnOnItemDropped;
    }

    private void OnDisable()
    {
        playerController.OnItemPickedUp -= PlayerControllerOnOnItemPickedUp;
        playerController.OnItemDropped -= PlayerControllerOnOnItemDropped;
    }

    // 
    protected virtual void GetPickedUp()
    {
        transform.SetParent(playerController.weaponHolder);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one * 2;

        rb.isKinematic = true;
        col.enabled = false;
    }

    //
    protected virtual void GetDropped()
    {
        transform.SetParent(null);
        transform.localScale = Vector3.one * 2;

        rb.isKinematic = false;
        col.enabled = true;
    }

    // Called everytime player picks up triggered weapon
    private void PlayerControllerOnOnItemPickedUp(object sender, PlayerController.OnItemPickedUpEventArgs e) => GetPickedUp();
    
    // Called everytime player drops equipped weapon
    private void PlayerControllerOnOnItemDropped(object sender, EventArgs e) => GetDropped();
    
} 