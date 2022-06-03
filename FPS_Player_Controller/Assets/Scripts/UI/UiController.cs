using System;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class UiController : MonoBehaviour
{
    private static UiController _self;
    public static UiController self => _self;

    private PlayerController playerController;
    private PlayerCombat playerCombat;

    [SerializeField] private Image crosshair;
    [SerializeField] private Sprite[] corsshairSprites;

    [SerializeField] private TextMeshProUGUI weaponNameTmp, ammoTmp;

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
            playerCombat = playerController.playerCombat;
        }
    }
    
    private void OnEnable()
    {
        playerController.OnItemPickedUp += PlayerControllerOnOnItemPickedUp;
        playerController.OnItemDropped += PlayerControllerOnOnItemDropped;
        playerCombat.OnShoot += PlayerCombatOnShoot;
    }

    private void Start()
    {
        weaponNameTmp.SetText("");
        ammoTmp.SetText("");
    }

    private void OnDisable()
    {
        playerController.OnItemPickedUp -= PlayerControllerOnOnItemPickedUp;
        playerController.OnItemDropped -= PlayerControllerOnOnItemDropped;
        playerCombat.OnShoot -= PlayerCombatOnShoot;
    }

    // Called everytime player picks up triggered weapon
    private void PlayerControllerOnOnItemPickedUp(object sender, PlayerController.OnItemPickedUpEventArgs e)
    {
        crosshair.sprite = corsshairSprites[1];

        // show
        var weapon = playerCombat.equippedWeapon;
        weaponNameTmp.SetText(weapon.weaponData.ItemName);
        ammoTmp.SetText(weapon.currentAmmo + "/" + weapon.weaponData.MagSize);
    }

    // Called everytime player drops equipped weapon
    private void PlayerControllerOnOnItemDropped(object sender, EventArgs e)
    {
        crosshair.sprite = corsshairSprites[0];
        
        // hide
        weaponNameTmp.SetText("");
        ammoTmp.SetText("");
    }
    
    // Called everytime player shoots a gun
    private void PlayerCombatOnShoot(object sender, EventArgs e)
    {
        var weapon = playerCombat.equippedWeapon;
        ammoTmp.SetText(weapon.currentAmmo + "/" + weapon.weaponData.MagSize);
    }
    
} 