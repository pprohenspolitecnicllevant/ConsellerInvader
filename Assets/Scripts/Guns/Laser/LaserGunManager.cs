using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class LaserGunManager : MonoBehaviour {
    //GUN HUD
    public TMP_Text counter;
    public Image panel;
    //SMOKE
    public GameObject smokeParticles;
    //AUDIO
    private AudioSource audioSource;
    public AudioClip audioTemp;
    //SHOOTING
    Animator anim;
    public bool isShooting = false;
    public bool isHot = false;
    //TEMPERATURE
    [SerializeField] private float timeCounterDuration;
    [SerializeField] private float coolSpeed;
    [SerializeField] private float heatSpeed;
    private float elapsedTime = 0.0f;
    Color newColor;
    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;

    void Start() {
        timeCounterDuration = StatsManager.player.laserGun.timeCounterDuration;
        coolSpeed = StatsManager.player.laserGun.coolSpeed;
        heatSpeed = StatsManager.player.laserGun.heatSpeed;

        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        smokeParticles.SetActive(false);
        Gradient();
    }

    public void Shooting() {
        anim.SetBool("isPullingTrigger", true);
        if (!isShooting) {
            isShooting = true;
        }
    }

    public void StopShooting() {
        anim.SetBool("isPullingTrigger", false);
        if (isShooting) {
            isShooting = false;
        }
    }

    void Update() {
        CheckIsShooting();
        CheckTemperature();
    }

    void CheckIsShooting() {
        if (isShooting && !isHot) {
            if (elapsedTime <= timeCounterDuration) {
                elapsedTime += Time.deltaTime * heatSpeed;
            } else {
                isHot = true;
                smokeParticles.SetActive(true);
            }
        } else if (!isShooting) {
            if (elapsedTime > 0) {
                elapsedTime -= Time.deltaTime * coolSpeed;
                if (elapsedTime < 0)
                    elapsedTime = 0;
            } else {
                ShootingAvailable();
            }
        }
    }

    void CheckTemperature() {
        if (!isHot) {
            counter.text = "" + (int) (elapsedTime*10) + "%";
            newColor = gradient.Evaluate(elapsedTime/timeCounterDuration);
            newColor.a = 0.5f;
            panel.color = newColor; 
        } else {
            PlayAudioTemp();
            counter.text = "HOT!!";
            newColor = gradient.Evaluate(elapsedTime/timeCounterDuration);
            newColor.a = 0.5f;
            panel.color = newColor; 
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

    void PlayAudioTemp() {
        audioSource.clip = audioTemp;
        audioSource.loop = true;
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    void ShootingAvailable() {
        newColor = Color.green;
        newColor.a = 0.5f;
        panel.color = newColor;
        isHot = false;
        audioSource.Stop();
        smokeParticles.SetActive(false);
    }
}
