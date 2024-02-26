using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyBullet : MonoBehaviour
{

    [SerializeField] public int bulletRange = 100;
    [SerializeField] private GameObject bulletSparks;

    /// <summary>
    /// Distancia máxima respecto al origen que alcanzará la bala. Sobrepasada esta distancia se autodestuirá.
    /// </summary>
    public int BulletRange
    {
        get => bulletRange;
        set => bulletRange = value;
    }
    void Update()
    {
        // Destruir la bala si su distancia al origen es > 1000 unidades
        if (Vector3.Distance(transform.position, Vector3.zero) > BulletRange)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision) 
    {
        ShowSparks();
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        ShowSparks();
        Destroy(this.gameObject);
    }

    private void ShowSparks()
    {
        GameObject sparks = Instantiate(bulletSparks, transform.position, Quaternion.identity);
        Destroy(sparks, 2f);
    }
}
