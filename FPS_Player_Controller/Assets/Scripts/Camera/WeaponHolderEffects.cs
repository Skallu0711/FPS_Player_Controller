using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WeaponHolderEffects : MonoBehaviour
{
    private static readonly Vector3 defaultPosition = new Vector3(0.6f, -0.35f, 0.95f);
    private static readonly Vector3 rotationOffset = new Vector3(0, 180, 0);
    
    private static readonly int animatorIsMoving = Animator.StringToHash("isMoving");

    private InputManager inputManager;
    private PlayerCombat playerCombat;
    private CameraRecoil cameraRecoil;
    private Animator animator;
    
    [SerializeField] private float smoothness = 8;
    [SerializeField] private float swaySensitivityX = 10;
    [SerializeField] private float swaySensitivityY = 10;
    
    private void Awake()
    {
        inputManager = InputManager.self;
        playerCombat = PlayerController.self.playerCombat;
        cameraRecoil = playerCombat.cameraRecoil;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        transform.localPosition = defaultPosition;
    }

    private void Update()
    {
        if (!cameraRecoil.recoilActive)
        {
            if (!animator.enabled)
                animator.enabled = true;

            Sway();
            
            if (playerCombat.hasWeapon)
                Bob();
        }
        else
        {
            if (animator.enabled)
                animator.enabled = false;
        }
    }

    private void Sway()
    {
        // get input
        var mouseInputX = inputManager.mouseXInput * swaySensitivityX;
        var mouseInputY = inputManager.mouseYInput * swaySensitivityY;

        // calculate rotation
        var rotationX = Quaternion.AngleAxis(mouseInputY, Vector3.right);
        var rotationY = Quaternion.AngleAxis(-mouseInputX - rotationOffset.y, Vector3.up);
        var targetRotation = rotationX * rotationY;
        
        // rotate
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothness);
    }

    private void Bob() => animator.SetBool(animatorIsMoving, inputManager.isMoving);
    
}