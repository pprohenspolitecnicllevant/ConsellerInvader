using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShootController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float bulletVelocity;


    // No volem disparar quan accionen un trigger o un botó, sinó subscriure'ns a
    //l'event "activate" (gatillo) d'un objecte grabbable...

    //[SerializeField] private InputAction shootAction;
    //[SerializeField] private InputActionProperty shootActionProp;

    //private void OnEnable()
    //{
    //    shootAction.performed += Shoot;
    //}

    //private void OnDisable()
    //{
    //    shootAction.performed -= Shoot;
    //}
    private void Start()
    {
        XRGrabInteractable grabbablePistol = GetComponent<XRGrabInteractable>();
        grabbablePistol.activated.AddListener(Shoot);
    }

    private void Shoot(ActivateEventArgs args)
    {
        //AudioManager.I.PlaySound(SoundName.PistolShot, shootPoint.position);
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        if (bullet.TryGetComponent(out Rigidbody rb))
        {
            rb.AddForce(bullet.transform.forward * bulletVelocity, ForceMode.VelocityChange);
        }
        Destroy(bullet, 5f);
    }

}
