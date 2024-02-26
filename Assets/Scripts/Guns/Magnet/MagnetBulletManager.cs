using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBulletManager : MonoBehaviour {
    private MagnetGunManager magnetGun;

    void Start() {
        magnetGun = this.gameObject.transform.parent.gameObject.GetComponent<MagnetGunManager>();
    }

    void Update() {
        if (!magnetGun.isHot && magnetGun.isShooting) {
            foreach (Transform t in this.gameObject.transform) {
                if (!t.gameObject.activeSelf)
                    t.gameObject.SetActive(true);
                t.gameObject.GetComponent<ParticleSystem>().Play();
            }
        } else {
            foreach (Transform t in this.gameObject.transform) {
                if (t.gameObject.activeSelf)
                    //t.gameObject.active = false;
                    t.gameObject.GetComponent<ParticleSystem>().Stop();
            }
        }
    }
}