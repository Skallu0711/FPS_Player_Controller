using System;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class UiController : MonoBehaviour
{
    private static UiController _self;
    public static UiController self => _self;

    private PlayerController playerController;

    [SerializeField] private Image crosshair;
    [SerializeField] private Sprite[] corsshairSprites;
    
    private void Awake()
    {
        if (_self != null && _self != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _self = this;
            
            playerController = PlayerController.self;
        }
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

    private void ChangeCrosshair(int idx)
    {
        if (idx < corsshairSprites.Length)
            crosshair.sprite = corsshairSprites[idx];
    }

    private void ChangeCrosshairColor(Color newColor)
    {
        if (crosshair.color != newColor)
            crosshair.color = newColor;
    }
    
    // Called everytime player picks up triggered weapon
    private void PlayerControllerOnOnItemPickedUp(object sender, EventArgs e) => ChangeCrosshair(1);

    // Called everytime player drops equipped weapon
    private void PlayerControllerOnOnItemDropped(object sender, EventArgs e) => ChangeCrosshair(0);
    
} 