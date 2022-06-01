using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MuzzleFlash : MonoBehaviour
{
    private static MuzzleFlash _self;
    public static MuzzleFlash self => _self;
    
    private SpriteRenderer sr;

    private float visibilityTimer = 0;
    private float visibilityMaxTime = 0.1f;
    private bool isVisible = false;

    private void Awake()
    {
        if (_self != null && _self != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _self = this;
            
            sr = GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        if (isVisible)
        {
            visibilityTimer += Time.deltaTime;

            if (visibilityTimer >= visibilityMaxTime)
            {
                Hide();
                visibilityTimer = 0;
            }
        }
    }
    
    // Shows muzzle flash with random rotation
    public void Show()
    {
        if (sr.color.a <= 0)
            sr.color = Color.white;

        transform.Rotate(0, 0, Random.Range(0, 360));

        isVisible = true;
    }
    
    // Hides muzzle flash
    public void Hide()
    {
        if (sr.color.a >= 1)
            sr.color = Color.clear;

        isVisible = false;
    }
    
} 