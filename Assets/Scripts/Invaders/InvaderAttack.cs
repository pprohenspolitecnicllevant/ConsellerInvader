using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderAttack : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletVelocity;
    [SerializeField] private float attackTimer;
    [SerializeField] private int remainingShots;

    private InvaderController invaderController;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = Random.Range(StatsManager.invader.shooting.minShootTimer, StatsManager.invader.shooting.maxShootTimer);
        remainingShots = Random.Range(StatsManager.invader.shooting.minShots, StatsManager.invader.shooting.maxShots + 1);
        invaderController = GetComponent<InvaderController>();
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        while (gameObject.activeSelf)
        {
            PerformShoot();
            yield return new WaitForSeconds(attackTimer);
        }
    }

    private void PerformShoot()
    {
        if (invaderController is null || !CanAttack())
            return;

        Vector3 targetDirection = invaderController.target.position - shootPoint.position;

        AudioManager.I.PlaySound(SoundName.InvaderShot, shootPoint.position);
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.transform.LookAt(invaderController.target);
        Destroy(bullet, 5f);
        if (bullet.TryGetComponent(out Rigidbody rb))
        {
            rb.AddForce(targetDirection.normalized * bulletVelocity, ForceMode.VelocityChange);
            remainingShots--;
        }
    }

    private bool CanAttack()
    {
        if (invaderController is null)
            return false;

        return invaderController.isGoingToPlayer && remainingShots > 0;
    }
}
