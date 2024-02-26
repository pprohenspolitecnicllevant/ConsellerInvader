using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private int startingHealth;
    [SerializeField] private int damagePerHit;

    private int currentHealth;

    public static event Action<int> HealthModified;
    public static event Action PlayerDead;
    // Start is called before the first frame update

    [SerializeField] private HUDHolder hudHolder;

    void Start()
    {
        currentHealth = startingHealth;
        HealthModified?.Invoke(currentHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InvaderBullet"))
        {
            DamagePlayer(damagePerHit);
        }
    }

    public void DamagePlayer(int amount)
    {
        currentHealth -= amount;
        AudioManager.I.PlaySound(SoundName.PlayerDamaged);
        HealthModified?.Invoke(currentHealth);

        hudHolder.UpdateHealthCounter(currentHealth);

        if (currentHealth <= 0)
            Dead();
    }

    private void Dead()
    {
        PlayerDead?.Invoke();
    }

}
