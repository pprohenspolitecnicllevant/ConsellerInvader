using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorberSolo : MonoBehaviour
{
    [SerializeField] private ParticleSystem absorbEffect;
    [SerializeField] private Renderer vidreRenderer;
    [SerializeField] private Color blinkColor;

    [SerializeField] private float amountOfEmisionPerHit;
    [SerializeField] private float maxPointsToGetCharged;

    public static event Action<float> ShieldCharging;

    private float chargePoints = 0;
    private Color baseColor;
    private Color alteredColor;
    private float linearAmount = 0;

    private bool isCharged = false;
    private BoxCollider thisCollider;

    private void OnEnable()
    {
        //InvaderDamage.InvaderDestroyed += ChargePerHit;
    }

    private void OnDisable()
    {
        //InvaderDamage.InvaderDestroyed -= ChargePerHit;
    }

    private void Start()
    {

        baseColor = vidreRenderer.material.GetColor("_EmissionColor");
        thisCollider = GetComponent<BoxCollider>();



        ShieldCharging?.Invoke(chargePoints / maxPointsToGetCharged * 100);
    }

    private void Update()
    {
        isCharged = chargePoints >= maxPointsToGetCharged;

        if (isCharged)
            Blinking();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("InvaderBullet") && other.gameObject.TryGetComponent(out Rigidbody rb))
        {
            absorbEffect.transform.position = thisCollider.ClosestPointOnBounds(other.transform.position);
            absorbEffect.Play();
            Destroy(other.gameObject);

            //ChargePerHit();
        }
    }

    private void ChargePerHit()
    {
        chargePoints++;
        if (chargePoints > maxPointsToGetCharged)
            chargePoints = maxPointsToGetCharged;

        if (!isCharged)
            IncreaseEmision();

        ShieldCharging?.Invoke(chargePoints / maxPointsToGetCharged * 100);

        StartCoroutine(ChargingBlink());
    }

    private void IncreaseEmision()
    {
        linearAmount = chargePoints * amountOfEmisionPerHit;
        alteredColor = baseColor * Mathf.LinearToGammaSpace(linearAmount);
        vidreRenderer.material.SetColor("_EmissionColor", alteredColor);
    }

    private void Blinking()
    {
        AudioManager.I.PlaySound(SoundName.UltimateCharged, transform.position);
        Color targetBlinkColor = blinkColor * Mathf.LinearToGammaSpace(linearAmount);
        Color lerpColor = Color.Lerp(alteredColor, targetBlinkColor, Mathf.PingPong(Time.time * 5, 1));
        vidreRenderer.material.SetColor("_EmissionColor", lerpColor);
    }
    private IEnumerator ChargingBlink()
    {
        float initialTime = Time.time;
        float interpolator = 0;
        Color targetBlinkColor = Color.white * Mathf.LinearToGammaSpace(linearAmount);
        while (interpolator <= 1)
        {

            interpolator += (Time.time - initialTime) * 2;

            Color lerpColor = Color.Lerp(alteredColor, targetBlinkColor, interpolator);
            vidreRenderer.material.SetColor("_EmissionColor", lerpColor);
            yield return null;
        }

        initialTime = Time.time;
        while (interpolator >= 0)
        {

            interpolator -= (Time.time - initialTime) * 2;

            Color lerpColor = Color.Lerp(alteredColor, targetBlinkColor, interpolator);
            vidreRenderer.material.SetColor("_EmissionColor", lerpColor);
            yield return null;
        }
    }
}
