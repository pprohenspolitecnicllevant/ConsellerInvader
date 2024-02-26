using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class MagnetGunManager : MonoBehaviour {
    public GameObject tempSensors;
    Animator anim;
    
    private AudioSource audioSource;
    public AudioClip audioShoot;
    public AudioClip audioTemp;
    
    public bool isShooting = false;
    public bool isHot = false;
    
    [SerializeField] private float timeCounterDuration;
    [SerializeField] private float coolSpeed;
    [SerializeField] private float heatSpeed;

    public GameObject smokeParticles;

    [SerializeField] private float magnetRange; // The max distance that the magnet gun can attract something
    [SerializeField] private float magnetStrength; // The strength of the magnetic force
    [SerializeField] private float magnetDistance; // The distance kept between the atracted object and the magnetPoint
    [SerializeField] private Transform magnetPoint; // The point from the distance with attracted objects is calculated
    [SerializeField] private LayerMask magnetLayer; // The layer that the magnetic field source is on
    [SerializeField] private bool attractByForce = false;
    private Attractable attractedObject;
    private float originalAttractedObjectDrag;
    
    public float moveSpeed = 10.0f;
    private float elapsedTime = 0.0f;

    Color newColor;
    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;

    void Start() {
        timeCounterDuration = StatsManager.player.magnetGun.timeCounterDuration;
        coolSpeed = StatsManager.player.magnetGun.coolSpeed;
        heatSpeed = StatsManager.player.magnetGun.heatSpeed;
        magnetStrength = StatsManager.player.magnetGun.magnetStrength;
        magnetDistance = StatsManager.player.magnetGun.magnetStopDistance;
        magnetRange = StatsManager.player.magnetGun.magnetRange;

        attractedObject = null;

        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        smokeParticles.SetActive(false);

        Gradient();
    }

    public void Shooting() {
        anim.SetBool("isPullingTrigger", true);
        if (!isShooting) {
            isShooting = true;
            audioSource.clip = audioShoot;
            audioSource.Play();
        }
    }

    public void StopShooting() {
        anim.SetBool("isPullingTrigger", false);
        if (isShooting) {
            isShooting = false;
            audioSource.Stop();
        }

        ReleaseAttractedObject();
    }

    private void ReleaseAttractedObject()
    {
        if (attractedObject is not null)
        {
            attractedObject.rb.isKinematic = false;
            attractedObject.rb.drag = originalAttractedObjectDrag;
            attractedObject = null;
        }
    }

    void Update() {
        if (isShooting && !isHot) {
            if (elapsedTime <= timeCounterDuration) {
                elapsedTime += Time.deltaTime * heatSpeed;
                Shoot();
            } else {
                audioSource.Stop();
                isHot = true;
                smokeParticles.SetActive(true);
                ReleaseAttractedObject();
            }
        } else if (!isShooting) {
            if (elapsedTime > 0) {
                elapsedTime -= Time.deltaTime * coolSpeed;
                if (elapsedTime < 0)
                    elapsedTime = 0;                
            } else {
                newColor = Color.green;
                newColor.a = 0.5f + (elapsedTime/timeCounterDuration);
                SetTempSensorsColor(newColor);
                audioSource.Stop();
                smokeParticles.SetActive(false);
                isHot = false;
            }
        }
        if (!isHot) {
            //counter.text = "" + (int) (elapsedTime*10) + "%";
            newColor = gradient.Evaluate(elapsedTime/timeCounterDuration);
            newColor.a = 0.5f + (elapsedTime/timeCounterDuration);
            SetTempSensorsColor(newColor);
            //panel.color = newColor; 
        } else {
            audioSource.clip = audioTemp;
            if (!audioSource.isPlaying)
                audioSource.Play();
            //counter.text = "HOT!!";
            newColor = gradient.Evaluate(elapsedTime/timeCounterDuration);
            newColor.a = 10f;
            SetTempSensorsColor(newColor);
            //panel.color = newColor; 
        }
    }

    private void FixedUpdate()
    {
        if (attractedObject is not null)
        {
            Vector3 targetPoint = magnetPoint.position + magnetPoint.forward * magnetDistance;
            Vector3 direction = targetPoint - attractedObject.transform.position;

            float distance = Vector3.Distance(attractedObject.transform.position, targetPoint);

            if (attractByForce)
            {
                attractedObject.rb.isKinematic = false;
                attractedObject.rb.drag = 10;

                float forceModifier = Mathf.Log(distance + 1, 2);
                attractedObject.rb.AddForce(direction.normalized * magnetStrength * forceModifier, ForceMode.Acceleration);
            }
            else
            {
                float forceModifier = Mathf.Log10(distance + 1);
                attractedObject.rb.isKinematic = true;
                //if (distance > 0.5f)
                    attractedObject.rb.MovePosition(attractedObject.transform.position + direction.normalized * magnetStrength/2 * forceModifier * Time.deltaTime);
                //else
                    //attractedObject.transform.position = targetPoint;
            }
        }
    }

    void Gradient() {
        gradient = new Gradient();

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[2];
        colorKey[0].color = Color.green;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.red;
        colorKey[1].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);
    }

     private void OnEnable() {
        // Enable mouse input
        InputSystem.EnableDevice(Mouse.current);
    }

    private void OnDisable() {
        // Disable mouse input
        InputSystem.DisableDevice(Mouse.current);
    }

    void SetTempSensorsColor(Color newColor) {
        foreach (Transform t in tempSensors.transform) {
            Material material = t.gameObject.GetComponent<Renderer>().material;
            material.SetColor("_EmissionColor", newColor);
            //material.color = newColor;
        } 
    }

    void Shoot() {
        if (attractedObject is null && Physics.Raycast(transform.position, transform.right, out RaycastHit hit, magnetRange, magnetLayer)) {
            //Debug.DrawRay(transform.position, transform.right * hit.distance, Color.green);
            //Vector3 currentMousePosition = Input.mousePosition;
            //Vector3 mouseDelta = currentMousePosition - lastMousePosition;
            //hit.transform.position += new Vector3(mouseDelta.x, mouseDelta.y, 0) * moveSpeed * Time.deltaTime;
            //lastMousePosition = currentMousePosition;

            if(hit.transform.gameObject.TryGetComponent(out Attractable target))
            {
                attractedObject = target;
                originalAttractedObjectDrag = attractedObject.rb.drag;
            }
                
        }

        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(magnetPoint.position + magnetPoint.forward * magnetDistance, 1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(magnetPoint.position + magnetPoint.forward * magnetRange, 1f);
    }
}
