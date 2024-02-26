using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using VolumetricLines;

public class LaserGunShootingSolo : MonoBehaviour
{
    private enum Mode
    {
        Bullets,
        Raycast
    }

    [SerializeField] private InputActionProperty shootAction;

    [SerializeField] private Mode mode;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private VolumetricLineBehavior laserPointer;
    [SerializeField] private float laserPointerStartShotWidth;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem laserImpactPrefab;
    [SerializeField] private float rateOfFire;
    [SerializeField] public float bulletSpeed;

    private float lastShot;

    private float originalPointerLineWidth;
    private Vector3 originalPointerStartPos;
    private LaserGunManager laserGunManager;
    private Vector3 laserImpactPos;
    private bool isRayCastShooting;
    private float pointerInterpolator;


    private void OnEnable()
    {
        shootAction.action.performed += Shoot;
    }

    private void OnDisable()
    {
        shootAction.action.performed -= Shoot;
    }

    private void Start()
    {
        
        laserGunManager = GetComponent<LaserGunManager>();

        originalPointerLineWidth = laserPointer.LineWidth;
        originalPointerStartPos = laserPointer.StartPos;
        isRayCastShooting = false;
        pointerInterpolator = 0;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (mode.Equals(Mode.Bullets))
            laserGunManager.Shooting();
        else
            RayCastShot();
    }

    //void StopShoot(DeactivateEventArgs _)
    //{
    //    if (mode.Equals(Mode.Bullets))
    //        laserGunManager.StopShooting();
    //}

    private void RayCastShot()
    {
        AudioManager.I.PlaySound(SoundName.RaycastShot, shootPoint.position);
        laserImpactPos = new Vector3(0, 1000, 0);
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out RaycastHit hit, Mathf.Infinity))
        {
            laserImpactPos = new Vector3(0, Vector3.Distance(shootPoint.position, hit.point),0);
            ParticleSystem laserImpact = Instantiate(laserImpactPrefab, hit.point, Quaternion.identity);
            Destroy(laserImpact, 2f);

            if (hit.transform.TryGetComponent(out InvaderDamage invader))
            {
                invader.TakeDamage(1);
            }

            if (hit.transform.TryGetComponent(out GameManager gm))
            {
                if (gm.gameObject.TryGetComponent(out MeshRenderer mr))
                {
                    gm.StartGame();
                }
            }
        }

        isRayCastShooting = true;
        laserPointer.LineWidth = laserPointerStartShotWidth;
        laserPointer.StartPos = laserImpactPos;
        pointerInterpolator = 0;

    }

    void Update()
    {

        if (mode.Equals(Mode.Bullets))
        {
            if (!laserGunManager.isHot && laserGunManager.isShooting)
            {
                if ((Time.time - lastShot) > rateOfFire)
                {
                    lastShot = Time.time;

                    muzzleFlash.Play();
                    AudioManager.I.PlaySound(SoundName.LaserShot, transform.position);
                    GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
                    bullet.transform.Rotate(90, 0, 0);
                    bullet.GetComponent<Rigidbody>().AddForce(shootPoint.forward * bulletSpeed, ForceMode.VelocityChange);
                }
            }
        } else if (mode.Equals(Mode.Raycast))
        {
            if (!isRayCastShooting)
            {
                laserPointer.LineWidth = originalPointerLineWidth;
                laserPointer.StartPos = originalPointerStartPos;
                return;
            }

            if (pointerInterpolator < 1)
            {
                laserPointer.LineWidth = Mathf.Lerp(laserPointerStartShotWidth, originalPointerLineWidth, pointerInterpolator);
                pointerInterpolator += Time.deltaTime * 8;
            } else
            {
                isRayCastShooting = false;
                pointerInterpolator = 0;
            }
        }
    }
}
