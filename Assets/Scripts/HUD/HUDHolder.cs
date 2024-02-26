using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    //[SerializeField] private TextMeshProUGUI shieldText;
    [SerializeField] private TextMeshProUGUI invaderCountText;

    //[SerializeField] private Animator helathTextAnimator;
    //[SerializeField] private Animator shieldTextAnimator;
    //[SerializeField] private Animator invaderTextAnimator;
    //[SerializeField] private Color warningColor;

    //[SerializeField] private Transform head;
    //[SerializeField] private float distanceFromCamera = 2;

    //[SerializeField] private float springStiffness = 100;
    //[SerializeField] private float dampingRatio = 0.7f;

    //[SerializeField] private float hudXOffset = 0;
    //[SerializeField] private float hudYOffset = 0;

    public int invaderCounter = 0;

    //private Vector3 targetPosition;
    //private Vector3 velocity;

    //public Color originalColor;

    private void Awake()
    {
        //originalColor = healthText.color;
    }

    private void OnEnable()
    {
        InvaderDamage.InvaderDestroyed += IncreaseInvaderCounter;
        //Absorber.ShieldCharging += UpdateShieldCounter;
        PlayerHealthController.HealthModified += UpdateHealthCounter;
    }

    private void OnDisable()
    {
        InvaderDamage.InvaderDestroyed -= IncreaseInvaderCounter;
        //Absorber.ShieldCharging -= UpdateShieldCounter;
        PlayerHealthController.HealthModified -= UpdateHealthCounter;
    }

    void Start()
    {
        //transform.position = head.position;
        UpdateInvaderCounter();
        UpdateHealthCounter(100);
    }

    void Update()
    {
        SetHudPosition();
    }

    private void IncreaseInvaderCounter()
    {
        invaderCounter++;
        UpdateInvaderCounter();
    }

    private void UpdateInvaderCounter()
    {
        invaderCountText.SetText(invaderCounter.ToString());
        //invaderTextAnimator.SetTrigger("highlight");
    }

    private void UpdateHealthCounter(int amount)
    {
        //helathTextAnimator.SetTrigger("highlight");
        healthText.SetText(amount.ToString());
        //healthText.color = amount <= 25 ? warningColor : originalColor;
    }

    private void UpdateShieldCounter(float amount)
    {
        //shieldText.SetText(amount.ToString("F0") + " %");
        //shieldTextAnimator.SetTrigger("highlight");
    }

    private void SetHudPosition()
    {
        //Vector3 targetOffset = head.up * hudYOffset + head.right * hudXOffset;
        //targetPosition = head.position + head.forward * distanceFromCamera + targetOffset;

        //float deltaTime = Time.deltaTime;
        //Vector3 displacement = targetPosition - transform.position;
        //Vector3 springForce = springStiffness * displacement;
        //Vector3 dampingForce = -2.0f * Mathf.Sqrt(springStiffness) * dampingRatio * velocity;
        //Vector3 totalForce = springForce + dampingForce;
        //Vector3 acceleration = totalForce / transform.localScale.x;
        //velocity += acceleration * deltaTime;
        //transform.position += velocity * deltaTime;

        //transform.LookAt(head);
        //transform.rotation *= Quaternion.Euler(0, 180, 0);
    }
}
