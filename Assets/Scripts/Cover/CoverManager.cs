using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverManager : MonoBehaviour {
    private AudioSource audioSource;
    public AudioClip audioClipHit;
    public AudioClip audioClipBroken;

    public GameObject particles;

    public Material cover1;
    public Material cover2;
    public Material cover3;
    public Material cover4;
    
    [SerializeField] private float maxLife;
    private float life;
    private float actualLife;

    [SerializeField] private float damage;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        maxLife = StatsManager.tower.cover.maxLife;
        actualLife = life = maxLife;
    }

    void Update() {
        if (life != actualLife && life >= 0) {
            life = actualLife;

            if (life >= (maxLife * 0.75f)) {
                this.gameObject.GetComponent<MeshRenderer>().sharedMaterial = cover1;
            } else if ((life >= (maxLife * 0.5f)) && (life < (maxLife * 0.75f))) {
                this.gameObject.GetComponent<MeshRenderer>().sharedMaterial = cover2;
            } else if ((life >= (maxLife * 0.25f)) && (life < (maxLife * 0.5f))) {
                this.gameObject.GetComponent<MeshRenderer>().sharedMaterial = cover3;
            } else if ((life > (maxLife * 0f)) && (life < (maxLife * 0.25f))) {
                this.gameObject.GetComponent<MeshRenderer>().sharedMaterial = cover4;
            } else {
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                particles.SetActive(true);
                audioSource.clip = audioClipBroken;
                audioSource.Play();
                Destroy(this.gameObject, 1.0f);
            }
        }
    }

    public void TakeDamage() {  
        this.actualLife -= damage;
    }

    public void TakeDamage(float dmg) {
        this.actualLife -= dmg;    
    }

    void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.CompareTag("InvaderBullet")) {
            if (life > 0) {
                TakeDamage();
                audioSource.clip = audioClipHit;
                audioSource.Play();
            }
            Destroy(collision.gameObject);
        }
    }
}
