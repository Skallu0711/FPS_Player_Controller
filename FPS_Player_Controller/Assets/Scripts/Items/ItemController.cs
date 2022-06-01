using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ItemController : MonoBehaviour
{
    protected PlayerController playerController;

    [HideInInspector] public Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        playerController = PlayerController.self;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
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
    private void GetPickedUp()
    {
        transform.SetParent(playerController.weaponHolder);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        rb.isKinematic = true;
        col.enabled = false;
    }

    //
    private void GetDropped()
    {
        transform.SetParent(null);

        rb.isKinematic = false;
        col.enabled = true;
    }
    
    // Called everytime player picks up triggered weapon
    private void PlayerControllerOnOnItemPickedUp(object sender, PlayerController.OnItemPickedUpEventArgs e) => GetPickedUp();
    
    // Called everytime player drops equipped weapon
    private void PlayerControllerOnOnItemDropped(object sender, EventArgs e) => GetDropped();
    
} 