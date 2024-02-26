using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MagnetGunShooting : MonoBehaviour
{
    
    private MagnetGunManager magnetGunManager;

    private void Start()
    {
        XRGrabInteractable grabbableGun = GetComponent<XRGrabInteractable>();
        grabbableGun.activated.AddListener(Shoot);
        grabbableGun.deactivated.AddListener(StopShoot);

        magnetGunManager = GetComponent<MagnetGunManager>();
    }

    void Shoot(ActivateEventArgs _)
    {
        magnetGunManager.Shooting();
    }

    void StopShoot(DeactivateEventArgs _)
    { 
        magnetGunManager.StopShooting();
    }
}
