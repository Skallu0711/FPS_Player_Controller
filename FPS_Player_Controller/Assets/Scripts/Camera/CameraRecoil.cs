using SkalluUtils.PropertyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraRecoil : MonoBehaviour
{
    [SerializeField] [ReadOnlyInspector] private float recoilX, recoilY, recoilZ;
    [Space]
    [SerializeField] [ReadOnlyInspector] private float snappiness;
    [SerializeField] [ReadOnlyInspector] private float returnSpeed;

    [HideInInspector] public bool recoilActive = false;

    private Vector3 currentRotation, targetRotation;

    private void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnSpeed);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, Time.fixedDeltaTime * snappiness);
        
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    //
    public void StartRecoil()
    {
        recoilActive = true;
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }

    //
    public void StopRecoil() => recoilActive = false;

    //
    public void SetRecoil(float newX, float newY, float newZ, float newSnappiness, float newReturnSpeed)
    {
        recoilX = newX;
        recoilY = newY;
        recoilZ = newZ;
        snappiness = newSnappiness;
        returnSpeed = newReturnSpeed;
    }

}