using System.Collections;
using UnityEngine;

public class InvaderAutoAttack : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetOffset;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletVelocity;
    [SerializeField] private float attackTimer;

    private Vector3 finalTargetPosition;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = Random.Range(StatsManager.invader.shooting.minShootTimer, StatsManager.invader.shooting.maxShootTimer);
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
        finalTargetPosition = target.position + targetOffset;

        Vector3 targetDirection = finalTargetPosition - shootPoint.position;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.transform.LookAt(finalTargetPosition);
        Destroy(bullet, 5f);
        if (bullet.TryGetComponent(out Rigidbody rb))
        {
            rb.AddForce(targetDirection.normalized * bulletVelocity, ForceMode.VelocityChange);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(target.position + targetOffset, .4f);
    }
}
