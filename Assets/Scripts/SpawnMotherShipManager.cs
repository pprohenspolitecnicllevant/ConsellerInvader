using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMotherShipManager : MonoBehaviour {
    public HUDHolder hudHolder;
    public GameObject motherShipPrefab;
    public GameObject[] spawnPoints;

    void Start() {
        //spawnPoints[0].SetActive(true);
    }

    void Update() {
        switch (hudHolder.invaderCounter) {
            case 5:
                spawnPoints[1].SetActive(true);
                break;
            case 10:
                spawnPoints[2].SetActive(true);
                break;
            case 15:
                spawnPoints[3].SetActive(true);
                break;
            case 20:
                spawnPoints[4].SetActive(true);
                break;
            case 25:
                spawnPoints[5].SetActive(true);
                break;
            case 30:
                spawnPoints[6].SetActive(true);
                break;
        }
    }
}
