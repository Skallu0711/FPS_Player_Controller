using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _self;
    public static PlayerController self => _self;
    
    [HideInInspector] public PlayerMovement playerMovement;

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
        }
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    
} 
