using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InvaderSpawner : MonoBehaviour
{
    [SerializeField] private GameObject invaderPrefab;
    [SerializeField] private float startCountDown;
    [SerializeField] private float spawnTimer;

    [SerializeField] private Transform target;
    [SerializeField] private Transform finalDestination;

    [SerializeField] private Transform wayPoints;

    [SerializeField] private GameObject finalPop;

    [SerializeField] private bool showGizmo = true;

    [SerializeField] private MeshRenderer motherShipRend;
    [SerializeField] private MeshRenderer invaderShipRend;
    private Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta };
    private int iColor;

    private bool canSpawn = true;

    public static event Action MotherShipDestroyed;

    void Start()
    {
        spawnTimer = UnityEngine.Random.Range(StatsManager.invader.spawning.minSpawnTime, StatsManager.invader.spawning.minSpawnTime);
        StartCoroutine(SpawnInvader());

        Material mat = motherShipRend.materials[0];
        iColor = UnityEngine.Random.Range(0, 5);
        mat.color = colors[iColor];

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("NukeWave"))
        {
            Defeat();
            Destroy(collision.gameObject, 0.5f);
        }
    }


    private IEnumerator SpawnInvader()
    {
        yield return new WaitForSeconds(startCountDown);
        while (canSpawn)
        {
            GameObject invader = Instantiate(invaderPrefab, transform.position, transform.rotation);
            //invaderShipRend.sharedMaterial.color = colors[iColor];
            invader.GetComponentInChildren<MeshRenderer>().material.color = colors[iColor];

            if (invader.TryGetComponent(out InvaderController invaderController))
            {
                List<Transform> _wayPoints = new List<Transform>();
                foreach (Transform wayPoint in wayPoints)
                {
                    _wayPoints.Add(wayPoint);
                }
                invaderController.SetWaypoints(_wayPoints);
                invaderController.SetTarget(target);
                invaderController.SetFinalDestination(finalDestination);
            }
            yield return new WaitForSeconds(spawnTimer);
        }
    }

    /// <summary>
    /// M�todo para llamar cuando salta el nuke y todas las naves nodrizas
    /// s�n destruidas
    /// </summary>
    private void Defeat()
    {
        canSpawn = false;
        AudioManager.I.PlaySound(SoundName.MothershipCollapsing, transform.position);
        GameObject pop = Instantiate(finalPop, transform.position, Quaternion.identity);
        Destroy(pop, 10f);
        Invoke(nameof(ExplosionSoundAndSend), 5f);
        Destroy(gameObject, 5f);
    }

    private void ExplosionSoundAndSend()
    {
        AudioManager.I.PlaySound(SoundName.MothershipExplosion, transform.position);
        MotherShipDestroyed?.Invoke();
    }

    /// <summary>
    /// M�todo para dibujar el gizmo del spawner y sus waypoints
    /// para facilitar el uso al dise�ador de niveles
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!showGizmo)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 3f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(finalDestination.position, 3f);

        Transform from = null;
        Transform to = null;
        Gizmos.color = Color.magenta;
        foreach (Transform wayPoint in wayPoints)
        {
            if (from is null)
                from = transform;
            else
                from = to;

            to = wayPoint;

            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(to.position, 1f);

            Gizmos.color = Color.white;
            if (from is not null && to is not null)
                Gizmos.DrawLine(from.position, to.position);
        }

        if (to is not null)
        {
            Gizmos.DrawLine(to.position, new Vector3(target.position.x, to.position.y, target.position.z));
            Gizmos.DrawLine(new Vector3(target.position.x, to.position.y, target.position.z), finalDestination.position);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(to.position, 3f);
        }
    }
}
