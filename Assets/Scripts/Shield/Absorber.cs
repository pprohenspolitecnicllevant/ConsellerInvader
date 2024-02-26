using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VolumetricLines;

public class Absorber : MonoBehaviour
{
    [SerializeField] private ParticleSystem absorbEffect;
    [SerializeField] private Renderer vidreRenderer;
    [SerializeField] private Color blinkColor;

    [SerializeField] private float amountOfEmisionPerHit;
    [SerializeField] private float maxPointsToGetCharged;

    [SerializeField] private VolumetricLineBehavior pointer;
    [SerializeField] private Color targetPointerColor;
    [SerializeField] private Transform ultimatePosition;
    [SerializeField] private GameObject ultimatePrefab;
    [SerializeField] private float ultimateSpeed;

    public static event Action<float> ShieldCharging;

    private float chargePoints = 0;
    private Color baseColor;
    private Color alteredColor;
    private float linearAmount = 0;

    private bool isCharged = false;
    private BoxCollider thisCollider;

    private float originalPointerLineWidth;
    private Color originalPointerColor;

    private void OnEnable()
    {
        InvaderDamage.InvaderDestroyed += ChargePerHit;
    }

    private void OnDisable()
    {
        InvaderDamage.InvaderDestroyed -= ChargePerHit;
    }

    private void Start()
    {
        if (gameObject.TryGetComponent(out XRGrabInteractable grabbableGun))
        {
            grabbableGun.activated.AddListener(Ultimate);
        }

        baseColor = vidreRenderer.material.GetColor("_EmissionColor");
        thisCollider = GetComponent<BoxCollider>();

        originalPointerLineWidth = pointer.LineWidth;
        originalPointerColor = pointer.LineColor;
        pointer.gameObject.SetActive(false);

        ShieldCharging?.Invoke(chargePoints / maxPointsToGetCharged * 100);
    }

    private void Update()
    {
        isCharged = chargePoints >= maxPointsToGetCharged;
        pointer.gameObject.SetActive(isCharged);
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

        pointer.LineWidth = originalPointerLineWidth;
        pointer.LineColor = originalPointerColor;
        if (Physics.Raycast(ultimatePosition.position, ultimatePosition.forward, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("Mothership"))
                PointerTargetEffect();
        }
    }

    private void PointerTargetEffect()
    {
        pointer.LineColor = targetPointerColor;
        pointer.LineWidth = Mathf.Lerp(originalPointerLineWidth, 2.5f, Mathf.PingPong(Time.time * 8, 1));
    }

    private void Ultimate(ActivateEventArgs _)
    {
        if (!isCharged)
            return;

        chargePoints = 0;
        isCharged = false;
        vidreRenderer.material.SetColor("_EmissionColor", baseColor);
        ShieldCharging?.Invoke(0f);

        GameObject ultimate = Instantiate(ultimatePrefab, ultimatePosition.position, ultimatePosition.rotation);
        if (ultimate.TryGetComponent(out Rigidbody rb))
        {
            rb.AddForce(rb.transform.forward * ultimateSpeed, ForceMode.VelocityChange);
            AudioManager.I.PlaySound(SoundName.UltimateRelease, transform.position);
        }

        Destroy(ultimate, 10f);
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
