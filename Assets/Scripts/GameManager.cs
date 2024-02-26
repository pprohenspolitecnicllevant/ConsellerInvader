using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject deadMenu;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject floatingHud;
    [SerializeField] private GameObject tutorialHud;

    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftRay;
    [SerializeField] private GameObject rightRay;

    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject shield;

    [SerializeField] private SpawnMotherShipManager spawner;
    [SerializeField] private int motherShipsToWin;

    [SerializeField] private float startPauseTime = 7f;

    private int mothershipsDestroyed;

    private void Awake() 
    {
        StatsManager.Init();
        //StartCoroutine(StatsManager.InitCo());

        mothershipsDestroyed = 0;
    }

    private void OnEnable()
    {
        PlayerHealthController.PlayerDead += ShowDeadMenu;
        InvaderSpawner.MotherShipDestroyed += CheckWinMenu;
    }

    private void OnDisable()
    {
        PlayerHealthController.PlayerDead -= ShowDeadMenu;
        InvaderSpawner.MotherShipDestroyed -= CheckWinMenu;
    }

    private void Start()
    {
        //StartCoroutine(PauseForSeconds(startPauseTime));
    }

    private void ShowDeadMenu()
    {
        floatingHud.SetActive(false);
        deadMenu.SetActive(true);
        if (deadMenu.TryGetComponent(out DeadMenu dm))
        {
            dm.ResetPosition();
        }

        activations();

        mainCamera.fieldOfView = 179;

        Time.timeScale = 0;
    }

    private void CheckWinMenu()
    {
        mothershipsDestroyed++;
        if (mothershipsDestroyed < motherShipsToWin)
            return;

        floatingHud.SetActive(false);
        winMenu.SetActive(true);
        if (winMenu.TryGetComponent(out DeadMenu dm))
        {
            dm.ResetPosition();
        }

        activations();

        mainCamera.fieldOfView = 179;

        Time.timeScale = 0;
    }

    private void activations()
    {
        leftHand.SetActive(false);
        rightHand.SetActive(false);

        gun.SetActive(false);
        shield.SetActive(false);

        leftRay.SetActive(true);
        rightRay.SetActive(true);
    }

    IEnumerator PauseForSeconds(float seconds)
    {
        // Pausa el juego
        //Time.timeScale = 0;

        // Espera durante el tiempo especificado
        yield return new WaitForSecondsRealtime(seconds);

        spawner.spawnPoints[0].SetActive(true);
        tutorialHud.SetActive(false);
        // Restaura el tiempo normal
        //Time.timeScale = 1;
    }

    public void StartGame()
    {
        spawner.spawnPoints[0].SetActive(true);
        tutorialHud.SetActive(false);
        if (TryGetComponent(out MeshRenderer mr))
        {
            mr.enabled = false;
        }
    }
}
