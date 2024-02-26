using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class InvaderDamage : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI textHealth;

    [SerializeField] private ParticleSystem damageSparks;
    [SerializeField] private GameObject deadExplosion;

    private Rigidbody rb;
    private InvaderController controller;
    private int health;
    private bool isAlive = true;

    public static event Action InvaderDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        controller = GetComponent<InvaderController>();
        health = Random.Range(StatsManager.invader.life.minHitsToDead, StatsManager.invader.life.maxHitsToDead);
        //textHealth.text = health.ToString();
    }

    private void DestroyInvaderBehaviour()
    {
        isAlive = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddTorque(new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2)), ForceMode.VelocityChange);
        rb.AddForce(transform.forward * controller.speed + new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0f), ForceMode.VelocityChange);
        damageSparks.Play();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1);
        }

        if (collision.gameObject.CompareTag("NukeWave"))
        {
            DestroyInvader();
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        //textHealth.text = health.ToString();
        if (health <= 0)
        {
            DestroyInvader();
        }
    }

    private void DestroyInvader() {
        if (!isAlive)
            return;
        InvaderDestroyed?.Invoke();
        DestroyInvaderBehaviour();
        if (TryGetComponent(out InvaderController invader))
        {
            invader.enabled = false;
        }

        Destroy(gameObject, 6f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject explosion = Instantiate(deadExplosion, transform.position, Quaternion.identity);
        AudioManager.I.PlaySound(SoundName.InvaderCrash, transform.position);
        Destroy(explosion, 3f);
        Destroy(gameObject);
    }
}
